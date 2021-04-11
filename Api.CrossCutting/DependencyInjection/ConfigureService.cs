using Api.Domain.Interfaces.Services.Login;
using Api.Domain.Interfaces.Services.User;
using Api.Service.Services.Login;
using Api.Service.Services.User;
using Microsoft.Extensions.DependencyInjection;

namespace Api.CrossCutting.DependencyInjection
{
    public static class ConfigureService
    {
        public static void ConfigureDependenciesService(this IServiceCollection serviceCollection)
        {
            //Transient -> Instancia toda vez que é chamado.
            //Scope -> Instancia apenas 1x durante a requisição.
            //Singletion -> Instancia apenas 1x durante o ciclo de vida da aplicação.

            serviceCollection.AddScoped<IUserService, UserService>();
            serviceCollection.AddScoped<ILoginService, LoginService>();
        }
    }
}

