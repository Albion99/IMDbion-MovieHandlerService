using IMDbion_MovieHandlerService.Exceptions;

namespace IMDbion_MovieHandlerService.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }
    }
}
