namespace ZDeals.Common
{
    public class Error
    {
        public ErrorType Type { get; private set; }
        /// <summary>
        /// codes are defined in modules
        /// </summary>
        public int Code { get; set; }
        public string? Message { get; set; }

        public Error(ErrorType type)
        {
            this.Type = type;
        }
    }

    public enum ErrorType
    {
        Internal = -1,
        Unknown = 0,
        Validation = 1,
        Authentication = 2,
        NotFound = 3,
        BadRequest = 4,
    }
}
