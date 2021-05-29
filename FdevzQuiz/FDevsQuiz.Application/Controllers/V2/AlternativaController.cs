using FDevsQuiz.Domain.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FDevsQuiz.Application.Controllers.V2
{
    [Controller]
    [Route("v2/alternativa")]
    public class AlternativaController : ControllerBase
    {
        private readonly IAlternativaRepository _alternativaRepository;

        public AlternativaController(IAlternativaRepository alternativaRepository)
        {
            _alternativaRepository = alternativaRepository;
        }
    }
}
