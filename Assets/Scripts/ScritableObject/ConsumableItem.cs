using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class ConsumableItem : ScriptableObject
{
    public List<ConsumableItemStatus> item;
}

[System.Serializable]
public class ConsumableItemStatus
{
    public int itemID;
    public ConsumableType type;
    public int healPoint;
    public int str;
    public string desc;

}

public enum ConsumableType
{
    Life,
    Attack,
    Buf,
}