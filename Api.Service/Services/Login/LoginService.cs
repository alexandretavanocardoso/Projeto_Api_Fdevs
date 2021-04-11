using Api.Domain.Dtos;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Repository;
using Api.Domain.Interfaces.Services.Login;
using Api.Domain.Security;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Api.Service.Services.Login
{
    public class LoginService : ILoginService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly SigningConfigurations _signingConfigurations;
        private readonly TokenConfiguration _tokenConfiguration;

        public LoginService(IConfiguration configuration,
                            IUserRepository userRepository,
                            SigningConfigurations signingConfigurations,
                            TokenConfiguration tokenConfiguration)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _signingConfigurations = signingConfigurations;
            _tokenConfiguration = tokenConfiguration;
        }

        public async Task<object> FindByLoginAsync(LoginDto login)
        {
            if (login != null && !string.IsNullOrWhiteSpace(login.Email))
            {
                UserEntity userEntity = await _userRepository.FindByLoginAsync(login.Email);

                if (userEntity == null)
                {
                    return new
                    {
                        authenticared = false,
                        message = "Falha ao autenticar"
                    };
                }
                else
                {
                    var identity = new ClaimsIdentity(new GenericIdentity(userEntity.Email), new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.UniqueName, userEntity.Email),
                    });

                    DateTime createDate = DateTime.Now;
                    DateTime expirationDate = createDate + TimeSpan.FromSeconds(_tokenConfiguration.Seconds);

                    var handler = new JwtSecurityTokenHandler();
                    string token = CreateToken(identity, createDate, expirationDate, handler);

                    return SuccessObject(createDate, expirationDate, token, userEntity);
                }
            }
            else
                return new
                {
                    authenticared = false,
                    message = "Falha ao autenticar"
                };
        }

        private string CreateToken(ClaimsIdentity identity, DateTime createDate, DateTime expirationDate, JwtSecurityTokenHandler handler)
        {
            var securityToken = handler.CreateToken(new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Issuer = _tokenConfiguration.Issuer,
                Audience = _tokenConfiguration.Audience,
                SigningCredentials = _signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = createDate,
                Expires = expirationDate
            });

            var token = handler.WriteToken(securityToken);
            return token;
        }

        private object SuccessObject(DateTime createDate, DateTime expirantionDate, string token, UserEntity user)
        {
            return new
            {
                authenticated = true,
                created = createDate.ToString("yyyy-MM-dd HH:mm:ss"),
                expirantionDate = expirantionDate.ToString("yyyy-MM-dd HH:mm:ss"),
                acessToken = token,
                userName = user.Name,
                email = user.Email,
                message = "Usuário autenticado com sucesso"
            };
        }
    }
}

