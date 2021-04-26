using Microsoft.AspNetCore.Mvc;

namespace FdevzQuiz.Controllers
{
    [Route("api/dificuldade")]
    [ApiController]
    public class DificuldadeController : ControllerBase
    {
        [HttpGet("recuperarDificuldade")]
        public ActionResult RecuperarDificuldade()
        {
            return Ok(new {
                CodigoUsuario = 1,
                CodigoQuiz = 4,
                NomeQuiz = "JSON (JavaScript Object Notation)",
                PontuacaoUsuario = 15
            });
        }
    }
}
