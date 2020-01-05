using MongoDB.Bson;
using MongoDB.Driver;

public class MongoUser
{
    /// <summary>
    /// Users Mongo id
    /// </summary>
    public ObjectId _id { get; set; }
    /// <summary>
    /// User's username
    /// </summary>
    public string username { get; set; }
    /// <summary>
    /// Users secret
    /// </summary>
    public string password { get; set; }
    /// <summary>
    /// Is the user already connected in any device?
    /// </summary>
    public bool IsConnected { get; set; }


    public FilterDefinition<MongoUser> FindUserThatIsNotConnected()
    {
        var builder = Builders<MongoUser>.Filter;
        return builder.Eq(x=>x.username, this.username) & builder.Eq(x=>x.password, this.password) & builder.Eq(x=>x.IsConnected,false);
    }

    
}

