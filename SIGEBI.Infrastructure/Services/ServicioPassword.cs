using SIGEBI.Application.Interfaces.ext;
using System.Security.Cryptography;

namespace SIGEBI.Infrastructure.Services
{
    public class ServicioPassword : IServicioPassword
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100000;

        private static readonly HashAlgorithmName Algorithm =
            HashAlgorithmName.SHA3_256;

        public string GenerarHash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException("La contraseña es obligatoria.");
            }

            var salt = RandomNumberGenerator.GetBytes(SaltSize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, KeySize);

            return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        public bool VerificarPassword(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(passwordHash))
            {
                return false;
            }

            try
            {
                var partes = passwordHash.Split('.');

                if (partes.Length != 3)
                {
                    return false;
                }

                var iterations = int.Parse(partes[0]);
                var salt = Convert.FromBase64String(partes[1]);
                var hashGuardado = Convert.FromBase64String(partes[2]);

                var hashIngresado = Rfc2898DeriveBytes.Pbkdf2(
                    password,
                    salt,
                    iterations,
                    Algorithm,
                    hashGuardado.Length
                );

                return CryptographicOperations.FixedTimeEquals(
                    hashIngresado,
                    hashGuardado
                );
            }
            catch
            {
                return false;
            }
        }
    }
}
