namespace FC.BuildingBlocks.Core.Exception
{
    public class PersistenceException : System.Exception
    {
        public PersistenceException() { }

        public PersistenceException(string errorCode)
            : base(errorCode) { }

        public PersistenceException(string errorCode, System.Exception innerException)
            : base(errorCode, innerException) { }
    }
}
