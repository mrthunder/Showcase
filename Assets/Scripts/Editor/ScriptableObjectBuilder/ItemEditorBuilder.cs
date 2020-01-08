using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using System.IO;
using Sirenix.OdinInspector.Editor;

public class ItemEditorBuilder : OdinMenuEditorWindow
{
    [MenuItem("Tools/Scriptable Objects/Items Editor")]
    public static void OpenWindow()
    {
        GetWindow<ItemEditorBuilder>().Show();
    }

    private const string AssetFolderPath = @"Assets/ScriptableObjects/Items";
    private CreateNewItem _createNewItem;

    private void OnDestroy()
    {
        if(_createNewItem != null)
        {
            DestroyImmediate(_createNewItem.NewItem);
        }
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();

        _createNewItem = new CreateNewItem();
        tree.Add("New Item", _createNewItem);
        tree.AddAllAssetsAtPath("Items", AssetFolderPath, typeof(Item));


        return tree;
    }

    public class CreateNewItem
    {
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public Item NewItem;

        public CreateNewItem()
        {
            NewItem = ScriptableObject.CreateInstance<Item>();
        }

        [Button("Create")]
        public void OnCreateNewItem()
        {
            AssetDatabase.CreateAsset(NewItem, Path.Combine(AssetFolderPath, $"{NewItem.Name}(Item).asset"));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }


}

