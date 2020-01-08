using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Rewards : MonoBehaviour
{
    [TableList(ShowIndexLabels = false)]
    public List<ItemSlot> Items;

    public void GiveRewards()
    {
        //TODO - Give the rewards to the inventory
        DestroyImmediate(this);
    }
}

[System.Serializable]
public class ItemSlot
{
    [AssetSelector]
    public Item StorageItem;
    
    public uint Amount;
}
