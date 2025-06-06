namespace FC.BuildingBlocks.Core.Exception;

public class BusinessException : System.Exception
{
    public BusinessException(string errorCode) : base(errorCode) { }

    public BusinessException(string errorCode, System.Exception innerException)
        : base(errorCode, innerException) { }
}
