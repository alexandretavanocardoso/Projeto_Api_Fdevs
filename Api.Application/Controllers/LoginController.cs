using Api.Domain.Dtos;
using Api.Domain.Interfaces.Services.Login;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Api.Application.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<ActionResult<object>> Login([FromBody] LoginDto login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (login == null)
                return BadRequest();

            try
            {
                return Ok(await _loginService.FindByLoginAsync(login));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
