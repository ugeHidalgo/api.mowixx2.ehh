using API.Mowizz2.EHH.Configs;
using API.Mowizz2.EHH.Models;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Mowizz2.EHH.Services
{
    public class UsersService
    {
        private readonly IMongoCollection<User> _users;

        public UsersService(IDataBaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var dataBase = client.GetDatabase(settings.DatabaseName);

            _users = dataBase.GetCollection<User>(settings.UsersCollectionName);
        }

        public HealthStatusData Check()
        {
            return new HealthStatusData
            {
                Connected = _users != null,
                CollectionName = _users.CollectionNamespace.CollectionName
            };
        }

        public async Task<User> Get(string userId)
        {
            var users = await _users.FindAsync(user => user.Id == userId);
            return users.FirstOrDefault();
        }

        public async Task<User> GetByUserName(string userName)
        {
            var users = await _users.FindAsync(user => user.UserName == userName);
            return users.FirstOrDefault();
        }

        public async Task<User> Post(User user)
        {
            await _users.InsertOneAsync(user);
            return user;
        }

        public async Task<UserToken> AddTokenToAuthorizedUser(UserToken userTokenRequest)
        {
            IAsyncCursor<User> users = await _users.FindAsync(user => user.UserName == userTokenRequest.UserName);
            User userInBd = users.FirstOrDefault();
            userTokenRequest.Token = string.Empty;
            if (IsAuthorizedUser(userTokenRequest, userInBd))
            {
                userTokenRequest.Token = GetToken(userTokenRequest, userInBd);                
            }
            
            return userTokenRequest;
        }

        #region Private methods

        private bool IsAuthorizedUser(UserToken userToken, User userInBd)
        {
            return userToken.Password == userInBd.Password;
        }

        private string GetToken(UserToken userToken, User userInBd)
        {
            return "Un token aquí";
        }

        #endregion
    }
}
