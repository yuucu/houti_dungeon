using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyButton : MonoBehaviour
{
    private ItemStatus item;

    public void setItem(ItemStatus item)
    {
        this.item = item;
    }

    public void OnClick()
    {
        DebugText.push("buy button ");

        if (item.itemType == ItemType.Consumable)
        {
            if (ItemDataBase.Instance.use(0, item.price * 10))
            {
                ItemDataBase.Instance.add(item.itemID, 10);
                SystemText.Instance.updateLogText(item.itemName + "を買った");
                return;
            }
            else
                SystemText.Instance.updateLogText("お金が足りません");

        }
        else
        {
            if (ItemDataBase.Instance.use(0, item.price))
            {
                ItemDataBase.Instance.add(item.itemID, 1);
                SystemText.Instance.updateLogText(item.itemName + "を買った");
                return;
            }
            else
                SystemText.Instance.updateLogText("お金が足りません");

        }

    }
}
