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
        private ICollection<T> CarregarData<T>(string path)
        {
            using var openStream = System.IO.File.OpenRead(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", $"{path}.json"));
            return JsonSerializer.DeserializeAsync<ICollection<T>>(openStream, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }).Result;
        }

        private async Task<ICollection<T>> SalvarDados<T>(ICollection<T> dados, string path)
        {
            using var createStream = System.IO.File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", $"{path}.json"));
            await JsonSerializer.SerializeAsync(createStream, dados, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return dados;
        }

        [HttpGet("recuperarQuiz")]
        public ActionResult<ICollection<QuizModel>> RecuperarQuiz()
        {
            var quiz = CarregarData<QuizModel>("Quiz");
            return Ok(quiz);
        }

        [HttpGet("recuperarPergunta")]
        public ActionResult<ICollection<QuizModel>> RecuperarPergunta()
        {
            var Pergunta = CarregarData<PerguntasModel>("Perguntas");
            return Ok(Pergunta);
        }

        [HttpGet("recuperarAlternativa")]
        public ActionResult<ICollection<AlternativasModel>> RecuperarAlternativa()
        {
            var alternativa = CarregarData<AlternativasModel>("Alternativas");
            return Ok(alternativa);
        }

        [HttpPost("adicionarQuiz")]
        public async Task<ActionResult<QuizModel>> AdicionarQuiz([FromBody] QuizCommand quizCommand)
        {
            if (!ModelState.IsValid)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Máximo de alternativas é 4!");

            if (quizCommand == null)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Falta Dados para fazer a inserção de dados!");

            if (string.IsNullOrEmpty(quizCommand.Titulo) || string.IsNullOrEmpty(quizCommand.Nivel))
                return StatusCode((int)HttpStatusCode.InternalServerError, "Falta Dados para fazer a inserção de dados!");

            var quizes = CarregarData<QuizModel>("Quiz");

            var quiz = new QuizModel
            {
                CodigoQuiz = Guid.NewGuid(),
                Titulo = quizCommand.Titulo,
                Respostas = 0,
                ImagemUrl = quizCommand.ImagemUrl,
                Nivel = quizCommand.Nivel
            };

            quizes.Add(quiz);

            await SalvarDados(quizes, "Quiz");

            return Created("quizz/{id}", quizes);
        }

        [HttpPost("adicionarPergunta")]
        public async Task<ActionResult<PerguntasModel>> AdicionarPergunta([FromBody] PerguntasCommand perguntasCommand)
        {
            if (!ModelState.IsValid)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Máximo de alternativas é 4!");

            if (perguntasCommand == null)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Falta Dados para fazer a inserção de dados!");

            if (string.IsNullOrEmpty(perguntasCommand.Titulo) || string.IsNullOrEmpty(perguntasCommand.Descricao))
                return StatusCode((int)HttpStatusCode.InternalServerError, "Falta Dados para fazer a inserção de dados!");


            var perguntas = CarregarData<PerguntasModel>("Perguntas");

            var pergunta = new PerguntasModel
            {
                CodigoQuiz = perguntasCommand.CodigoQuiz,
                CodigoPergunta = Guid.NewGuid(),
                Titulo = perguntasCommand.Titulo,
                Descricao = perguntasCommand.Descricao
            };

            perguntas.Add(pergunta);

            await SalvarDados(perguntas, "Perguntas");

            return Created("perguntas/{id}", perguntas);
        }

        [HttpPost("adicionarAlternativa")]
        public async Task<ActionResult<AlternativasModel>> AdicionarAlternativa([FromBody] List<AlternativasCommand> alternativasCommand)
        {
            if (!ModelState.IsValid)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Máximo de alternativas é 4!");

            if (alternativasCommand == null)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Falta Dados para fazer a inserção de dados!");

            foreach (var item in alternativasCommand)
            {
                if (string.IsNullOrEmpty(item.Titulo))
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Falta Dados para fazer a inserção de dados!");
            }

            if (alternativasCommand.Count > 4)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Máximo 4 alternativas!");

            var alternativas = CarregarData<AlternativasModel>("Alternativas");

            foreach (var item in alternativasCommand)
            {
                var alternativa = new AlternativasModel
                {
                    CodigoAlternativa = Guid.NewGuid(),
                    CodigoQuiz = item.CodigoQuiz,
                    CodigoPergunta = item.CodigoPergunta,
                    Titulo = item.Titulo,
                    Correta = item.Correta
                };

                alternativas.Add(alternativa);

                await SalvarDados(alternativas, "Perguntas");
            }

            return Created("alternativas/{id}", alternativas);
        }

        [HttpPut("alterarQuiz")]
        public async Task<ActionResult<QuizModel>> AlterarQuiz([FromBody] QuizCommand quizCommand)
        {
            if (!ModelState.IsValid)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Máximo de alternativas é 4!");

            if (quizCommand == null)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Falta Dados para fazer a inserção de dados!");

            if (string.IsNullOrEmpty(quizCommand.Titulo) || string.IsNullOrEmpty(quizCommand.Nivel))
                return StatusCode((int)HttpStatusCode.InternalServerError, "Falta Dados para fazer a inserção de dados!");

            var quizes = CarregarData<QuizModel>("Quiz");
            var quiz = quizes.Where(u => u.CodigoQuiz == quizCommand.CodigoQuiz).FirstOrDefault();

            if (quiz == null)
                throw new Exception("Quiz não existe!");

            quiz.CodigoQuiz = quizCommand.CodigoQuiz;
            quiz.ImagemUrl = quizCommand.ImagemUrl;
            quiz.Nivel = quizCommand.Nivel;
            quiz.Respostas = quizCommand.Respostas;
            quiz.Titulo = quizCommand.Titulo;
            quizes.Add(quiz);

            await SalvarDados(quizes, "Quiz");

            return NoContent();
        }

        [HttpPut("alterarPergunta")]
        public async Task<ActionResult<PerguntasModel>> AlterarPergunta([FromBody] PerguntasCommand perguntasCommand)
        {
            if (!ModelState.IsValid)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Máximo de alternativas é 4!");

            if (perguntasCommand == null)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Falta Dados para fazer a inserção de dados!");

            if (string.IsNullOrEmpty(perguntasCommand.Titulo) || string.IsNullOrEmpty(perguntasCommand.Descricao))
                return StatusCode((int)HttpStatusCode.InternalServerError, "Falta Dados para fazer a inserção de dados!");


            var perguntas = CarregarData<PerguntasModel>("Perguntas");
            var pergunta = perguntas.Where(u => u.CodigoPergunta == perguntasCommand.CodigoPergunta && u.CodigoQuiz == perguntasCommand.CodigoQuiz).FirstOrDefault();

            if (pergunta == null)
                throw new Exception("Quiz não existe!");

            pergunta.Descricao = perguntasCommand.Descricao;
            pergunta.Titulo = perguntasCommand.Titulo;
            perguntas.Add(pergunta);

            await SalvarDados(perguntas, "Quiz");

            return NoContent();
        }

        [HttpPut("alterarAlternativa")]
        public async Task<ActionResult<AlternativasModel>> AlterarAlternativa([FromBody] List<AlternativasCommand> alternativasCommand)
        {
            if (!ModelState.IsValid)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Máximo de alternativas é 4!");

            if (alternativasCommand == null)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Falta Dados para fazer a inserção de dados!");

            foreach (var item in alternativasCommand)
            {
                if (string.IsNullOrEmpty(item.Titulo))
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Falta Dados para fazer a inserção de dados!");
            }

            if (alternativasCommand.Count > 4)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Máximo 4 alternativas!");

            var alternativas = CarregarData<AlternativasModel>("Alternativas");

            foreach (var item in alternativasCommand)
            {
                var alternativa = alternativas.Where(u => u.CodigoAlternativa == item.CodigoAlternativa && u.CodigoPergunta == item.CodigoPergunta && u.CodigoQuiz == item.CodigoQuiz).FirstOrDefault();

                alternativa.Correta = item.Correta;
                alternativa.Titulo = item.Titulo;
                alternativas.Add(alternativa);

                await SalvarDados(alternativas, "Quiz");
            }

            return NoContent();
        }

        [HttpDelete("excluirAlternativa")]
        public async Task<ActionResult> ExcluirAlternativa([FromBody] List<Guid> id)
        {
            foreach (var item in id)
            {
                if (item == Guid.Empty)
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Alternativas não existente!");

                var alternativas = CarregarData<AlternativasModel>("Alternativas");
                alternativas = alternativas
                    .Where(u => u.CodigoAlternativa != item)
                    .ToList();

                await SalvarDados(alternativas, "Alternativas");
            }

            return NoContent();
        }

        [HttpDelete("excluirPergunta")]
        public async Task<ActionResult> ExcluirPergunta([FromBody] Guid id)
        {
            if (id == Guid.Empty)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Quiz não existente!");

            var perguntas = CarregarData<PerguntasModel>("Perguntas");
            perguntas = perguntas
                .Where(u => u.CodigoPergunta != id)
                .ToList();

            await SalvarDados(perguntas, "Perguntas");

            return NoContent();
        }

        [HttpDelete("excluirQuiz")]
        public async Task<ActionResult> ExcluirQuiz([FromBody] Guid id)
        {
            if (id == Guid.Empty)
                return StatusCode((int)HttpStatusCode.InternalServerError, "Quiz não existente!");

            var quiz = CarregarData<QuizModel>("Quiz");
            quiz = quiz
                .Where(u => u.CodigoQuiz != id)
                .ToList();

            await SalvarDados(quiz, "Quiz");

            return NoContent();
        }
    }
}
