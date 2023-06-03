namespace IMDbion_MovieHandlerService.Exceptions
{
    public class InvalidJWTTokenException : Exception
    {
        public InvalidJWTTokenException(string message)
            : base(message)
        {
        }
    }
}
