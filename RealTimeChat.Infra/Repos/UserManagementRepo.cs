using Microsoft.EntityFrameworkCore;
using RealTimeChat.Core.Dtos;
using RealTimeChat.Core.Entities;
using RealTimeChat.Core.Repos;

namespace RealTimeChat.Infra.Repos
{
    public class UserManagementRepo : IUsermanagementRepo
    {
        private readonly AppDbContext _dbContext;

        public UserManagementRepo(AppDbContext dbContext)
        {

            _dbContext = dbContext;
        }
        public async Task<bool> DoesUserExist(LoginRequest userInfo)
        {
            if (await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == userInfo.Username && x.Password == userInfo.Password) == null)
                return false;

            return true;
        }

        public async Task<User> GetUser(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<List<Message>> GetUsersUnRecivedMessages(string username)
        {
            var unRecivedMessages= await _dbContext.Messages
                .Include(x => x.UserFrom)
                .Include(x => x.UserTo)
                .Where(x => x.UserTo.Username == username)
                .OrderBy(x => x.CreatedOn)
                .ToListAsync();

            _dbContext.Messages.RemoveRange(unRecivedMessages); // this should be in a separate method to not violote SRP
            await _dbContext.SaveChangesAsync();

            return unRecivedMessages;
        }

        public async Task<List<string>> GetUsersNames()
        {
            return await _dbContext.Users.Select(x => x.Username).ToListAsync();
        }

        public async Task<bool> Register(User user)
        {
            if (await _dbContext.Users.AnyAsync(x => x.Username == user.Username))
                return false;

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task SaveUserMessage(Message message)
        {

            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();
        }


    }
}
