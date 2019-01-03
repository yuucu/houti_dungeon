using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{

    [SerializeField]
    private int lv = 1;
    [SerializeField]
    private int haveExp = 0;
    [SerializeField]
    private int nextLvExp;
    [SerializeField]
    private float moveSpeed;

    private GameObject lvUPText;
    private GameObject healParticle;
    private GameObject healText;
    private PlayerData playerdata;
    private bool moveFlag;

    private GameManager gameManager;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerdata = Resources.Load<PlayerData>("Data/playerData");
        lvUPText = Resources.Load<GameObject>("Prefabs/Text/lvupText");
        healParticle = Resources.Load<GameObject>("Prefabs/Particles/heal");
        healText = Resources.Load<GameObject>("Prefabs/Player/PlayerHealUI");
        livingFlag = true;
        initPlayerStatus();
    }

    // Use this for initialization
    void Start()
    {
        DebugText.push("Player Start()");

        gameManager = GameManager.Instance;
        systemText = SystemText.Instance;

        gameManager.setPlayer(this);
        ItemDataBase.Instance.setPlayer(this);
        EquipmentItemDataBase.Instance.setPlayer(this);
        systemText.setPlayer(this);

        switch (gameManager.getGameMode())
        {
            case GameManager.GameMode.Town1:
                setTownPos();
                break;
            case GameManager.GameMode.Town2:
                setTownPos2();
                break;
            case GameManager.GameMode.Stage:
                setStagePos();
                break;
        }

        gameManager.load();

    }

    // Update is called once per frame
    void Update()
    {
        switch (gameManager.getGameMode())
        {
            case GameManager.GameMode.Town1:
                townUpdate();
                break;
            case GameManager.GameMode.Town2:
                townUpdate();
                break;
            case GameManager.GameMode.Stage:
                stageUpdate();
                break;
        }
    }

    private Vector3 targetPos;
    private bool moveRight;

    private void townUpdate()
    {
        if (currentHp > eqHp + maxHp)
            currentHp = eqHp + maxHp;

        if (moveFlag)
        {
            if (targetPos == null)
                targetPos = transform.position;

            float x = 1.5f + ((moveSpeed + eqMoveSpd) * 0.2f);

            if (moveRight)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                if (transform.position.x > targetPos.x)
                {
                    moveFlag = false;
                    x = 0;
                }
            }

            if (!moveRight)
            {
                x = -x;
                transform.rotation = Quaternion.Euler(0, 180, 0);
                if (transform.position.x < targetPos.x)
                {
                    moveFlag = false;
                    x = 0;
                }
            }


            rb2d.velocity = new Vector2(x, rb2d.velocity.y);

            animator.SetBool("moveFlag", moveFlag);

        }
    }
    public void setTargetPos(float posX, bool r)
    {
        moveRight = r;
        targetPos = new Vector3(posX, transform.position.y, transform.position.z);
        moveFlag = true;
    }

    private void stageUpdate()
    {
        if (livingFlag)
        {
            if (currentHp > eqHp + maxHp)
                currentHp = eqHp + maxHp;

            if (timeElapsed < 1.2 - ((attackSpd + eqAttackSpd) * 0.07))
                timeElapsed += Time.deltaTime;

            animator.SetBool("moveFlag", moveFlag);

            if (moveFlag)
                rb2d.velocity = new Vector2(1.6f + ((moveSpeed + eqMoveSpd) * 0.2f), rb2d.velocity.y);

            Vector3 pos = new Vector3(transform.position.x + 0.3f, transform.position.y - 0.1f, transform.position.z);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector3.right, 0.1f);
            Debug.DrawRay(pos, new Vector3(0.1f, -0.1f, 0), Color.blue);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    moveFlag = false;
                    if (timeElapsed >= 1.2 - ((attackSpd + eqAttackSpd) * 0.07))
                    {
                        attack();
                        timeElapsed = 0.0f;
                    }
                }
            }
            else
                moveFlag = true;
        }
    }


    protected override void attack()
    {
        animator.SetTrigger("Attack");
        Vector3 pos = new Vector3(transform.position.x + 0.4f, transform.position.y - 0.1f, transform.position.z);
        GameObject attack = Instantiate(attackRange, pos, transform.rotation);
        attack.transform.parent = transform;
        attack.GetComponent<Attack>().set(str + eqStr);
    }

    protected override void destroyUnit()
    {
        DebugText.push("Player destroyUnit");

        livingFlag = false;
        animator.SetBool("isLiving", livingFlag);

        systemText.updateLogText("主人公は死んでしまった");
        StartCoroutine(gameManager.playerDeath());
    }

    // playerのstatusの初期化
    public void initPlayerStatus()
    {
        DebugText.push("init Player Status");

        lv = 1;
        haveExp = 0;
        setInitStatus(playerdata.status);
        moveSpeed = playerdata.status.moveSpd;
    }

    public void restartPlayer()
    {
        currentHp = maxHp / 2;
    }

    public void setStagePos()
    {
        DebugText.push("Player set Stage Pos");
        livingFlag = true;
        animator.SetBool("isLiving", livingFlag);
        transform.position = new Vector3(96.9f, 101.8f, 0);
    }

    public void setTownPos()
    {
        DebugText.push("Player set Town Pos");

        livingFlag = true;
        animator.SetBool("isLiving", livingFlag);
        transform.position = new Vector3(92f, 101.8f, 0);
    }

    public void setTownPos2()
    {
        livingFlag = true;
        animator.SetBool("isLiving", livingFlag);
        transform.position = new Vector3(98f, 101.8f, 0);
    }

    // ダンジョンでの経験値
    public void addExp(int exp)
    {
        if (livingFlag)
        {
            haveExp += exp;
            while (haveExp > getTotalNextEXP(lv))
            {
                lv++;
                GameObject obj = Instantiate(lvUPText, transform.position, transform.rotation) as GameObject;
                obj.transform.parent = transform;
                Destroy(obj, 2f);
                setLv(lv);
                systemText.updateLogText("レベルが " + lv + " になった！");
            }
        }
    }

    // Load時 経験値
    public void setExp(int exp)
    {
        haveExp = exp;
        while (haveExp > getTotalNextEXP(lv))
        {
            lv++;
            setLv(lv);
        }
    }

    private void setLv(int lv)
    {
        this.lv = lv;
        str = playerdata.status.str + (lv) - 1;
        def = playerdata.status.def + (lv) - 1;
        maxHp = playerdata.status.maxHp + ((lv - 1) * 5);
    }

    protected override void createDamageText(int i)
    {
        GameObject obj = Instantiate(damageText, transform.position, transform.rotation) as GameObject;
        obj.GetComponent<TextMesh>().text = i.ToString();
        obj.transform.parent = transform;
    }


    public void heal(int num)
    {

        GameObject obj = Instantiate(healParticle, transform.position, transform.rotation) as GameObject;
        obj.transform.parent = transform;
        Destroy(obj, 2f);

        GameObject obj2 = Instantiate(healText, transform.position, transform.rotation) as GameObject;
        obj2.GetComponent<TextMesh>().text = num.ToString();

        if (currentHp > 0)
        {
            currentHp += num;
            if (currentHp > (maxHp + eqHp))
                currentHp = maxHp + eqHp;

            if (currentHp <= 0)
                currentHp = 1;
        }
    }

    // - - - - - save, load - - - - - - - - - - - - - - - -  - - - 


    // 初回起動時のみ
    public void createSaveData()
    {
        DebugText.push("Player create Save!");

        UserStatusSaveData userStatus = new UserStatusSaveData();
        userStatus.haveEXP = 0;
        userStatus.currentHP = playerdata.status.maxHp;
        SaveData.SetClass<UserStatusSaveData>("userStatus", userStatus);
    }

    public void save()
    {
        DebugText.push("Player Saving...");
        UserStatusSaveData userStatus = new UserStatusSaveData();
        userStatus.haveEXP = haveExp;
        userStatus.currentHP = currentHp;
        SaveData.SetClass<UserStatusSaveData>("userStatus", userStatus);
    }

    public void load()
    {
        DebugText.push("Player Loading...");

        UserStatusSaveData userStatus = SaveData.GetClass<UserStatusSaveData>("userStatus", new UserStatusSaveData());
        haveExp = userStatus.haveEXP;
        currentHp = userStatus.currentHP;

        setExp(haveExp);
    }

    // - - - - -Setter(), Getter() - - - - - - - - - - - - - - - -  - - - 

    public int getHaveEXP()
    {
        return haveExp - getTotalNextEXP(lv - 1);
    }

    public int getTotalHaveExp()
    {
        return haveExp;
    }

    public int getNextLvUpEXP()
    {
        return ((lv * lv) + (lv + 1) * (lv + 1));
    }

    // LV: numまでの獲得経験値を返す
    public int getTotalNextEXP(int num)
    {
        int totalNextExp = 0;
        for (int i = 1; i <= num; i++)
        {
            totalNextExp += ((i * i) + ((i + 1) * (i + 1)));
        }

        return totalNextExp;
    }

    public int getLv()
    {
        return lv;
    }
    public float getMoveSpeed()
    {
        return moveSpeed + eqMoveSpd;
    }

    public void setEquipment(float str, float def, float attackSpd, float moveSpd, int hp)
    {
        eqStr = str;
        eqDef = def;
        eqAttackSpd = attackSpd;
        eqMoveSpd = moveSpd;
        eqHp = hp;
    }

    public void setMoveFlag(bool b)
    {
        moveFlag = b;
    }

}


// SaveData定義クラス
[System.Serializable]
public class UserStatusSaveData
{
    public int haveEXP = 0;
    public int currentHP = 10;
}
