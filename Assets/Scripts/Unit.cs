using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    protected Rigidbody2D rb2d;
    protected Animator animator;
    protected SystemText systemText;

    public GameObject attackRange;
    public GameObject damageText;

    [SerializeField]
    protected UnitStatus initStatus;


    [SerializeField]
    protected string unitName;
    [SerializeField]
    protected float str;
    [SerializeField]
    protected float def;
    [SerializeField]
    protected int maxHp;
    [SerializeField]
    protected float attackSpd;

    [SerializeField]
    protected float eqStr = 0;
    [SerializeField]
    protected float eqDef = 0;
    [SerializeField]
    protected int eqHp = 0;
    [SerializeField]
    protected float eqAttackSpd = 0;
    [SerializeField]
    protected float eqMoveSpd = 0;




    [SerializeField]
    protected int currentHp;
    protected float timeElapsed;
    protected bool livingFlag;

    protected Vector3 unitCenter;

    public virtual void damage(float str)
    {
        if (livingFlag)
        {
            animator.SetTrigger("Hit");
            // ダメージ計算式,悩み中 -- - - -
            int damage = Mathf.RoundToInt((str * str) / (def + eqDef));
            if (damage <= 0)
                damage = 1;

            currentHp -= damage;
            createDamageText(damage);
            if (currentHp <= 0)
            {
                currentHp = 0;
                destroyUnit();
            }
        }
    }

    public void setInitStatus(UnitStatus status)
    {
        initStatus = status;
        str = status.str;
        def = status.def;
        maxHp = status.maxHp;
        attackSpd = status.attackSpd;
        currentHp = maxHp + eqHp;
    }

    protected virtual void createDamageText(int i)
    {
        GameObject obj = Instantiate(damageText, unitCenter, transform.rotation) as GameObject;
        obj.GetComponent<TextMesh>().text = i.ToString();
    }

    protected virtual void destroyUnit()
    {
        livingFlag = false;
    }

    protected virtual void attack()
    {
        Debug.Log("Attack");
    }

    public int getMaxHp()
    {
        return maxHp + eqHp;
    }

    public int getCurrentHp()
    {
        return currentHp;
    }

    public bool isLiving()
    {
        return livingFlag;
    }

    public Vector3 getCenter()
    {
        return unitCenter;
    }

    public float getStr()
    {
        return str + eqStr;
    }

    public float getDef()
    {
        return def + eqDef;
    }

    public float getAttackSpd()
    {
        return attackSpd + eqAttackSpd;
    }
}
