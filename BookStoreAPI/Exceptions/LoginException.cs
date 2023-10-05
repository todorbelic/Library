namespace BookStoreAPI.Exceptions
{
    public class LoginException : Exception
    {
        public LoginException(string message = "Invalid credentials") : base(message) { }
        public LoginException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}