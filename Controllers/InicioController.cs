using Microsoft.AspNetCore.Mvc;
using ProyectoLogin.Models;
using ProyectoLogin.Recursos;
using ProyectoLogin.Servicios.Contrato;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.HttpSys;


namespace ProyectoLogin.Controllers
{
    public class InicioController : Controller
    {
        private readonly IUsuarioService _usuarioServicio;
        public InicioController(IUsuarioService usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        // GET: InicioController
        public IActionResult Registrarse()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registrarse(Usuario modelo)
        {
            modelo.Clave = Utilidades.EncriptarClave(modelo.Clave!);
            Usuario usuario_creado = await _usuarioServicio.SaveUsuario(modelo);

            if(usuario_creado.IdUsuario > 0)
            {
                return RedirectToAction("IniciarSesion", "Inicio");
            }

            ViewData["Mensaje"] = "No se pudo crear el usuario";

            return View();
        }
        // GET: InicioController
        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string correo, string clave)
        {
            // Recibir los datos del formulario
            Usuario usuario_encontrado = await _usuarioServicio.GetUsuario(correo, Utilidades.EncriptarClave(clave));
            
            if(usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "Credenciales incorrectas";
                return View();
            }
            // Crear las claims
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario_encontrado.NombreUsuario!),
                new Claim("IdUsuario", usuario_encontrado.IdUsuario.ToString())
            };
            ClaimsIdentity clainsidentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true
            };
            // Sign in the user
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(clainsidentity), properties);
            // Redirigir a la pagina principal
            return RedirectToAction("Index", "Home");

        }
    }
}
