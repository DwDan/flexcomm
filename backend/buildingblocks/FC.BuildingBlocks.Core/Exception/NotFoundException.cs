namespace FC.BuildingBlocks.Core.Exception
{
    public class NotFoundException : System.Exception
    {
        public NotFoundException() : base("Error.ResourceNotFound") { }

        public NotFoundException(string errorCode)
            : base(errorCode) { }

        public NotFoundException(string errorCode, System.Exception innerException)
            : base(errorCode, innerException) { }
    }
}
