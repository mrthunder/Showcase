using MongoDB.Driver;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using Sirenix.OdinInspector;
using MongoDB.Bson;

public class MongoManager : MonoBehaviour
{
    private static MongoManager _instance = null;
    public static MongoManager Instance => _instance;

    [AssetSelector()]
    public MongoSettings Settings;

    private MongoClient _client;

    private ObjectId _connectedUsersId; 

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (_instance != null)
        {
            DestroyImmediate(_instance);
        }
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _client = new MongoClient($"mongodb+srv://{Settings.Username}:{Settings.Password}@{Settings.ClusterName}-birif.mongodb.net/test?retryWrites=true&w=majority");
    }


    public async void ConnectUser((string username, string password) user, System.Action<bool> callback)
    {
        Debug.Log("Async method");
        MongoUser mongoUser = new MongoUser()
        {
            username = user.username,
            password = GetHashString(user.password),
        };
        
        var database = _client.GetDatabase("Game");
        Debug.Log("Get Database");
        var users = database.GetCollection<MongoUser>("Users");
        Debug.Log("Get Collection");
        var cursor = await users.FindAsync<MongoUser>(mongoUser.FindUserThatIsNotConnected());
        Debug.Log("Find Async");
        var result = await cursor.ToListAsync();
        Debug.Log("Turn to list");
        if (result.Count > 0)
        {
            Debug.Log("Update");
            await users.UpdateOneAsync((x) => x._id == result[0]._id, Builders<MongoUser>.Update.Set(x=>x.IsConnected,true));
            _connectedUsersId = result[0]._id;
            Debug.Log("User connected!");
            callback(true);
        }
        else
        {
            Debug.LogError("User not found");
            callback(false);
        }
    }

    public async void RegisterUser((string username, string password) newUser ,System.Action<bool> callback)
    {
        var database = _client.GetDatabase("Game");
        var users = database.GetCollection<MongoUser>("Users");
        var cursor = await users.FindAsync(x => x.username == newUser.username);
        var result = await cursor.ToListAsync();
        if(result.Count > 0)
        {
            callback(false);
            Debug.Log("User already exist");
        }
        else
        {
            MongoUser user = new MongoUser()
            {
                username = newUser.username,
                password = GetHashString(newUser.password),
                IsConnected = false
            };
            await users.InsertOneAsync(user);

            var checkCursor = await users.FindAsync<MongoUser>(user.FindUserThatIsNotConnected());
            var checkResult = await checkCursor.ToListAsync();
            if(checkResult.Count > 0)
            {
                callback(true);
            }
            else
            {
                callback(false);
            }
            
        }
    }

    private void OnDestroy()
    {
        DisconnectUser();
    }

    private async void DisconnectUser()
    {
        if (_connectedUsersId == ObjectId.Empty) return;
        //TODO - disconnect mongo user
        var database = _client.GetDatabase("Game");
        var users = database.GetCollection<MongoUser>("Users");
        var cursor = await users.FindAsync<MongoUser>(FindConnectedUser(_connectedUsersId));
        var result = await cursor.ToListAsync();
        if (result.Count > 0)
        {
            await users.UpdateOneAsync((x) => x._id == result[0]._id, Builders<MongoUser>.Update.Set(x => x.IsConnected, false));
            _connectedUsersId = ObjectId.Empty;
            Debug.Log("User Disconnected");
        }
        else
        {
            Debug.LogError("User not found");
        }
    }

    public FilterDefinition<MongoUser> FindConnectedUser(ObjectId userId)
    {
        var builder = Builders<MongoUser>.Filter;
        return builder.Eq(x => x._id, userId) & builder.Eq(x => x.IsConnected, true);
    }


    #region Stackoverflow - Hash string in c#
    //https://stackoverflow.com/questions/3984138/hash-string-in-c-sharp
    public static byte[] GetHash(string inputString)
    {
        HashAlgorithm algorithm = SHA256.Create();
        return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
    }

    public static string GetHashString(string inputString)
    {
        StringBuilder sb = new StringBuilder();
        foreach (byte b in GetHash(inputString))
            sb.Append(b.ToString("X2"));

        return sb.ToString();
    }
    #endregion

}

