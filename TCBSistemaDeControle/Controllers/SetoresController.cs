using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TCBSistemaDeControle.Data;
using TCBSistemaDeControle.Models;
using TCBSistemaDeControle.Repositories;

namespace TCBSistemaDeControle.Controllers
{
    public class SetoresController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly ISetoresRepositorio setoresRepositorio;

        public SetoresController(ApplicationDbContext db, ISetoresRepositorio _setoresRepositorio)
        {
            this.db = db;
            setoresRepositorio = _setoresRepositorio;
        }

        public int sessionIdUsuario
        {
            get
            {
                int sessionIdUsuario = 0;
                if (HttpContext.Session.GetInt32("Id") != null)
                    sessionIdUsuario = (int)HttpContext.Session.GetInt32("Id");
                return sessionIdUsuario;
            }
        }
        public IActionResult Index(string searchString, int? categoriaId, int? ativo)
        {
            var idUsuario = HttpContext.Session.GetInt32("idUsuario");
            if (idUsuario == null) return RedirectToAction("Index", "Login");

            var dbconsult = db.Usuarios
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == idUsuario && u.Hash == HttpContext.Session.GetString("hash"));

            if (dbconsult == null) return RedirectToAction("Index", "Login");

            var sessionIdUsuario = dbconsult.Id;

            // Consulta inicial dos setores
            var setoresQuery = db.Setores
                .AsNoTracking()
                .Where(s => s.UsuarioId == sessionIdUsuario);

            // Aplica o filtro de busca por nome
            if (!string.IsNullOrEmpty(searchString))
            {
                setoresQuery = setoresQuery.Where(s =>
                    EF.Functions.Like(s.Nome, $"%{searchString}%") ||
                    EF.Functions.Like(s.Descricao, $"%{searchString}%") ||
                    EF.Functions.Like(s.ResponsavelSetor, $"%{searchString}%")
                );
            }

            // Aplica o filtro por categoria
            if (categoriaId.HasValue)
            {
                setoresQuery = setoresQuery.Where(s => s.CategoriaId == categoriaId);
            }

            // Aplica o filtro por status (ativo/inativo)
            if (ativo.HasValue)
            {
                setoresQuery = setoresQuery.Where(s => s.Ativo == ativo.Value);
            }

            // Executa a consulta e ordena os resultados
            var setoresFiltrados = setoresQuery
                .OrderBy(s => s.Nome)
                .ToList();

            // Obtém todas as categorias do banco de dados (independentemente dos filtros)
            var todasCategorias = db.Categorias
                .AsNoTracking()
                .ToList();

            var viewModel = new SetoresViewModel
            {
                Setores = setoresFiltrados,
                Categorias = todasCategorias 
            };

            ViewBag.NomeCompleto = dbconsult.NomeCompleto;
            ViewBag.Email = dbconsult.Email;
            ViewBag.TipoPerfil = dbconsult.TipoPerfil;
            ViewBag.SearchString = searchString;
            ViewBag.CategoriaId = categoriaId;
            ViewBag.Ativo = ativo;

            // Prepara as opções para os dropdowns
            ViewBag.CategoriasOpcoes = new SelectList(todasCategorias, "Id", "Nome", categoriaId); // Todas as categorias
            ViewBag.StatusOpcoes = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Todos" },
                new SelectListItem { Value = "1", Text = "Ativos" },
                new SelectListItem { Value = "0", Text = "Inativos" }
            }, "Value", "Text", ativo);

            return View(viewModel);
        }

            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(SetoresModel setor)
        {
            var idUsuario = HttpContext.Session.GetInt32("idUsuario");
            if (idUsuario == null) return RedirectToAction("Index", "Login");

            var dbconsult = db.Usuarios.Find(idUsuario);
            if (dbconsult == null || dbconsult.Hash != HttpContext.Session.GetString("hash"))
                return RedirectToAction("Index", "Login");

            var sessionIdUsuario = dbconsult.Id;

            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["MensagemErro"] = "Dados inválidos!";
                    return RedirectToAction("Index", "Setores");
                }

                setor.UsuarioId = sessionIdUsuario;
                setor = setoresRepositorio.Cadastrar(setor);

                TempData["MensagemSucesso"] = "Setor cadastrado com sucesso!";
                return RedirectToAction("Index", "Setores");
            }
            catch (System.Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não foi possível cadastrar o setor. Detalhes do erro: {erro.Message}";
                return RedirectToAction("Index", "Setores");
            }
        }
    }
}