using AutoMapper;
using FC.Auth.Application.Usuarios.CriarUsuario;
using FC.Auth.WebAPI.Feature.Usuario.CriarUsuario;
using FC.BuildingBlocks.WebAPI;
using MediatR;
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
    }
}
