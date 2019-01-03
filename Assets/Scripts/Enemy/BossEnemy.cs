using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{


    protected override IEnumerator coinInstantiate(int coin)
    {
        if (coin != 0)
        {
            if (coin == 1)
                systemText.updateLogText("コインを" + "ゲットしました");
            else
                systemText.updateLogText("コインを" + coin + "枚ゲットしました");
        }

        for (int i = 0; i < coin; i++)
        {
            GameObject go = (GameObject)Instantiate(dropItem, new Vector2(transform.position.x + Random.Range(0, 0.1f), transform.position.y + Random.Range(0, 0.1f)), transform.rotation);
            go.GetComponent<Item>().setItemID(0);
            if (i % 3 == 0)
                yield return null;
        }
    }
}
