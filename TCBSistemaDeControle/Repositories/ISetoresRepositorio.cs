using TCBSistemaDeControle.Models;

namespace TCBSistemaDeControle.Repositories
{
    public interface ISetoresRepositorio
    {
        List<SetoresModel> BuscarTodosSetores(int usuarioId);

        SetoresModel ListarPorId(int Id);

        SetoresModel Cadastrar(SetoresModel setor);

        SetoresModel Atualizar(SetoresModel setor);

        //List<FuncionariosPorSetorViewModel> ObterFuncionariosPorSetor();

        bool Desativar(int id);

        bool Reativar(int id);
    }
}