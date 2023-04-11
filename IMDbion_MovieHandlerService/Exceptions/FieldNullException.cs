namespace IMDbion_MovieHandlerService.Exceptions
{
    public class FieldNullException : Exception
    {
        public FieldNullException(string message)
            : base(message)
        {
        }
    }
}
