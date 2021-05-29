using FDevsQuiz.Domain.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FDevsQuiz.Application.Controllers.V2
{
    [Controller]
    [Route("v2/pergunta")]
    public class PerguntaController : ControllerBase
    {
        private readonly IPerguntaRepository _perguntaRepository;

        public PerguntaController(IPerguntaRepository perguntaRepository)
        {
            _perguntaRepository = perguntaRepository;
        }
    }
}
