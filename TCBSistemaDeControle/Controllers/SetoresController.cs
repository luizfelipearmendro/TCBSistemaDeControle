using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Org.BouncyCastle.Crypto.Digests;
using TCBSistemaDeControle.Data;
using TCBSistemaDeControle.Models;

namespace TCBSistemaDeControle.Controllers
{
    public class SetoresController : Controller
    {
        private readonly ApplicationDbContext db;

        public SetoresController(ApplicationDbContext db)
        {
            this.db = db;
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
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(int? id, string nome, string descricao, int numeroFuncionarios, string responsavelSetor, string emailResponsavelSetor, string localizacao, DateTime dataCriacao, char ativo)
        {
            var idUsuario = HttpContext.Session.GetInt32("idUsuario");
            if (idUsuario == null) return RedirectToAction("Index", "Login");

            var dbconsult = db.Usuarios.Find(idUsuario);
            if (dbconsult == null || dbconsult.Hash != HttpContext.Session.GetString("hash"))
                return RedirectToAction("Index", "Login");

            var sessionIdUsuario = dbconsult.Id;

            try
            {
                if (ModelState.IsValid)
                {
                    TempData["MensagemErro"] = "Dados inválidos!";
                }
                    
                SetoresModel setor;
                setor = new SetoresModel
                {
                    Nome = nome,
                    Descricao = descricao,
                    ResponsavelSetor = responsavelSetor,
                    EmailResposavelSetor = emailResponsavelSetor,
                    Localizacao = localizacao,
                    Ativo = ativo,
                    DataCriacao = dataCriacao,
                    UsuarioId = sessionIdUsuario
                };

                db.Setores.Add(setor);

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
