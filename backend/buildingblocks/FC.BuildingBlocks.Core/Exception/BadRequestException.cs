namespace FC.BuildingBlocks.Core.Exception
{
    public class BadRequestException : System.Exception
    {
        public BadRequestException(string errorCode) : base(errorCode) { }

        public BadRequestException(string errorCode, System.Exception innerException)
            : base(errorCode, innerException) { }
    }
}
