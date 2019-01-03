using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
[System.Serializable]
public class StageData : ScriptableObject {

    public List<enemyInfo> turn;
    public Sprite stageSprite;
    public string stageName;
    public string desc;
    public int stageID;
}

[System.Serializable]
public class enemyInfo
{
    public List<int> enemyID;
    public int count;
    public List<int> bossEnemyID;
}