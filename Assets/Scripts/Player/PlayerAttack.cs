using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : Attack {

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Enemy")
        {
            Unit enemy = col.GetComponent<Unit>();
            enemy.damage(str);
            GameObject obj = Instantiate(damageEffect, enemy.getCenter(), Quaternion.identity);
            obj.transform.parent = transform;
            Destroy(obj, 0.5f);
        }
    }
}
