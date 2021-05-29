using FDevsQuiz.Domain.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FDevsQuiz.Application.Controllers.V2
{
    [Controller]
    [Route("v2/quiz")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizRepository _quizRepository;

        public QuizController(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }
    }
}
