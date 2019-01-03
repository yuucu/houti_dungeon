using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    [SerializeField]
    private Player player;

    private EnemyManager enemyManager;
    private ItemDataBase itemDataBase;
    private EquipmentItemDataBase equipmentItemDataBase;
    private GameFade gameFade;
    private SystemText systemText;
    private StageDataBase stageDataBase;

    private GameMode mode;
    public enum GameMode
    {
        Town1,
        Town2,
        Stage,
    }

    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        DebugText.push("GameManager Start()");

        player = GameObject.Find("Player").GetComponent<Player>();
        systemText = SystemText.Instance;
        enemyManager = EnemyManager.Instance;
        itemDataBase = ItemDataBase.Instance;
        equipmentItemDataBase = EquipmentItemDataBase.Instance;
        stageDataBase = StageDataBase.Instance;
        gameFade = GameObject.Find("FadePanel").GetComponent<GameFade>();
        gameFade.hide();

        // 初回起動時処理
        if (SaveData.GetInt("createSaveDataFlag", 0) == 0)
        {
            SaveData.SetInt("createSaveDataFlag", 1);
            createSaveData();
        }
    }

    public void gameStart(int id)
    {
        DebugText.push("GameManager stageSet ");
        save();
        mode = GameMode.Stage;
        FadeManager.Instance.LoadScene("Stage" + id, 1f);
        StartCoroutine(gameStartAfter());
    }

    // SceneFadeする時間待ってから処理
    public IEnumerator gameStartAfter()
    {
        yield return new WaitForSeconds(1f);
        systemText.resetToggle();
    }

    public void townBack()
    {
        DebugText.push("GameManager townBack");
        stageDataBase.townBack();
        player.setMoveFlag(false);
        save();
        mode = GameMode.Town1;
        FadeManager.Instance.LoadScene("Town", 1f);
        StartCoroutine(townBackAfter());
    }

    // SceneFadeする時間待ってから処理
    public IEnumerator townBackAfter()
    {
        yield return new WaitForSeconds(1f);
        systemText.resetToggle();
        systemText.updateLogText("街に戻ってきました。");
    }

    public void enterKaziya()
    {
        mode = GameMode.Town1;
        save();
        FadeManager.Instance.LoadScene("Kaziya", 0.5f);
    }
    public void enterYadoya()
    {
        mode = GameMode.Town1;
        save();
        FadeManager.Instance.LoadScene("Yadoya", 0.5f);
    }
    public void enterCastle()
    {
        mode = GameMode.Town1;
        save();
        FadeManager.Instance.LoadScene("Castle", 0.5f);
    }
    public void townBack2()
    {
        save();
        FadeManager.Instance.LoadScene("Town", 0.5f);
        mode = GameMode.Town1;
    }
    public void townBackYadoya()
    {
        save();
        FadeManager.Instance.LoadScene("Town", 0.5f);
        mode = GameMode.Town2;
    }

    public IEnumerator playerDeath()
    {
        DebugText.push("GameManager playerDeath");
        switch (mode)
        {
            case GameMode.Town1:
                stageDataBase.playerDeath();
                //    player.initPlayerStatus();
                save();
                yield return new WaitForSeconds(1f);
                gameFade.fadeOut(1f);
                yield return new WaitForSeconds(1f);
                player.setTownPos();
                gameFade.fadeIn(0.5f);
                yield return new WaitForSeconds(0.5f);
                break;
            case GameMode.Town2:
                stageDataBase.playerDeath();
                player.restartPlayer();
                save();
                yield return new WaitForSeconds(1f);
                gameFade.fadeOut(1f);
                yield return new WaitForSeconds(1f);
                player.setTownPos();
                gameFade.fadeIn(0.5f);
                yield return new WaitForSeconds(0.5f);
                break;

            case GameMode.Stage:

                yield return new WaitForSeconds(1f);
                gameFade.fadeOut(1f);
                yield return new WaitForSeconds(1f);
                stageDataBase.playerDeath();
                player.restartPlayer();
                save();
                enemyManager.destroyEnemys();
                player.setStagePos();
                gameFade.fadeIn(0.5f);
                yield return new WaitForSeconds(0.5f);
                systemText.updateLogText("再出発！");

                break;
        }
    }

    public IEnumerator startTurn()
    {
        DebugText.push("GameManager startTurn");
        // 次の階に行くときに毎回セーブ
        stageDataBase.nextTurn();
        save();
        gameFade.fadeOut(0.5f);
        yield return new WaitForSeconds(0.5f);
        enemyManager.setEnemy();
        player.setStagePos();
        int turn = stageDataBase.getCurrentTurn();
        if (turn != 0)
            systemText.updateLogText("Turn : " + turn);
        gameFade.fadeIn(0.5f);
        yield return new WaitForSeconds(0.5f);
    }

    public void stageExit()
    {
        stageDataBase.stageExit();
    }

    // 初回起動時のみ
    private void createSaveData()
    {
        DebugText.push("create SaveData!");

        player.createSaveData();
        itemDataBase.createSaveData();
        equipmentItemDataBase.createSaveData();
        stageDataBase.createSaveData();
        SaveData.Save();
    }

    // - - - - - - save() & load() - - - - - -
    public void save()
    {
        DebugText.push("Saving...");
        player.save();
        itemDataBase.save();
        equipmentItemDataBase.save();
        stageDataBase.save();
        SaveData.Save();
    }

    public void load()
    {
        DebugText.push("Loading...");

        player.load();
        itemDataBase.load();
        equipmentItemDataBase.load();
        stageDataBase.load();
    }


    public void addExp(int exp)
    {
        player.addExp(exp);
    }

    public GameMode getGameMode()
    {
        return mode;
    }

    public void setPlayer(Player player)
    {
        this.player = player;
    }

}
