using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonMono<EnemyManager>
{

    private GameManager gameManager;
    private SystemText systemText;

    private GameObject enemy;
    private GameObject bossEnemy;
    private EnemyData enemyData;
    private StageData stageData = null;
    private StageDataBase stageDataBase;

    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);

        enemyData = Resources.Load<EnemyData>("Data/enemyData");
        enemy = Resources.Load<GameObject>("Prefabs/Enemys/Enemy");
        bossEnemy = Resources.Load<GameObject>("Prefabs/Enemys/BossEnemy");
    }

    public void Start()
    {
        gameManager = GameManager.Instance;
        systemText = SystemText.Instance;
        stageDataBase = StageDataBase.Instance;
    }

    public void setEnemy()
    {
        stageData = StageDataBase.Instance.getCurrentStage();

        if (stageData != null)
        {
            int currentTurn = stageDataBase.getCurrentTurn();
            int turn10 = Mathf.FloorToInt((currentTurn - 1) / 10);
            if (turn10 < 0)
                turn10 = 0;

            if (turn10 >= stageData.turn.Count)
            {
                gameManager.stageExit();
                return;
            }


            systemText.createTurnText(currentTurn);
            enemyCreate(stageData.turn[turn10].count, stageData.turn[turn10].enemyID);

            if (currentTurn != 0 && currentTurn % 10 == 0)
                bossEnemyCreate(stageData.turn[turn10].bossEnemyID);
        }
    }

    public void destroyEnemys()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemys)
        {
            Destroy(enemy);
        }
    }

    private void enemyCreate(int count, List<int> list)
    {
        for (int i = 0; i < count; i++)
        {
            float x = Random.Range(0, 35);
            int enemyID = list[Random.Range(0, list.Count)];
            GameObject go = (GameObject)GameObject.Instantiate(enemy, new Vector3(x * 0.1f + 99f, 101.15f, 0), Quaternion.identity);
            for (int j = 0; j < enemyData.enemyList.Count; j++)
            {
                if (enemyData.enemyList[j].enemyID == enemyID)
                {
                    go.GetComponent<Enemy>().setEnemyID(enemyData.enemyList[j]);
                }

            }
        }
    }

    private void bossEnemyCreate(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int enemyID = list[i];
            float x = i;
            GameObject go = (GameObject)GameObject.Instantiate(bossEnemy, new Vector3(101 + x, 101.11f, 0), Quaternion.identity);

            for (int j = 0; j < enemyData.enemyList.Count; j++)
            {
                if (enemyData.enemyList[j].enemyID == enemyID)
                {
                    go.GetComponent<Enemy>().setEnemyID(enemyData.enemyList[j]);
                }

            }
        }
    }



}
