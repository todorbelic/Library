using BookStoreAPI.DTO.User;
using BookStoreAPI.Exceptions;
using BookStoreAPI.Options;
using BookStoreAPI.Service.Interface;
using BookStoreAPI.Util;
using BookStoreClassLibrary.Core.Entities;
using BookStoreClassLibrary.Core.Security;
using BookStoreClassLibrary.Core.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace BookStoreAPI.Service
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtUtils _jwtUtils;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtOptions _options;

        public UserAuthenticationService(UserManager<User> userManager, IJwtUtils jwtUtils, 
                                         IUnitOfWork unitOfWork, IOptions<JwtOptions> options)
        {
            _userManager = userManager;
            _jwtUtils = jwtUtils;
            _unitOfWork = unitOfWork;
            _options = options.Value;
        }

        private async Task<AuthenticateResponseDTO> CreateAuthenticateResponse(User? user, string ipAddress, string userAgent)
        {
            var accessToken = await _jwtUtils.CreateTokenAsync(user);
            DateTime validTo = accessToken.ValidTo;
            string accessTokenSerialized = new JwtSecurityTokenHandler().WriteToken(accessToken);
            RefreshToken refreshToken = await _jwtUtils.GenerateRefreshToken(user.Id, ipAddress, userAgent);
            RemoveOldRefreshTokens(user);
            return new AuthenticateResponseDTO(accessTokenSerialized, refreshToken.Value, validTo);
        }

        public async Task<AuthenticateResponseDTO?> ValidateUserLogin(UserLoginDTO loginDto, string ipAddress, string userAgent)
        {
            User user = await _userManager.FindByEmailAsync(loginDto.Email);
            var validCredentials = user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!validCredentials)
            {
                throw new LoginException();
            }
            RemoveOldRefreshTokens(user);
            return await CreateAuthenticateResponse(user, ipAddress, userAgent);
        }

        private void RemoveOldRefreshTokens(User? user)
        {
            var refreshTokenTTL = Convert.ToDouble(_options.RefreshTokenExpiresInDays);
            List<RefreshToken> oldRefreshTokens = _unitOfWork.RefreshTokenRepository
                                                             .FindByCondition(t => t.UserId.ToString() == user.Id                                                                      && t.Revoked != null
                                                                           && t.Created.AddDays(refreshTokenTTL) < DateTime.UtcNow).ToList();
            
            oldRefreshTokens.ForEach(t => _unitOfWork.RefreshTokenRepository.Delete(t));
        }

        public async Task<AuthenticateResponseDTO> RefreshToken(RefreshTokenRequestDTO refreshTokenRequest, string ipAddress, string userAgent)
        {
            RefreshToken? refreshToken = _unitOfWork.RefreshTokenRepository
                                                    .FindByCondition(token => token.Value == refreshTokenRequest.RefreshToken).FirstOrDefault();
            if (refreshToken == null)
                throw new InvalidTokenException();

            if (refreshToken.IsRevoked)
            {
                await RevokeDescendantRefreshTokens(refreshToken, ipAddress);
                throw new InvalidTokenException();
            }

            if ((refreshToken.UserId.ToString() == _jwtUtils.ValidateJwtToken(refreshTokenRequest.AccessToken))
              && refreshToken.CreatedByIp.Equals(ipAddress) && refreshToken.UserAgent.Equals(userAgent))
            {
                User user = await _userManager.FindByIdAsync(refreshToken.UserId.ToString());
                var authenticateResponse = await CreateAuthenticateResponse(user, ipAddress, userAgent);
                await RevokeRefreshToken(refreshToken, ipAddress, authenticateResponse.RefreshToken);
                return authenticateResponse;
            }
            else
            {
                throw new InvalidTokenException();
            }
        }

        private async Task RevokeDescendantRefreshTokens(RefreshToken refreshToken, string ipAddress)
        {
            if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
            {
                var childToken = _unitOfWork.RefreshTokenRepository
                                            .FindByCondition(x => x.Value == refreshToken.ReplacedByToken).Single();
                if (childToken.IsActive)
                    await RevokeRefreshToken(childToken, ipAddress);
                else
                    await RevokeDescendantRefreshTokens(childToken, ipAddress);
            }
        }

        private async Task RevokeRefreshToken(RefreshToken token, string? ipAddress, string? replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReplacedByToken = replacedByToken;
            _unitOfWork.RefreshTokenRepository.Update(token);
            await _unitOfWork.CompleteAsync();
        }

        public async Task RevokeAllRefreshTokens(string? ipAddress)
        {
            List<RefreshToken> refreshTokens = _unitOfWork.RefreshTokenRepository.GetAll().ToList();
            foreach (RefreshToken refreshToken in refreshTokens) 
            { 
                await RevokeRefreshToken(refreshToken, ipAddress);
            }
        }

        public async Task RevokeAllRefreshTokensForUser(RevokeUserTokenRequest revokeRequest, string? ipAddress)
        {
            User user = await _userManager.FindByEmailAsync(revokeRequest.UserEmail);
            if(user == null)
            {
                throw new NotFoundException("User with provided email doesn't exist");
            }
            List<RefreshToken> userRefreshTokens =  _unitOfWork.RefreshTokenRepository.FindByCondition(token => token.UserId.ToString() == user.Id).ToList();
            foreach (RefreshToken refreshToken in userRefreshTokens)
            {
                await RevokeRefreshToken(refreshToken, ipAddress);
            }
        }
    }
}
