namespace FC.BuildingBlocks.Core.Exception
{
    public class NotFoundException : System.Exception
    {
        public NotFoundException() : base("O recurso não foi encontrado.") { }

        public NotFoundException(string resourceName, object key)
    :       base($"{resourceName} com identificador '{key}' não foi encontrado.") { }

        public NotFoundException(string message)
            : base(message) { }

        public NotFoundException(string message, System.Exception innerException)
            : base(message, innerException) { }
    }
}
