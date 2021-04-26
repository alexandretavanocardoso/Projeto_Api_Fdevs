using Microsoft.AspNetCore.Mvc;

namespace FdevzQuiz.Controllers
{
    [Route("api/quiz")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        [HttpGet("recuperarQuiz")]
        public ActionResult RecuperarQuiz()
        {
            return Ok(new {
                CodigoUsuario = 1,
                CodigoQuiz = 1,
                NomeQuiz = "Estrutura Banco de Dados",
                CodigoDificuldade = 3,
                DescricaoDificuldade = "Dificíl",
                PontuacaoUsuario = 15
            });
        }
    }
}
