using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
[System.Serializable]
public class ShopData : ScriptableObject {

    public List<ShopItemStatus> item;
}

[System.Serializable]
public class ShopItemStatus
{
    public int ItemID;
}
