namespace IMDbion_MovieHandlerService.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message)
            : base(message)
        {
        }
    }
}
