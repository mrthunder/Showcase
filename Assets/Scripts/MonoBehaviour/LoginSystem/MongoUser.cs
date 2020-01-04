using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

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


    public bool FindUserThatIsNotConnected(MongoUser user) => ((this.username == user.username) && (this.password == user.password) && (!user.IsConnected));

    public bool FindConnectedUser(ObjectId userId) => (this._id == userId && this.IsConnected);
}

