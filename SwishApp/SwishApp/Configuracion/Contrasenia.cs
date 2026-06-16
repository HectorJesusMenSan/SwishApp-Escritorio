using System;
using System.Security.Cryptography;
using System.Text;

namespace SwishApp.Configuracion
{
    public class Contrasenia
    {
        private const int Iteraciones = 65536;
        private const int TamañoHash = 32; // 256 bits
        private const int TamañoSalt = 16;

        // =====================================================
        // HASH PASSWORD
        // =====================================================
        public static string HashPassword(string password)
        {
            // Generar salt aleatorio
            byte[] salt = new byte[TamañoSalt];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Generar hash con PBKDF2
            byte[] hash = GenerarHash(password, salt);

            // Retornar salt:hash en Base64
            return Convert.ToBase64String(salt) +
                   ":" +
                   Convert.ToBase64String(hash);
        }

        // =====================================================
        // VERIFICAR PASSWORD
        // =====================================================
        public static bool VerificarPassword(
            string password, string hashAlmacenado)
        {
            try
            {
                // Separar salt y hash
                string[] partes = hashAlmacenado.Split(':');

                if (partes.Length != 2)
                    return false;

                byte[] salt = Convert.FromBase64String(partes[0]);
                byte[] hashOriginal = Convert.FromBase64String(partes[1]);

                // Generar hash con la misma salt
                byte[] hashNuevo = GenerarHash(password, salt);

                // Comparar de forma segura
                return CompararBytes(hashOriginal, hashNuevo);
            }
            catch
            {
                return false;
            }
        }

        // =====================================================
        // GENERAR HASH PBKDF2
        // =====================================================
        private static byte[] GenerarHash(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                Iteraciones,
                HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(TamañoHash);
            }
        }

        // =====================================================
        // COMPARAR BYTES DE FORMA SEGURA
        // (evita timing attacks)
        // =====================================================
        private static bool CompararBytes(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;

            int diferencia = 0;

            for (int i = 0; i < a.Length; i++)
            {
                diferencia |= a[i] ^ b[i];
            }

            return diferencia == 0;
        }
    }
}