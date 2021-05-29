using FDevsQuiz.Domain.Interface;
using FDevsQuiz.Domain.Model;
using FDevsQuiz.Domain.Repository;
using FDevsQuiz.Infra.Data.Repository.Base;

namespace FDevsQuiz.Infra.Data.Repository
{
    public class PerguntaRepository : CrudRepository<long, EnqPergunta>, IPerguntaRepository
    {
        public PerguntaRepository(IDbContext context) : base(context)
        {
        }
    }
}
