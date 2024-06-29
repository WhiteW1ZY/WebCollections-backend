using backend.Models;
using Microsoft.EntityFrameworkCore;
using backend.DB;
using backend.Models.Entityes.UserEntityes;

namespace backend.Service
{
    public class UserService
    {
        private readonly ApplicationContext _context;

        public UserService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IResult> GetUsers()
        {
            var users = await _context.Users
                .Select(user => new UserData
                {
                    id = user.id,
                    login = user.Login,
                    role = user.Role,
                    isBanned = user.isBanned,
                    CollectionsId = user.Collections.Select(c => c.Id).ToList(),
                    CommentsId = user.Comments.Select(c => c.Id).ToList(),
                    ReactionsId = user.reactions.Select(r => r.Id).ToList()
                }).ToListAsync();

            return Results.Ok(users);
        }

        public async Task<IResult> GetUserById(int id)
        {
            var user = await _context.Users
                .Where(u => u.id == id)
                .Select(user => new 
                {
                    id = user.id,
                    login = user.Login,
                    password = user.Password,
                    role = user.Role,
                    isBanned = user.isBanned,
                    CollectionsId = user.Collections.Select(c => c.Id).ToList(),
                    CommentsId = user.Comments.Select(c => c.Id).ToList(),
                    ReactionsId = user.reactions.Select(r => r.Id).ToList()
                }).FirstOrDefaultAsync();

            if(user is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(user);
        }

        public async Task<IResult> GetUser(string login)
        {
            var u = await _context.Users
                .Where(u => u.Login == login)
                .Select(user => new UserData
                {
                    id = user.id,
                    login = user.Login,
                    role = user.Role,
                    isBanned = user.isBanned,
                    CollectionsId = user.Collections.Select(c => c.Id).ToList(),
                    CommentsId = user.Comments.Select(c => c.Id).ToList(),
                    ReactionsId = user.reactions.Select(r => r.Id).ToList()
                }).FirstOrDefaultAsync();

            return u is null
                ? Results.NotFound()
                : Results.Ok(u);
        }

        public async Task<IResult> UpdateUser(string login, UserEntity userEntity)
        {
            if (userEntity is null
                || string.IsNullOrEmpty(userEntity.login))
            {
                return Results.BadRequest(new { errorText = "login must not be empty" });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
            if (user is null)
            {
                return Results.BadRequest(new { errorText = "User with this id is not exist" });
            }

            user.Login = userEntity.login;
            user.Password = userEntity.password;
            user.Role = userEntity.role;
            user.isBanned = userEntity.isBanned;

            await _context.SaveChangesAsync();

            return Results.Ok();
        }

        public async Task<IResult> DeleteUser(string login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return Results.Ok();
        }

        public async Task<IResult> CreateUser(UserEntity userData)
        {
            if (userData is null
                || string.IsNullOrEmpty(userData.password)
                || string.IsNullOrEmpty(userData.login))
            {
                return Results.BadRequest(new { errorText = "login and password must not be empty" });
            }
            else if (await _context.Users.FirstOrDefaultAsync(u => u.Login == userData.login) != null)
            {
                return Results.BadRequest(new { errorText = "User with this login is already exist" });
            };

            var user = new User() { Login = userData.login, Password = userData.password, Role = userData.role };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Results.Created();
        }
    }
}
