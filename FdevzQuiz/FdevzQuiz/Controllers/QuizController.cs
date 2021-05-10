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

        private async Task<ICollection<T>> SalvarDados<T>(ICollection<T> dados)
        {
            using var createStream = System.IO.File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Quiz.json"));
            await JsonSerializer.SerializeAsync(createStream, dados, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return dados;
        }

        [HttpGet("recuperarQuiz")]
        public ActionResult<ICollection<dynamic>> Quizzes()
        {
            var quizzes = CarregarData<dynamic>();
            return Ok(quizzes);
        }

        [HttpPost("adicionarQuiz")]
        public async Task<ActionResult<QuizModel>> AdicionarQuiz([FromBody] QuizCommand quizCommand, [FromBody] PerguntasCommand perguntasCommand, [FromBody] AlternativasCommand alternativasCommand)
        {
            if (!ModelState.IsValid)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Máximo de alternativas é 4!");

            if (quizCommand == null)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Falta Dados para fazer a inserção de dados!");

            foreach (var item in quizCommand.Perguntas)
            {
                if (item.Alternativas.Count > 4)
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Falta Dados para fazer a inserção de dados!");

                foreach (var item2 in item.Alternativas)
                {
                    if (string.IsNullOrEmpty(quizCommand.Titulo) ||
                        string.IsNullOrEmpty(item.Titulo) ||
                        string.IsNullOrEmpty(item.Descricao) ||
                        string.IsNullOrEmpty(item2.Titulo))
                        return StatusCode((int)HttpStatusCode.InternalServerError, "Falta Dados para fazer a inserção de dados!");
                }
            }

            var quizes = CarregarData<QuizModel>();
            quizCommand.CodigoQuiz = Guid.NewGuid();
            perguntasCommand.CodigoPergunta = Guid.NewGuid();
            alternativasCommand.CodigoAlternativa = Guid.NewGuid();

            var quiz = new QuizModel
            {
                Titulo = quizCommand.Titulo,
                Respostas = 0,
                ImagemUrl = quizCommand.ImagemUrl,
                Nivel = quizCommand.Nivel,
                //perguntas = new perguntasmodel()
                //{
                //    codigopergunta = perguntascommand.codigopergunta,
                //    codigoquiz = quizcommand.codigoquiz,
                //    titulo = perguntascommand.titulo,
                //    descricao = perguntascommand.descricao,
                //    alternativas = new alternativasmodel()
                //    {
                //        codigoquiz = quizcommand.codigoquiz,
                //        codigopergunta = perguntascommand.codigopergunta,
                //        codigoalternativa = alternativascommand.codigoalternativa,
                //        titulo = alternativascommand.titulo,
                //        correta = alternativascommand.correta
                //    }
                //}
            };

            quizes.Add(quiz);

            await SalvarDados(quizes);

            return Created("quizz/{id}", quizes);
        }

        [HttpPut("alterarQuiz")]
        public async Task<ActionResult<QuizModel>> AlterarQuiz([FromBody] QuizCommand quizCommand, [FromBody] PerguntasCommand perguntasCommand, [FromBody] AlternativasCommand alternativasCommand)
        {
            if (!ModelState.IsValid)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Falta Dados para fazer a inserção de dados!");

            if (quizCommand == null)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Falta Dados para fazer a inserção de dados!");

            foreach (var item in quizCommand.Perguntas)
            {
                if (item.Alternativas.Count > 4)
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Máximo de alternativas é 4!");

                foreach (var item2 in item.Alternativas)
                {
                    if (string.IsNullOrEmpty(quizCommand.Titulo) ||
                        string.IsNullOrEmpty(item.Titulo) ||
                        string.IsNullOrEmpty(item.Descricao) ||
                        string.IsNullOrEmpty(item2.Titulo))
                        return StatusCode((int)HttpStatusCode.InternalServerError, "Falta Dados para fazer a alteração de dados!");
                }
            }

            var quizesQuiz = CarregarData<QuizModel>();
            var quizesPergunta = CarregarData<PerguntasModel>();
            var quizesAlternativa = CarregarData<AlternativasModel>();

            var quiz = quizesQuiz
                .Where(u => u.CodigoQuiz == quizCommand.CodigoQuiz)
                .FirstOrDefault();

            var quizPergunta = quizesPergunta
                .Where(u => u.CodigoPergunta == perguntasCommand.CodigoPergunta && u.CodigoQuiz == quiz.CodigoQuiz)
                .FirstOrDefault();

            var quizAlternativa = quizesAlternativa
                .Where(u => u.CodigoAlternativa == alternativasCommand.CodigoAlternativa && u.CodigoQuiz == quiz.CodigoQuiz && u.CodigoPergunta == quizPergunta.CodigoPergunta)
                .FirstOrDefault();

            quiz.ImagemUrl = quizCommand.ImagemUrl;
            quiz.Nivel = quizCommand.Nivel;
            quiz.Titulo = quizCommand.Titulo;
            quiz.Respostas = quizCommand.Respostas;

            perguntasCommand.Descricao = perguntasCommand.Descricao;
            perguntasCommand.Titulo = perguntasCommand.Titulo;

            quizPergunta.Titulo = alternativasCommand.Titulo;
            quizAlternativa.Correta = alternativasCommand.Correta;

            quizesQuiz.Add(quiz);
            quizesPergunta.Add(quizPergunta);
            quizesAlternativa.Add(quizAlternativa);

            await SalvarDados(quizesQuiz);
            await SalvarDados(quizesPergunta);
            await SalvarDados(quizesAlternativa);

            return NoContent();
        }

        [HttpDelete("excluirQuizz")]
        public async Task<ActionResult> ExcluirUsuario([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Quiz não existente!");

            var quizes = CarregarData<QuizModel>();
            quizes = quizes
                .Where(u => u.CodigoQuiz != id)
                .ToList();

            await SalvarDados(quizes);

            return NoContent();
        }
    }
}
