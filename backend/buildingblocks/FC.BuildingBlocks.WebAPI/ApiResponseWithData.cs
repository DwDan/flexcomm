namespace FC.BuildingBlocks.WebAPI
{
    public class ApiResponseWithData<T> : ApiResponse
    {
        public T? Data { get; set; }
    }
}
