using FDevsQuiz.Domain.Interface;
using FDevsQuiz.Domain.Model;
using FDevsQuiz.Domain.Repository;
using FDevsQuiz.Infra.Data.Repository.Base;

namespace FDevsQuiz.Infra.Data.Repository
{
    public class AlternativaRepository : CrudRepository<long, EnqAlternativa>, IAlternativaRepository
    {
        public AlternativaRepository(IDbContext context) : base(context)
        {
        }
    }
}
