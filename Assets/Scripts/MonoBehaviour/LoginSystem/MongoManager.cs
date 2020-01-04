using MongoDB.Driver;
using System.Collections;
using System.Collections.Generic;
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


    public async void ConnectUser(MongoUser user)
    {
        var database = _client.GetDatabase("Game");
        var users = database.GetCollection<MongoUser>("Users");
        var cursor = await users.FindAsync<MongoUser>((x) => user.FindUserThatIsNotConnected(x));
        var result = await cursor.ToListAsync();
        if(result.Count > 0)
        {
            await users.UpdateOneAsync((x) => x._id == result[0]._id, Builders<MongoUser>.Update.Set(x=>x.IsConnected,true));
            _connectedUsersId = result[0]._id;
            Debug.Log("User connected!");
        }
        else
        {
            Debug.LogError("User not found");
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
        var cursor = await users.FindAsync<MongoUser>((x) => x.FindConnectedUser(_connectedUsersId));
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

