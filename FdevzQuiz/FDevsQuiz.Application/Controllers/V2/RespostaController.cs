using FDevsQuiz.Domain.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FDevsQuiz.Application.Controllers.V2
{
    [Controller]
    [Route("v2/resposta")]
    public class RespostaController : ControllerBase
    {
        private readonly IRespostaRepository _respostaRepository;

        public RespostaController(IRespostaRepository respostaRepository)
        {
            _respostaRepository = respostaRepository;
        }
    }
}
