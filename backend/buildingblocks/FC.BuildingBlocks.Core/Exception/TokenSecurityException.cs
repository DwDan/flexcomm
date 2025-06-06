namespace FC.BuildingBlocks.Core.Exception
{
    public class TokenSecurityException : System.Exception
    {
        public TokenSecurityException() { }

        public TokenSecurityException(string errorCode)
            : base(errorCode) { }

        public TokenSecurityException(string errorCode, System.Exception innerException)
            : base(errorCode, innerException) { }
    }
}
