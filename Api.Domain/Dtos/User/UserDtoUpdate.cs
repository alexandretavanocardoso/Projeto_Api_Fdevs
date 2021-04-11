using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Dtos.User
{
    public class UserDtoUpdate
    {
        [Required(ErrorMessage = "Id é campo obrigatório.")]
        public Guid Id { get; set; }
        
        [Required(ErrorMessage = "Name é campo obrigatório.")]
        [StringLength(200, ErrorMessage = "Name deve ter no máximo {1} caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email é campo obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail em formato inválido.")]
        [StringLength(200, ErrorMessage = "E-mail deve ter no máximo {1} caracteres.")]
        public string Email { get; set; }
    }
}
