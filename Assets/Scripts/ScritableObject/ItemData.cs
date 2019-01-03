using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class ItemData : ScriptableObject
{

    public List<ItemStatus> item;
}

[System.Serializable]
public class ItemStatus
{
    public int itemID;
    public Sprite itemSprite;
    public string itemName;
    [Multiline]
    public string itemDesc;
    public float dropProbability;
    public ItemType itemType;
    public int price;
}

public enum ItemType
{
    Consumable,
    Equipment,
    Craft,
    Collection,
}