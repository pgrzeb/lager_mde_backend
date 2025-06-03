using lager_mde_backend.Data;
using lager_mde_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace lager_mde_backend.Services
{
    public class PersonalService : IPersonalService
    {
        private readonly ApplicationDbContext _context;

        public PersonalService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var personal = await _context.Personal
                .FirstOrDefaultAsync(p => p.komnr == request.komnr);

            if (personal == null)
            {
                return new LoginResponse
                {
                    nachricht = "Ungültige komnr."
                };
            }

            using (var sha256 = SHA256.Create())
            {
                var hashedPinBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(request.pin));
                var hashedPinHex = BitConverter.ToString(hashedPinBytes).Replace("-", "");
                if (personal.pin != hashedPinHex)
                {
                    return new LoginResponse
                    {
                        komnr = 0,
                        komna = "",
                        pin = "",
                        rechte = 0,
                        nachricht = "Ungültiger pin."
                    };
                }
            }

            return new LoginResponse
            {
                komnr = personal.komnr,
                komna = personal.komna,
                pin = personal.pin,
                rechte = personal.rechte
            };
        }
    }
}