using TCBSistemaDeControle.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer; // Adicione esta linha para resolver o erro CS1061
//using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
}
);

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
//    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

builder.Services.AddSession();

var app = builder.Build();

app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "-1";
    await next();
});

// Configure the HTTP request pipeline.
app.UseSession();
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=LandingPage}/{action=Index}/{id?}");

app.Run();






////PROGRAM.CS PARA MIM CONSEGUIR VER AS A��ES DO BACKEND

//using Microsoft.EntityFrameworkCore;
//using TCBSistemaDeControle.Data;
//using TCBSistemaDeControle.Models;
//using TCBSistemaDeControle.Helper;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllersWithViews();

//// Configura��o do DbContext
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
//});

//builder.Services.AddSession();

//var app = builder.Build();

//// Garante que o banco de dados seja criado e populado com dados iniciais
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    try
//    {
//        var context = services.GetRequiredService<ApplicationDbContext>();
//        context.Database.Migrate(); 


//        if (!context.Usuarios.Any(u => u.Email == "admin@example.com"))
//        {
//            // Gera um salt aleat�rio
//            var salt = Utilitarios.GerarSalt();

//            // Cria um usu�rio administrador padr�o
//            var adminUser = new UsuariosModel
//            {
//                NomeCompleto = "Admin",
//                Email = "admin@example.com",
//                Senha = Utilitarios.GerarHashSenha("Senha123!", salt), // Usa o salt gerado
//                Salt = salt, // Armazena o salt no usu�rio
//                TipoPerfil = 1
//            };

//            // Adiciona o usu�rio ao banco de dados
//            context.Usuarios.Add(adminUser);
//            context.SaveChanges();
//        }
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Erro ao inicializar o banco de dados: {ex.Message}");
//    }
//}

//app.Use(async (context, next) =>
//{
//    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
//    context.Response.Headers["Pragma"] = "no-cache";
//    context.Response.Headers["Expires"] = "-1";
//    await next();
//});

//// Configura��o do pipeline HTTP
//app.UseSession();
//if (!app.Environment.IsDevelopment())
//{
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseRouting();
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Login}/{action=Index}/{id?}");

//app.Run();