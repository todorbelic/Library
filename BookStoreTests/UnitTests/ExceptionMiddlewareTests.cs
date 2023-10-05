using BookStoreAPI.Middleware;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Net;

namespace BookStoreTests.UnitTests
{
    public class ExceptionMiddlewareTests
    {
        [Fact]
        public async Task Given_Exception_When_ExceptionMiddlewareInvoked_Then_Return500StatusCode()
        {
            //arrange
            var expectedException = new Exception();
            RequestDelegate mockNextMiddleware = (HttpContext) =>
            {
                return Task.FromException(expectedException);
            };
            var httpContext = new DefaultHttpContext();

            var exceptionHandlingMiddleware = new Mock<ExceptionMiddleware>();

            //act
            await exceptionHandlingMiddleware.Object.InvokeAsync(httpContext, mockNextMiddleware);

            //assert
            Assert.Equal(HttpStatusCode.InternalServerError, (HttpStatusCode)httpContext.Response.StatusCode);
        }
    }
}
