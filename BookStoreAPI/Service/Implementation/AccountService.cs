using AutoMapper;
using BookStoreAPI.DTO.User;
using BookStoreAPI.Exceptions;
using BookStoreAPI.Service.Interface;
using BookStoreClassLibrary.Core.Entities;
using BookStoreClassLibrary.Core.Util;
using Microsoft.AspNetCore.Identity;

namespace BookStoreAPI.Service.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public AccountService(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IdentityResult> RegisterUserAsync(UserRegistrationDTO userRegistration)
        {
            var user = _mapper.Map<User>(userRegistration);
            var result = await _userManager.CreateAsync(user, userRegistration.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, userRegistration.Role);
            }
            return result;
        }

        public async Task<UserDTO> GetProfile(string? id)
        {
            var user = await GetById(id);
            var userDTO = _mapper.Map<UserDTO>(user);
            return userDTO;
        }

        public async Task UpdateUser(string? id, UserDTO userDTO)
        {
            var user = await GetById(id);
            _mapper.Map(userDTO, user);
            await _userManager.UpdateAsync(user);
        }

        public async Task<User> GetById(string? id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            return user;
        }

        public async Task UploadAvatar(string id, IFormFile avatar)
        {
            var user = await GetById(id);
            byte[] imageConverted = await avatar.GetBytes();
            user.Avatar = imageConverted;
            await _userManager.UpdateAsync(user);
        }

        public async Task<byte[]> GetAvatar(string? id)
        {
            var user = await GetById(id);
            if (user.Avatar == null || user.Avatar.Length == 0)
            {
                throw new NotFoundException("Avatar not found");
            }
            return user.Avatar;
        }

        public async Task RemoveAvatar(string? id)
        {
            var user = await GetById(id);
            user.Avatar = null;
            await _userManager.UpdateAsync(user);
        }
    }
}
