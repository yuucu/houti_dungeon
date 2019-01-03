using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : Attack {

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<Unit>().damage(str);
            Vector3 colPos  =  col.gameObject.transform.position;
            GameObject obj = Instantiate(damageEffect, new Vector3(colPos.x + (Random.Range(0, 5) - 2) * 0.05f, colPos.y - 0.1f + (Random.Range(0, 5) - 2) * 0.1f, colPos.z), Quaternion.identity) as GameObject;
            obj.transform.parent = transform;
            Destroy(obj, 0.5f);
        }
    }
}
