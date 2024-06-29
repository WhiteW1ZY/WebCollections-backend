using backend.DB;
using backend.Models.Entityes.UserEntityes;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace backend.Service
{
    public class AuthentificationService
    {
        private readonly ApplicationContext _context;

        public AuthentificationService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IResult> GenerateToken(UserAuthorization userData)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == userData.login);
            if (user is null)
            {
                return Results.BadRequest(new { errorText = "Invalid username or password" });
            }
            else if (user.isBanned)
            {
                return Results.BadRequest(new { errorText = "User has been banned" });
            }
            else if(user.Password != userData.password)
            {
                return Results.BadRequest(new { errorText = "Uncurrent authorization data" });
            }

            var token = await GetIdentity(userData.login, userData.password);

            var response = new
            {
                access_token = token,
                username = user.Login
            };

            return Results.Json(response);
        }

        private async Task<string> GetIdentity(string login, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == password);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Role)
            };
           
            var cred = new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(10),
                    signingCredentials: cred
                );
            
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;

        }
    }
}
