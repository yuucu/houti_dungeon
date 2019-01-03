using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StageDataBase : SingletonMono<StageDataBase>
{
    [SerializeField]
    private List<UserStageData> userStageData;
    [SerializeField]
    private UserLastPlayStageSaveData userLastPlayStage;
    private StageData[] stageData;
    private GameManager gameManager;
    private SystemText systemText;

    private StageData currentStage;
    private int turn;
    private bool clearFlag;

    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);

        stageData = Resources.LoadAll<StageData>("Data/StageData/");
        userStageData = new List<UserStageData>();
        userLastPlayStage = new UserLastPlayStageSaveData();
        turn = 0;
    }

    void Start()
    {
        DebugText.push("StageDataBase Start()");
        gameManager = GameManager.Instance;
        systemText = SystemText.Instance;
    }

    public void playerDeath()
    {
        turn = 0;
        userLastPlayStage.exist = false;
    }

    public void nextTurn()
    {
        bool b = false;
        turn += 1;
        foreach (UserStageData stage in userStageData)
        {
            if (currentStage.stageID == stage.stageID)
            {
                b = true;
                if (turn > stage.maxReachTurn)
                    stage.maxReachTurn = turn;

            }
        }
        if (!b)
        {
            UserStageData newStage = new UserStageData();
            newStage.stageID = currentStage.stageID;
            newStage.maxReachTurn = 1;
            newStage.completeStage = false;
            userStageData.Add(newStage);
        }
        userLastPlayStage.stageID = currentStage.stageID;
        userLastPlayStage.turn = turn;
        userLastPlayStage.exist = true;

    }

    public void townBack()
    {
        clearFlag = false;
    }

    [SerializeField]
    private GameObject gameClearText;
    public void stageExit()
    {
        GameObject obj = Instantiate(gameClearText, new Vector3(100.12f, 102.25f, 0), transform.rotation) as GameObject;
        Destroy(obj, 5f);

        foreach (UserStageData stage in userStageData)
        {
            if (currentStage.stageID == stage.stageID)
            {
                stage.maxReachTurn = getStage(currentStage.stageID).turn.Count * 10;
                stage.completeStage = true;
            }
        }
        userLastPlayStage.exist = false;
        turn = 0;
        clearFlag = true;
    }

    public UserLastPlayStageSaveData getLastPlayStage()
    {
        if (userLastPlayStage != null)
            return userLastPlayStage;

        return null;
    }


    public List<UserStageData> getUserStageData()
    {
        return userStageData;
    }

    // - - - - - - - - - save() & load() - - - - - - - - - - - - - -

    public void save()
    {
        DebugText.push("StageDataBase saving...");

        SaveData.SetClass<UserLastPlayStageSaveData>("userLast", userLastPlayStage);
        SaveData.SetList<UserStageData>("userStage", userStageData);
    }

    public void load()
    {
        DebugText.push("StageDataBase loading...");

        userLastPlayStage = SaveData.GetClass<UserLastPlayStageSaveData>("userLast", new UserLastPlayStageSaveData());
        userStageData = SaveData.GetList<UserStageData>("userStage", new List<UserStageData>());
    }

    public void createSaveData()
    {
        UserLastPlayStageSaveData userLast = new UserLastPlayStageSaveData();
        userLast.stageID = 0;
        userLast.turn = 0;
        userLast.exist = false;

        SaveData.SetClass<UserLastPlayStageSaveData>("userLast", userLast);

    }

    // - - - - - - - - - - -いろいろ - - - - - - -
    public void setCurrentStage(StageData stageData)
    {
        turn = 0;
        userLastPlayStage.exist = false;
        currentStage = stageData;
        gameManager.gameStart(stageData.stageID);
        systemText.updateLogText(stageData.stageName + "　に挑戦！");
    }


    // interruptStageButtonからのみ呼んでください
    public void setRestartStage()
    {
        turn = userLastPlayStage.turn - 1;
        currentStage = getStage(userLastPlayStage.stageID);

        gameManager.gameStart(currentStage.stageID);
        systemText.updateLogText(currentStage.stageName + "の" + " Turn" + userLastPlayStage.turn + " から再出発！");
    }

    public StageData getCurrentStage()
    {
        return currentStage;
    }

    public int getCurrentTurn()
    {
        if (clearFlag)
            return 0;
        return turn;
    }

    public StageData getStage(int stageID)
    {
        foreach (StageData stage in stageData)
            if (stage.stageID == stageID)
                return stage;
        return null;
    }

    public bool getClearFlag()
    {
        return clearFlag;
    }

    // Goal Objectから設定してもらおう
    public void setClearFlag(bool b)
    {
        clearFlag = b;
    }
}

// Stage セーブデータの定義クラス
[System.Serializable]
public class UserStageData
{
    public int stageID;
    public int maxReachTurn;
    public bool completeStage;
}

[System.Serializable]
public class UserLastPlayStageSaveData
{
    public int stageID;
    public int turn;
    public bool exist;
}