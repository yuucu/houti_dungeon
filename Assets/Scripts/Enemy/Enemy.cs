using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{

    protected bool foundPlayer;
    [SerializeField]
    private GameObject deathParticle;
    [SerializeField]
    private GameObject canvas;
    private EnemyStatus enemyStatus;

    // itemの元
    public GameObject dropItem;

    private float width;
    private float height;

    void Awake()
    {
        animator = GetComponent<Animator>();
        livingFlag = true;
    }

    void Start()
    {
        systemText = SystemText.Instance;
    }

    void Update()
    {
        if (livingFlag)
        {
            // 敵を見つけ出してからカウントを始める
            if (timeElapsed < 1.3 - (attackSpd * 0.1) && foundPlayer)
                timeElapsed += Time.deltaTime;

            Vector3 pos = new Vector3(transform.position.x - (width / 2) - 0.1f, 101.74f, transform.position.z);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector3.left, width / 5);
            Debug.DrawRay(pos, new Vector3(-width / 3, 0, 0), Color.blue);

            if (hit.collider != null)
                if (hit.collider.gameObject.tag == "Player")
                {
                    foundPlayer = true;
                    if (timeElapsed >= 1.3 - (attackSpd * 0.1))
                        if (hit.collider.gameObject.GetComponent<Unit>().isLiving())
                        {
                            attack();
                            timeElapsed = 0.0f;
                        }
                }
        }
    }

    protected override void attack()
    {
        Vector3 pos = new Vector3(transform.position.x - width / 2 - 0.1f, transform.position.y + height / 2, transform.position.z);
        GameObject attack = Instantiate(attackRange, pos, transform.rotation);
        attack.transform.parent = transform;
        attack.GetComponent<Attack>().set(str);
    }


    protected override void destroyUnit()
    {
        livingFlag = false;
        animator.SetTrigger("Dead");
        systemText.updateLogText(unitName + "を倒した");
        GameManager.Instance.addExp(enemyStatus.exp);

        int coin = Random.Range(0, enemyStatus.coinRange);
        StartCoroutine(coinInstantiate(coin));

        itemInstantiate();
        Instantiate(deathParticle, new Vector3(unitCenter.x, unitCenter.y - height / 5, unitCenter.z), Quaternion.Euler(-90, 0, 0));
        Destroy(this.gameObject, 0.45f);
    }

    protected virtual IEnumerator coinInstantiate(int coin)
    {
        for (int i = 0; i < coin; i++)
        {
            GameObject go = (GameObject)Instantiate(dropItem, new Vector2(unitCenter.x + Random.Range(0, 0.1f), unitCenter.y + Random.Range(0, 0.1f)), transform.rotation);
            go.GetComponent<Item>().setItemID(0);
        }

        if (coin != 0)
        {
            if (coin == 1)
                systemText.updateLogText("コインを" + "ゲットした");
            else
                systemText.updateLogText("コインを" + coin + "枚ゲットした");
        }
        yield return null;
    }

    protected virtual void itemInstantiate()
    {
        for (int i = 0; i < enemyStatus.dropItemID.Count; i++)
        {
            ItemStatus item = ItemDataBase.Instance.getItemStatus(enemyStatus.dropItemID[i]);
            if (item != null)
            {
                if (Random.value < item.dropProbability / 100)
                {
                    GameObject go = (GameObject)Instantiate(dropItem, new Vector2(unitCenter.x + Random.Range(0, 0.1f), unitCenter.y + Random.Range(0, 0.1f)), transform.rotation);
                    go.GetComponent<Item>().setItemID(item.itemID);
                    systemText.updateLogText(item.itemName + "を" + "ゲットした！");
                }
            }
        }
    }

    public void setEnemyID(EnemyStatus enemyStatus)
    {
        this.enemyStatus = enemyStatus;

        setInitStatus(enemyStatus);
        unitName = enemyStatus.name;
        SpriteRenderer go = gameObject.GetComponent<SpriteRenderer>();
        go.sprite = enemyStatus.enemySprite;
        this.gameObject.AddComponent<BoxCollider2D>();
        width = go.bounds.size.x;
        height = go.bounds.size.y;
        GetComponent<BoxCollider2D>().size = new Vector2(width, height);

        unitCenter = new Vector2(transform.position.x, transform.position.y + (height / 2));


        canvas.transform.position = new Vector3(transform.position.x, transform.position.y + height * 1.2f, 0);
    }


}
