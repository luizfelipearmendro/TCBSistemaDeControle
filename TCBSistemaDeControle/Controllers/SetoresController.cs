using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Org.BouncyCastle.Crypto.Digests;
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
        public IActionResult Index(string searchString)
        {
            var idUsuario = HttpContext.Session.GetInt32("idUsuario");
            if (idUsuario == null) return RedirectToAction("Index", "Login");

            var dbconsult = db.Usuarios.Find(idUsuario);
            if (dbconsult == null || dbconsult.Hash != HttpContext.Session.GetString("hash"))
                return RedirectToAction("Index", "Login");

            var sessionIdUsuario = dbconsult.Id;

            IQueryable<SetoresModel> setoresQuery = db.Setores
                .Where(s => s.UsuarioId == sessionIdUsuario);

            if (!string.IsNullOrEmpty(searchString))
            {
                setoresQuery = setoresQuery.Where(s =>
                    s.Nome.Contains(searchString) ||
                    s.Descricao.Contains(searchString) ||
                    s.ResponsavelSetor.Contains(searchString)
                );
            }

            var setores = setoresQuery.OrderBy(s => s.Nome).ToList();
            var categorias = db.Categorias.ToList(); // Buscando categorias do banco

            var viewModel = new SetoresViewModel
            {
                Setores = setores,
                Categorias = categorias
            };

            ViewBag.NomeCompleto = dbconsult.NomeCompleto;
            ViewBag.Email = dbconsult.Email;
            ViewBag.TipoPerfil = dbconsult.TipoPerfil;
            ViewBag.SearchString = searchString;

            return View(viewModel);
        }


        public IActionResult Cadastrar()
        {
            var idUsuario = HttpContext.Session.GetInt32("idUsuario");
            if (idUsuario == null) return RedirectToAction("Index", "Login");

            var dbconsult = db.Usuarios.Find(idUsuario);
            if (dbconsult == null || dbconsult.Hash != HttpContext.Session.GetString("hash"))
                return RedirectToAction("Index", "Login");

            var sessionIdUsuario = dbconsult.Id;

            ViewBag.NomeCompleto = dbconsult.NomeCompleto;
            ViewBag.Email = dbconsult.Email;
            ViewBag.TipoPerfil = dbconsult.TipoPerfil;

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