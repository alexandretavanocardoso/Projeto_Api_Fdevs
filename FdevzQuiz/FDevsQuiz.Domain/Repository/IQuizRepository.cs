using FDevsQuiz.Domain.Interface;
using FDevsQuiz.Domain.Model;

namespace FDevsQuiz.Domain.Repository
{
    public interface IQuizRepository : ICrudRepository<long, EnqQuiz>
    {
    }
}
