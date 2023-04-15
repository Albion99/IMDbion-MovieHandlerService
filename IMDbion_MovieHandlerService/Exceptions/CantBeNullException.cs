namespace IMDbion_MovieHandlerService.Exceptions
{
    public class CantBeNullException : Exception
    {
        public CantBeNullException(string message)
            : base(message)
        {
        }
    }
}
