using FdevzQuiz.Command;
using FdevzQuiz.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace FdevzQuiz.Controllers
{
    [Route("api/quiz")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private ICollection<T> CarregarData<T>()
        {
            using var openStream = System.IO.File.OpenRead(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Quiz.json"));
            return JsonSerializer.DeserializeAsync<ICollection<T>>(openStream, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }).Result;
        }

        private async Task SalvarDados(ICollection<QuizModel> dados)
        {
            using var createStream = System.IO.File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Quiz.json"));
            await JsonSerializer.SerializeAsync(createStream, dados, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        [HttpGet("recuperarQuiz")]
        public ActionResult<ICollection<dynamic>> Quizzes()
        {
            var quizzes = CarregarData<dynamic>();
            return Ok(quizzes);
        }

        [HttpPost("adicionarQuiz")]
        public async Task<ActionResult<QuizModel>> AdicionarQuiz([FromBody] QuizCommand quizCommand)
        {
            if (!ModelState.IsValid)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Falta Dados para fazer a inserção de dados!");

            if (quizCommand == null)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Falta Dados para fazer a inserção de dados!");

            if (quizCommand.Perguntas.Alternativas.ToString().Length > 5)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Máximo de alternativas são 4!");

            if (!string.IsNullOrEmpty(quizCommand.Titulo) ||
                !string.IsNullOrEmpty(quizCommand.Perguntas.Titulo) ||
                !string.IsNullOrEmpty(quizCommand.Perguntas.Descricao) ||
                !string.IsNullOrEmpty(quizCommand.Perguntas.Alternativas.Titulo))
                return StatusCode((int)HttpStatusCode.InternalServerError, "Falta Dados para fazer a inserção de dados!");

            var quizes = CarregarData<QuizModel>();
            quizCommand.CodigoQuiz = quizes.Select(u => u.CodigoQuiz).ToList().Max() + 1;

            var quiz = new QuizModel
            {
                Titulo = quizCommand.Titulo,
                Respostas = 0,
                ImagemUrl = quizCommand.ImagemUrl,
                Nivel = quizCommand.Nivel,
                Perguntas = new PerguntasModel
                {
                    CodigoQuiz = quizCommand.CodigoQuiz,
                    Titulo = quizCommand.Perguntas.Titulo,
                    Descricao = quizCommand.Perguntas.Descricao,
                    Alternativas = new AlternativasModel
                    {
                        CodigoQuiz = quizCommand.CodigoQuiz,
                        Titulo = quizCommand.Perguntas.Alternativas.Titulo,
                        Correta = quizCommand.Perguntas.Alternativas.Correta
                    }
                }
            };

            quiz.Perguntas.CodigoPergunta = quizes.Select(u => u.Perguntas.CodigoPergunta).ToList().Max() + 1;
            quiz.Perguntas.Alternativas.CodigoAlternativa = quizes.Select(u => u.Perguntas.Alternativas.CodigoAlternativa).ToList().Max() + 1;
            quizes.Add(quiz);

            await SalvarDados(quizes);

            return Created("quizz/{id}", quizes);
        }

        [HttpPut("alterarQuiz")]
        public async Task<ActionResult<QuizModel>> AlterarQuiz([FromBody] QuizCommand quizCommand)
        {
            if (!ModelState.IsValid)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Falta Dados para fazer a inserção de dados!");

            if (quizCommand == null)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Falta Dados para fazer a inserção de dados!");

            if (quizCommand.Perguntas.Alternativas.ToString().Length > 5)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Máximo de alternativas são 4!");

            if (!string.IsNullOrEmpty(quizCommand.Titulo) ||
                !string.IsNullOrEmpty(quizCommand.Perguntas.Titulo) ||
                !string.IsNullOrEmpty(quizCommand.Perguntas.Descricao) ||
                !string.IsNullOrEmpty(quizCommand.Perguntas.Alternativas.Titulo))
                return StatusCode((int)HttpStatusCode.InternalServerError, "Falta Dados para fazer a inserção de dados!");

            var quizes = CarregarData<QuizModel>();
            var quiz = quizes.Where(u => u.CodigoQuiz == quizCommand.CodigoQuiz).FirstOrDefault();

            quiz.ImagemUrl = quizCommand.ImagemUrl;
            quiz.Nivel = quizCommand.Nivel;
            quiz.Titulo = quizCommand.Titulo;
            quiz.Respostas = quizCommand.Respostas;
            quiz.Perguntas.Descricao = quizCommand.Perguntas.Descricao;
            quiz.Perguntas.Titulo = quiz.Perguntas.Titulo;
            quiz.Perguntas.Alternativas.Titulo = quizCommand.Perguntas.Alternativas.Titulo;
            quiz.Perguntas.Alternativas.Correta = quizCommand.Perguntas.Alternativas.Correta;
            quizes.Add(quiz);

            await SalvarDados(quizes);

            return NoContent();
        }

        [HttpDelete("excluirQuizz")]
        public async Task<ActionResult> ExcluirUsuario([FromRoute] long id)
        {
            var quizes = CarregarData<QuizModel>();
            quizes = quizes
                .Where(u => u.Perguntas.Alternativas.CodigoQuiz != id )
                .Where(u => u.Perguntas.CodigoQuiz != id)
                .Where(u => u.CodigoQuiz != id)
                .ToList();

            await SalvarDados(quizes);

            return NoContent();
        }
    }
}
