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

            var hash = HttpContext.Session.GetString("hash");
            if (string.IsNullOrEmpty(hash))
            {
                TempData["MensagemErro"] = "Sessão inválida. Faça login novamente.";
                return RedirectToAction("Index", "Login");
            }

            var dbconsult = db.Usuarios
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == idUsuario && u.Hash == hash);

            if (dbconsult == null) return RedirectToAction("Index", "Login");

            try
            {
                
                if (!ModelState.IsValid)
                {
                    TempData["MensagemErro"] = "Dados inválidos!";
                    return View(setor);
                }

                var novoSetor = new SetoresModel
                {
                    Nome = setor.Nome,
                    Descricao = setor.Descricao,
                    ResponsavelSetor = setor.ResponsavelSetor,
                    EmailResponsavelSetor = setor.EmailResponsavelSetor,
                    SexoResponsavel = setor.SexoResponsavel,
                    Localizacao = setor.Localizacao,
                    DataCriacao = DateTime.Now,
                    DataAtualizacao = DateTime.Now,
                    Ativo = 1, // Define o setor como ativo por padrão
                    UsuarioId = dbconsult.Id,
                    CategoriaId = setor.CategoriaId,
                    ImagemSetor = setor.ImagemSetor // Se a imagem for enviada como string
                };

                db.Setores.Add(novoSetor);
                db.SaveChanges();

                return RedirectToAction("Index");
=======
                if (!ModelState.IsValid)

                setor.UsuarioId = sessionIdUsuario;
                setor = setoresRepositorio.Cadastrar(setor);

                TempData["MensagemSucesso"] = "Setor cadastrado com sucesso!";
                return RedirectToAction("Index", "Setores");
>>>>>>> 6d1b123fbdb6e1e4a30a25683c851a5ae5625560
            }
            catch (System.Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não foi possível cadastrar o setor. Detalhes do erro: {erro.Message}";
<<<<<<< HEAD
                return View();
=======
                return RedirectToAction("Index", "Setores");
>>>>>>> 6d1b123fbdb6e1e4a30a25683c851a5ae5625560
            }
        }
    }
}