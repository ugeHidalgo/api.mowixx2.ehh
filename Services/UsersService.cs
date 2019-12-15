using API.Mowizz2.EHH.Configs;
using API.Mowizz2.EHH.Models;
using MongoDB.Driver;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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

        public async Task<UserToken> AddTokenToAuthorizedUser(UserToken userTokenRequest, JwtIssuerOptions jwtOptions)
        {
            IAsyncCursor<User> users = await _users.FindAsync(user => user.UserName == userTokenRequest.UserName);
            User userInBd = users.FirstOrDefault();

            userTokenRequest.Token = string.Empty;            
            userTokenRequest.ExpiresIn = 0;

            if (IsAuthorizedUser(userTokenRequest, userInBd))
            {
                userTokenRequest.Token = await GetToken(userTokenRequest.UserName, jwtOptions);
                userTokenRequest.ExpiresIn = (int)jwtOptions.ValidFor.TotalSeconds;                
            }

            userTokenRequest.Password = string.Empty;
            return userTokenRequest;
        }

        #region Private methods

        private bool IsAuthorizedUser(UserToken userToken, User userInBd)
        {
            return userInBd == null ? false : userToken.Password == userInBd.Password;
        }

        private async Task<string> GetToken(string userName, JwtIssuerOptions jwtOptions)
        {
            var claims = new[]
              {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, await jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat,
                          ToUnixEpochDate(jwtOptions.IssuedAt).ToString(),
                          ClaimValueTypes.Integer64,
                          "MowizzApiAccess")
              };

            jwtOptions.IssuedAt = DateTime.UtcNow;            

            var jwt = new JwtSecurityToken(
                              issuer: jwtOptions.Issuer,
                              audience: jwtOptions.Audience,
                              claims: claims,
                              notBefore: jwtOptions.NotBefore,
                              expires: jwtOptions.Expiration,
                              signingCredentials: jwtOptions.SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

        #endregion
    }
}
