using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private ItemStatus itemStatus;

    public void setItemID(int id)
    {
        itemStatus = ItemDataBase.Instance.getItemStatus(id);
        if (itemStatus == null)
        {
            Destroy(this.gameObject);
            return;
        }

        ItemDataBase.Instance.add(id, 1);
        GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-40, 70), Random.Range(70, 250) + 100 / itemStatus.dropProbability, 0));
        gameObject.GetComponent<SpriteRenderer>().sprite = itemStatus.itemSprite;
        Destroy(this.gameObject, 2f);
    }

}
