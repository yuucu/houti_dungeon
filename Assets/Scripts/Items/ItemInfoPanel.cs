using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoPanel : MonoBehaviour
{

    [SerializeField]
    private Image itemInfoImage;
    [SerializeField]
    private Text itemInfoName;
    [SerializeField]
    private Text itemInfoDesc;

    [SerializeField]
    private GameObject kouka;
    [SerializeField]
    private Text itemKouka;

    [SerializeField]
    private GameObject itemUseButton;
    [SerializeField]
    private Button itemSellButton;

    public void set(ItemStatus item)
    {
        itemInfoImage.sprite = item.itemSprite;
        itemInfoName.text = item.itemName;
        itemInfoDesc.text = item.itemDesc;

        itemUseButton.GetComponent<ItemUseButton>().setItem(item);

        if (item.itemType == ItemType.Consumable)
        {
            kouka.SetActive(true);
            itemKouka.text = ItemDataBase.Instance.getConsumableItemStatus(item.itemID).desc;
        }
        else
        {
            kouka.SetActive(false);
            itemKouka.text = "";
        }
    }


}
