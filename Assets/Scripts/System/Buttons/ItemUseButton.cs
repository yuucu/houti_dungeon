using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUseButton : MonoBehaviour
{

    private ItemStatus item;

    public void setItem(ItemStatus item)
    {
        this.item = item;
        if (item.itemType == ItemType.Consumable)
            this.GetComponent<Button>().interactable = true;
        else
            this.GetComponent<Button>().interactable = false;
    }


    public void OnClick()
    {
        ItemDataBase.Instance.use(item.itemID, 1);
    }


    [SerializeField]
    private GameObject sellPanel;
    [SerializeField]
    private Button sellButton;
    [SerializeField]
    private Text priceText;

    public void OnClickSell()
    {
        ItemDataBase itemDataBase = ItemDataBase.Instance;
        priceText.text = itemDataBase.getPrice(item.itemID, itemDataBase.getItemValue(item.itemID)) + " G で売りますか？";
        sellPanel.SetActive(true);
        sellButton.onClick.RemoveAllListeners();
        sellButton.onClick.AddListener(() =>
        {
            ItemDataBase.Instance.sell(item.itemID);
        });
    }
}
