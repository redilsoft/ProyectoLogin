using Microsoft.EntityFrameworkCore;
using ProyectoLogin.Models;
using ProyectoLogin.Servicios.Contrato;
using ProyectoLogin.Servicios.Implementacion;

// Añadimos las referencias para la cookie de autenticación
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DbpruebaLoginContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("cadenasql") ?? throw new InvalidOperationException("Connection string 'DbpruebaLogin' not found.")));

// Inyeccion de dependencias de servicios para utilizar en los controladores MVC
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// Configuración de la autenticación por cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Inicio/IniciarSesion"; // Página de inicio de sesión
        options.LogoutPath = "/Inicio/CerrarSesion"; // Página de cierre de sesión
        options.AccessDeniedPath = "/Inicio/AccesoDenegado"; // Página de acceso denegado
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20); // Tiempo de expiración de la cookie
    });

// Deshabilitar el almacenamiento en caché de las respuestas para mejorar la seguridad y evitar que el usuario vualva a la página anterior sin loguearse
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(
        new ResponseCacheAttribute
        {
            NoStore = true,
            Location = ResponseCacheLocation.None,
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Habilitar la autenticación

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inicio}/{action=IniciarSesion}/{id?}");

app.Run();
