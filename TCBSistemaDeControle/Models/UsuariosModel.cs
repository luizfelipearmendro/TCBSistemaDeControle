using System.ComponentModel.DataAnnotations;
using TCBSistemaDeControle.Enum;

namespace TCBSistemaDeControle.Models
{
    public class UsuariosModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório!")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória!")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "O nome completo é obrigatório!")]
        public string? NomeCompleto { get; set; }

        [Required(ErrorMessage = "O tipo de perfil é obrigatório!")]
        public PerfilEnum? Perfil { get; set; }

        public string? Hash { get; set; }

        public int? Confirmado { get; set; }

        public string? Salt { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataHoraEsqueceuSenha { get; set; }
    }
}
