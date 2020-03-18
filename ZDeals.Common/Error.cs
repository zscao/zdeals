namespace ZDeals.Common
{
    public abstract class Error
    {
        /// <summary>
        /// codes are defined in modules
        /// </summary>
        public int Code { get; set; }
        public string Message { get; set; }
    }


    public class InternalError : Error
    {

    }

    public class UnknownError : Error
    {

    }

    public class BadRequestError: Error
    {

    }

    public class NotFoundError: Error
    {

    }
}
