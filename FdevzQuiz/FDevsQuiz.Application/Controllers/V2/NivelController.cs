using FDevsQuiz.Domain.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FDevsQuiz.Application.Controllers.V2
{
    [Controller]
    [Route("v2/nivel")]
    public class NivelController : ControllerBase
    {
        private readonly INivelRepository _nivelRepository;

        public NivelController(INivelRepository nivelRepository)
        {
            _nivelRepository = nivelRepository;
        }
    }
}
