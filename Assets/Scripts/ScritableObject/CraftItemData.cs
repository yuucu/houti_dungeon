using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class CraftItemData : ScriptableObject
{
    public List<CraftItemStatus> item;
}


[System.Serializable]
public class CraftItemStatus
{
    public int itemID;
    public int updataCount;
}