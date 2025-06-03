using FC.BuildingBlocks.Core.Exception;
using Microsoft.AspNetCore.Mvc;

namespace FC.BuildingBlocks.WebAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult Ok<T>(T data) =>
                base.Ok(new ApiResponseWithData<T> { Data = data, Success = true });

        protected void EnsureRouteMatchesBodyId(Guid routeId, Guid bodyId, string? message = null)
        {
            if (routeId != bodyId)
                throw new BadRequestException(message ?? "O ID da rota difere do corpo da requisição.");
        }
    }
}
