﻿using System.ComponentModel.DataAnnotations;

namespace TCBSistemaDeControle.Models
{
    public class SetoresModel
    {
        
        public int Id { get; set; } // Identificador único  
        
        public string Nome { get; set; } = string.Empty; // Nome do setor  
        
        public string Descricao { get; set; } = string.Empty; // Descrição do setor  
        
        public string ResponsavelSetor { get; set; } = string.Empty; // Nome do responsável pelo setor  

        [EmailAddress(ErrorMessage = "O e-mail informado não é válido!")]
        public string EmailResposavelSetor { get; set; } = string.Empty; // E-mail do responsável pelo setor

        public string Localizacao { get; set; } = string.Empty; // Localização dentro da empresa  
        
        public DateTime DataCriacao { get; set; } = DateTime.Now; // Data de criação do setor  

        public DateTime DataAtualizacao { get; set; } // Data de atualização do setor

        public char Ativo { get; set; } = 'S'; // Indica se o setor está ativo ou não

        public int UsuarioId { get; set; } // Identificador do usuário que criou o setor
    }
}