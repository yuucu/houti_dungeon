using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeEquipmentButton : MonoBehaviour
{

    private int eqID = -1;
    private int eqLV;
    private int itemID = -1;
    [SerializeField]
    private Text priceText;
    [SerializeField]
    private Text num;

    private int price;
    private int upgradeCount;

    private CraftItemData craftItemData;


    public void OnClick()
    {
        if (eqID != -1)
        {
            if (ItemDataBase.Instance.use(0, price))
            {
                if (EquipmentItemDataBase.Instance.upgradeEq(eqID, eqLV, upgradeCount))
                    ItemDataBase.Instance.use(itemID, int.Parse(this.num.text));
            }
            else
                SystemText.Instance.updateLogText("お金が足りません");

        }
    }

    public void setEq(int id, int lv)
    {
        eqID = id;
        eqLV = lv;

        setPrice();
    }

    public void setItem(int id)
    {
        if (craftItemData == null)
            craftItemData = Resources.Load<CraftItemData>("Data/craftItem");

        itemID = id;
        setPrice();
    }

    public void setPrice()
    {
        if (itemID != -1)
            foreach (CraftItemStatus craft in craftItemData.item)
            {
                if (craft.itemID == itemID)
                {
                    upgradeCount = craft.updataCount * int.Parse(this.num.text);
                    int num = eqLV * 100 * int.Parse(this.num.text);
                    price = num;
                }
            }

        if (eqID != -1 && itemID != -1)
        {
            priceText.text = price + "\tG";
            if (price <= ItemDataBase.Instance.getItemValue(0))
                GetComponent<Button>().interactable = true;
            else
                GetComponent<Button>().interactable = false;
        }
    }

    public void reset()
    {
        eqID = -1;
        itemID = -1;
        GetComponent<Button>().interactable = false;
    }
}
