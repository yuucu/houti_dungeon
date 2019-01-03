using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class EquipmentItem : ScriptableObject
{
    public List<EquipmentItemStatus> item;
}


[System.Serializable]
public class EquipmentItemStatus
{
    public int itemID;
    public EquipmentType equipmentType;
    public int str;
    public int def;
    public int hp;
    public int attackSpd;
    public int moveSpd;

}

[System.Serializable]
public enum EquipmentType
{
    Weapon,
    Armor,
    Accessory,
}
