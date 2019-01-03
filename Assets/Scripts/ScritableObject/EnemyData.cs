using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class EnemyData : ScriptableObject {

    public List<EnemyStatus> enemyList;
}

[System.Serializable]
public class EnemyStatus : UnitStatus
{
    public int enemyID;
    public Sprite enemySprite;
    public string name;
    public int exp;
    public int coinRange;
    public List<int> dropItemID;

}
