using Microsoft.AspNetCore.Mvc;

namespace FC.BuildingBlocks.WebAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult Ok<T>(T data) =>
                base.Ok(new ApiResponseWithData<T> { Data = data, Success = true });
    }
}
