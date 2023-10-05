namespace BookStoreAPI.Exceptions
{
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException(string message = "Invalid token(s)") : base(message) { }

        public InvalidTokenException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
