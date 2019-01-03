using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
[System.Serializable]
public class PlayerData : ScriptableObject {

    public PlayerStatus status; 
}

[System.Serializable]
public class PlayerStatus :UnitStatus
{
    public float moveSpd;
}