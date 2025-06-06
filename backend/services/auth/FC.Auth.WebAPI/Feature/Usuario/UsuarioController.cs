using AutoMapper;
using FC.Auth.Application.Usuarios.AlterarUsuario;
using FC.Auth.Application.Usuarios.ConfirmarEmail;
using FC.Auth.Application.Usuarios.CriarUsuario;
using FC.Auth.WebAPI.Feature.Usuario.AlterarUsuario;
using FC.Auth.WebAPI.Feature.Usuario.CriarUsuario;
using FC.BuildingBlocks.WebAPI;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FC.Auth.WebAPI.Feature.Usuario
{
    public class UsuarioController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UsuarioController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CriarUsuarioRequest request, CancellationToken cancellationToken)
        {
            var command = _mapper.Map<CriarUsuarioCommand>(request);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] AlterarUsuarioRequest request, CancellationToken cancellationToken)
        {
            EnsureRouteMatchesBodyId(id, request.Id);

            var command = _mapper.Map<AlterarUsuarioCommand>(request);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpGet("confirmar-email")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string token, CancellationToken cancellationToken)
        {
            var command = new ConfirmarEmailUsuarioCommand(token);

            var response = await _mediator.Send(command, cancellationToken);

            return Ok(response);
        }
    }
}
