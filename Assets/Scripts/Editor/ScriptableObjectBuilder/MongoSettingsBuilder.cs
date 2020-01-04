using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using System.IO;

public class MongoSettingsBuilder : OdinMenuEditorWindow
{
    [MenuItem("Tools/Scriptable Objects/Mongo Settings")]
    public static void OpenWindow()
    {
        GetWindow<MongoSettingsBuilder>().Show();
    }

    private const string AssetFolderPath = "Assets/ScriptableObjects/Mongo Settings";
    CreateNewMongoSetting _mongoSettings;

    private void OnDestroy()
    {
        if(_mongoSettings !=null)
        {
            DestroyImmediate(_mongoSettings.MongoSetting);
        }
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();

        _mongoSettings = new CreateNewMongoSetting();
        tree.Add("New Settings", _mongoSettings);
        tree.AddAllAssetsAtPath("Mongo Settings", AssetFolderPath, typeof(MongoSettings));


        return tree;
    }

    public class CreateNewMongoSetting
    {
        public CreateNewMongoSetting()
        {
            MongoSetting = ScriptableObject.CreateInstance<MongoSettings>();
        }

        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public MongoSettings MongoSetting;

        [Button("Create")]
        public void CreateNewSetting()
        {
            AssetDatabase.CreateAsset(MongoSetting, Path.Combine(AssetFolderPath, $"Mongo Settings - {MongoSetting.ClusterName} {MongoSetting.Username}.asset"));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            MongoSetting = ScriptableObject.CreateInstance<MongoSettings>();
        }
    }

}
