using Microsoft.AspNetCore.Mvc;

namespace FdevzQuiz.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        [HttpGet("recuperarUsuario")]
        public ActionResult RecuperarUsuario()
        {
            return Ok(new
            {
                CodigoUsuario = 1,
                NomeUsuario = "Alexandre",
                CodigoQuiz = 4,
                NomeQuiz = "JSON (JavaScript Object Notation)",
                CodigoDificuldade = 3,
                DescricaoDificuldade = "Dificíl",
                PontuacaoUsuario = 15
            });
        }
    }
}
