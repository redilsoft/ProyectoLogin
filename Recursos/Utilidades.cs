using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Query.Internal;


namespace ProyectoLogin.Recursos
{
    public class Utilidades
    {
        public static string EncriptarClave(string clave)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(clave);
                byte[] hash = sha256.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
