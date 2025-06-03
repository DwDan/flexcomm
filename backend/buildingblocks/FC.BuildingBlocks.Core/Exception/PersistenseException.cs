namespace FC.BuildingBlocks.Core.Exception
{
    public class PersistenceException : System.Exception
    {
        public PersistenceException() { }

        public PersistenceException(string message)
            : base(message) { }

        public PersistenceException(string message, System.Exception innerException)
            : base(message, innerException) { }
    }
}
