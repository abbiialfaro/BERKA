using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace BERKA.Web.Controllers
{
    public class AccountController : Controller
    {
        // Usuarios simulados para login de prueba
        private static List<dynamic> Usuarios = new List<dynamic>
        {
            new { Correo = "admin@berka.com", Contrasena = "admin123", Rol = "Administrador" },
            new { Correo = "inspector@berka.com", Contrasena = "inspector123", Rol = "Inspector" },
            new { Correo = "cliente@berka.com", Contrasena = "cliente123", Rol = "Cliente" }
        };

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Revision()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SacarCita()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string correo, string contrasena)
        {
            var usuario = Usuarios.FirstOrDefault(u => u.Correo == correo && u.Contrasena == contrasena);
            if (usuario != null)
            {
                HttpContext.Session.SetString("Rol", (string)usuario.Rol);
                HttpContext.Session.SetString("Correo", (string)usuario.Correo);


                // Redirigir según rol
                if (usuario.Rol == "Administrador")
                    return RedirectToAction("Dashboard", "Administrador");

                if (usuario.Rol == "Inspector")
                    return RedirectToAction("Revision", "Account");

                if (usuario.Rol == "Cliente")
                    return RedirectToAction("SacarCita", "Account");
            }

            ViewBag.Error = "Credenciales incorrectas.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}