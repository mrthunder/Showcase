using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MongoSettings : ScriptableObject
{
    [BoxGroup("DB User")]
    public string Username;
    [BoxGroup("DB User")]
    public string Password;
    [BoxGroup("Database Information")]
    public string ClusterName;
}
