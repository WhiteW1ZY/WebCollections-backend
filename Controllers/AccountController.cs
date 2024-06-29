using backend.Models.Entityes.UserEntityes;
using backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private AuthentificationService _authorizationService;

        public AccountController(AuthentificationService authorizationService) 
        { 
            _authorizationService = authorizationService;
        }

        [HttpPost("/generateToken")]
        public async Task<IResult> GenerateToken([FromBody] UserAuthorization userData)
        {
            return await _authorizationService.GenerateToken(userData);
        }

    }
}
