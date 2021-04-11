using Api.Data.Context;
using Api.Data.Repository;
using Api.Domain.Interfaces;
using Api.Domain.Interfaces.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Api.CrossCutting.DependencyInjection
{
    public static class ConfigureRepository
    {
        public static void ConfigureDependenciesRepository(this IServiceCollection serviceCollection)
        {
            //Transient -> Instancia toda vez que é chamado.
            //Scope -> Instancia apenas 1x durante a requisição.
            //Singletion -> Instancia apenas 1x durante o ciclo de vida da aplicação.

            serviceCollection.AddScoped<IDbContext, DbContext>();
            serviceCollection.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
