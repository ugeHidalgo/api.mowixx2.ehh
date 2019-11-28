using MongoDB.Bson.Serialization.Attributes;

namespace API.Mowizz2.EHH.Models
{
    public class UserToken
    {
        [BsonElement("userName")]
        public string UserName { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("token")]
        public string Token { get; set; }
    }
}
