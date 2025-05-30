using AutoMapper;
using FC.Auth.Application.Autenticacao.Login;
using FC.Auth.WebAPI.Feature.Autenticacao.Login;
using FC.BuildingBlocks.WebAPI;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FC.Auth.WebAPI.Feature.Autenticacao
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public AutenticacaoController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<LoginCommand>(request);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }
    }
}
