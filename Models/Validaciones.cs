using System.Net.Mail;

namespace ComputacionFCQ_MVC.Models
{
    public class Validaciones
    {
        public static bool Entero(string str)
        {
            if (str == "" || str == null) return false;
            foreach (char c in str)
                if (!char.IsDigit(c))
                    return false;
            if (str.Length > 100) return false;
            return true;
        }

        public static bool Nombre(string str)
        {
            if (str == "" || str == null) return false;
            if (str.Contains("  ")) return false;
            foreach (char c in str)
                if (!(char.IsLetter(c) || c == ' '))
                    return false;
            if (str.Count(x => x == ' ') == str.Length) return false;
            if (str.Length > 100) return false;
            return true;
        }

        public static bool Correo(string str)
        {
            try
            {
                MailAddress mail = new MailAddress(str);
                if (str.Length > 100) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
