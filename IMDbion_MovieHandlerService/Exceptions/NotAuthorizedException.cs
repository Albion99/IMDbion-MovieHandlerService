namespace IMDbion_MovieHandlerService.Exceptions
{
    public class NotAuthorizedException : Exception
    {
        public NotAuthorizedException(string message)
            : base(message)
        {
        }
    }
}
