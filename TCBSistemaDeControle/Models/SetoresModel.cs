using System.ComponentModel.DataAnnotations;

namespace TCBSistemaDeControle.Models
{
    public class SetoresModel
    {
        
        public int Id { get; set; } // Identificador único  
        
        public string Nome { get; set; } = string.Empty; // Nome do setor  
        
        public string Descricao { get; set; } = string.Empty; // Descrição do setor  
        
        public int NumeroFuncionarios { get; set; } // Quantidade de funcionários no setor  
        
        public string ResponsavelSetor { get; set; } = string.Empty; // Nome do responsável pelo setor  

        [EmailAddress(ErrorMessage = "O e-mail informado não é válido!")]
        public string EmailResposavelSetor { get; set; } = string.Empty; // E-mail do responsável pelo setor

        public string Localizacao { get; set; } = string.Empty; // Localização dentro da empresa  
        
        public DateTime DataCriacao { get; set; } = DateTime.Now; // Data de criação do setor  
        
        public bool Ativo { get; set; } = true; // Indica se o setor está ativo 
    }
}