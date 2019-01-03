using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemText : SingletonMono<SystemText>
{

    private ItemDataBase itemDataBase;
    private EquipmentItemDataBase equipmentDataBase;

    [SerializeField]
    private Player player;

    [SerializeField]
    private Slider hpBar;
    [SerializeField]
    private Slider expBar;
    [SerializeField]
    private Text hpText;
    [SerializeField]
    private Text expText;
    [SerializeField]
    private Text lvText;
    [SerializeField]
    private Text statusText;
    [SerializeField]
    private Text logText;
    [SerializeField]
    private Text coinText;

    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        logTextList = new List<string>();
    }

    void Start()
    {
        itemDataBase = ItemDataBase.Instance;
        equipmentDataBase = EquipmentItemDataBase.Instance;

        DebugText.push("SystemText Start()");
    }


    // - - - - - LogText - - - - - - - - - -
    private List<string> logTextList;
    public void updateLogText(string str)
    {
        logTextList.Add(str);
        string log = "";

        int logCount = 0;
        for (int i = logTextList.Count - 1; i >= 0; i--)
        {
            log = logTextList[i] + "\n" + log;
            logCount += 1;
            if (logCount == 4)
                break;
        }
        logText.text = log;
    }

    public void resetText()
    {
        logTextList.Clear();
    }


    // Update is called once per frame
    void Update()
    {
        // - - - - - - -StatusPanel - - - - - - - - -
        if (player != null)
        {

            // hp
            hpBar.value = (player.getCurrentHp() * hpBar.maxValue) / player.getMaxHp();
            hpText.text = player.getCurrentHp() + " / " + player.getMaxHp();

            // exp
            expBar.value = Mathf.FloorToInt(player.getHaveEXP() * expBar.maxValue) / player.getNextLvUpEXP();
            int nextExp = player.getTotalNextEXP(player.getLv()) + 1;
            expText.text = player.getTotalHaveExp() + " / " + nextExp;

            // Lv
            lvText.text = player.getLv().ToString();

            // status
            statusText.text = player.getLv() + "\n" + player.getStr() + "\n" + player.getDef() + "\n" + player.getAttackSpd() + "\n" + player.getMoveSpeed() + "\n"
                + equipmentDataBase.getIsWeapon() + "\n" + equipmentDataBase.getIsArmor() + "\n" + equipmentDataBase.getIsAcce();

            // coin
            coinText.text = itemDataBase.getItemValue(0).ToString();
        }
    }

    public void setPlayer(Player player)
    {
        this.player = player;
    }


    [SerializeField]
    private Toggle homeToggle;
    public void resetToggle()
    {
        homeToggle.isOn = true;
    }

    [SerializeField]
    private GameObject turnTextNormal;
    [SerializeField]
    private GameObject turnTextBoss;

    [SerializeField]
    private GameObject parentObj;
    private GameObject obj = null;
    public void createTurnText(int turn)
    {
        if (parentObj == null)
            parentObj = GameObject.FindGameObjectWithTag("turnObj");

        if (turn != 0 && turn % 10 == 0)
            obj = Instantiate(turnTextBoss, parentObj.transform.position, parentObj.transform.rotation) as GameObject;
        else
            obj = Instantiate(turnTextNormal, parentObj.transform.position, transform.rotation) as GameObject;

        obj.transform.parent = parentObj.transform;
        obj.GetComponent<TextMesh>().text = "turn " + turn.ToString();

        Destroy(obj, 5f);
    }
}
