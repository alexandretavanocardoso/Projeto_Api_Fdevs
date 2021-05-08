using FdevzQuiz.Command;
using FdevzQuiz.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FdevzQuiz.Controllers
{
    [Route("usuarios")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private ICollection<T> CarregarData<T>()
        {
            // Ler arquivo -- AppDomain - pegamos nosso projeto
            using var openStream = System.IO.File.OpenRead(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Usuario.json"));
            // Deserializando json e pegando o result
            return JsonSerializer.DeserializeAsync<ICollection<T>>(openStream, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }).Result;
        }

        private async Task SalvarDados(ICollection<UserModel> dados)
        {
            using var createStream = System.IO.File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Usuario.json"));
            await JsonSerializer.SerializeAsync(createStream, dados, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        [HttpGet("recuperarListaUsuario")]
        public ActionResult<ICollection<UserModel>> RecuperarListaUsuario()
        {
            var usuarios = CarregarData<dynamic>();
            return Ok(usuarios);
        }

        [HttpGet("recuperarUsuario")]
        public ActionResult<UserModel> RecuperarUsuario([FromRoute] long id)
        {
            var usuarios = CarregarData<UserModel>();
            var usuario = usuarios.Where(u => u.CodigoUsuario == id).FirstOrDefault();
            return Ok(usuario);
        }

        [HttpPost("adicionarUsuario")]
        public async Task<ActionResult<UserModel>> AdicionarUsuario([FromBody] UsuarioCommand userCommand)
        {
            if (!ModelState.IsValid || userCommand == null)
                throw new Exception("Está faltando dados para a inclusão!");

            if (string.IsNullOrEmpty(userCommand.NomeUsuario))
                throw new Exception("Nome do usuário não obrigatório!");

            var usuarios = CarregarData<UserModel>();
            var usuario = new UserModel()
            {
                NomeUsuario = userCommand.NomeUsuario,
                Email = userCommand.Email,
                Idade = userCommand.Idade,
                UrlImagem = userCommand.UrlImagem,
                Pontuacao = 0
            };

            usuario.CodigoUsuario = usuarios.Select(u => u.CodigoUsuario).ToList().Max() + 1;
            usuarios.Add(usuario);

            await SalvarDados(usuarios);

            return Created("usuarios/{id}", usuarios);
        }

        [HttpPut("alterarUsuario")]
        public async Task<IActionResult> AlterarUsuario([FromRoute] long id, [FromBody] UsuarioCommand userCommand)
        {
            if (!ModelState.IsValid || userCommand == null)
                throw new Exception("Está faltando dados para a alteração!");

            if (string.IsNullOrEmpty(userCommand.NomeUsuario))
                throw new Exception("Nome do usuário não obrigatório!");

            var usuarios = CarregarData<UserModel>();
            var usuario = usuarios.Where(u => u.CodigoUsuario == id).FirstOrDefault();

            if (usuario == null)
                throw new Exception("USuario não existe!");

            usuario.NomeUsuario = userCommand.NomeUsuario;
            usuario.Pontuacao = userCommand.Pontuacao;
            usuario.UrlImagem = userCommand.UrlImagem;
            usuario.Email = userCommand.Email;
            usuarios.Add(usuario);

            await SalvarDados(usuarios);

            return NoContent();
        }

        [HttpDelete("excluirUsuario")]
        public async Task<ActionResult> ExcluirUsuario([FromRoute] long id)
        {
            var usuarios = CarregarData<UserModel>();
            usuarios = usuarios.Where(u => u.CodigoUsuario != id).ToList();

            await SalvarDados(usuarios);

            return NoContent();
        }
    }
}
