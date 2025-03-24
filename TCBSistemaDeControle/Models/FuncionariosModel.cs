using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TCBSistemaDeControle.Models
{
    public class FuncionariosModel
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public string Nome { get; set; }

        public DateTime DataNascimento { get; set; }

        public char Sexo { get; set; }

        public int Raca { get; set; }

        public int EstadoCivil { get; set; }

        public string? NomeMae { get; set; }

        public string Naturalidade { get; set; }

        public string Endereco { get; set; }

        public string CidadeResidencia { get; set; }

        [EmailAddress(ErrorMessage = "O e-mail informado não é válido!")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "O celular informado não é válido!")]
        public string Celular { get; set; }

        public string Setor { get; set; }

        public string Cargo { get; set; }

        public Decimal Salario { get; set; }

        public DateTime DataIngresso { get; set; }

        public int DiasTrabalhados { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime? DataAtualizacao { get; set; }
    }
}
