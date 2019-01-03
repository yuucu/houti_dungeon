using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ShopDataBase : MonoBehaviour
{

    [SerializeField]
    private ShopData shopData;
    private ItemDataBase itemDataBase;
    private EquipmentItemDataBase equipmentItemDataBase;

    void Start()
    {
        itemDataBase = ItemDataBase.Instance;
        equipmentItemDataBase = EquipmentItemDataBase.Instance;
    }

    [SerializeField]
    private GameObject shopListContent;
    [SerializeField]
    private GameObject shopNode;
    [SerializeField]
    private GameObject shopItemInfo;

    // shopItemInfo
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private Text itemName;
    [SerializeField]
    private Text itemPrice;
    [SerializeField]
    private Text itemDesc;
    [SerializeField]
    private GameObject buyButton;

    [SerializeField]
    private ToggleGroup toggleGroup;


    public void updateShopList()
    {
        if (itemDataBase == null)
            itemDataBase = ItemDataBase.Instance;
        if (equipmentItemDataBase == null)
            equipmentItemDataBase = EquipmentItemDataBase.Instance;

        foreach (Transform n in shopListContent.transform)
            GameObject.Destroy(n.gameObject);

        Toggle tgl = toggleGroup.ActiveToggles().FirstOrDefault();

        if (tgl != null)
            foreach (ShopItemStatus item in shopData.item)
            {
                ItemStatus itemStatus = itemDataBase.getItemStatus(item.ItemID);

                switch (tgl.name)
                {
                    case "Home":
                        createShopNode(itemStatus);
                        break;
                    case "Consumable":
                        if (itemStatus.itemType == ItemType.Consumable)
                            createShopNode(itemStatus);
                        break;
                    case "Equipment":
                        if (itemStatus.itemType == ItemType.Equipment)
                            createShopNode(itemStatus);
                        break;
                    case "Craft":
                        if (itemStatus.itemType == ItemType.Craft)
                            createShopNode(itemStatus);

                        break;
                }

            }
    }

    [SerializeField]
    private GameObject kouka;
    [SerializeField]
    private GameObject itemkouka;
    [SerializeField]
    private GameObject attack;
    [SerializeField]
    private GameObject def;
    [SerializeField]
    private GameObject equipmentStatus;


    private void createShopNode(ItemStatus itemStatus)
    {
        GameObject obj = Instantiate(shopNode);
        obj.transform.SetParent(shopListContent.transform, false);
        obj.transform.Find("itemImage").GetComponent<Image>().sprite = itemStatus.itemSprite;


        if (itemStatus.itemType == ItemType.Consumable)
        {
            int price = itemStatus.price * 10;
            obj.transform.Find("itemName").GetComponent<Text>().text = itemStatus.itemName + "\t× 10";
            obj.transform.Find("itemPrice").GetComponent<Text>().text = price.ToString();
        }
        else
        {
            obj.transform.Find("itemName").GetComponent<Text>().text = itemStatus.itemName;
            obj.transform.Find("itemPrice").GetComponent<Text>().text = itemStatus.price.ToString();
        }

        obj.transform.localScale = new Vector3(1f, 1f, 1f);
        obj.name = itemStatus.itemName + "," + itemStatus.itemID;

        obj.GetComponent<Button>().onClick.AddListener(() =>
        {
            kouka.SetActive(false);
            itemkouka.SetActive(false);
            attack.SetActive(false);
            def.SetActive(false);
            equipmentStatus.SetActive(false);

            switch (itemStatus.itemType)
            {
                case ItemType.Consumable:
                    kouka.SetActive(true);
                    itemkouka.SetActive(true);
                    itemkouka.GetComponent<Text>().text = itemDataBase.getConsumableItemStatus(itemStatus.itemID).desc;
                    break;
                case ItemType.Equipment:
                    EquipmentItemStatus eq = equipmentItemDataBase.getEqStatus(itemStatus.itemID);
                    switch (eq.equipmentType)
                    {
                        case EquipmentType.Weapon:
                            attack.SetActive(true);
                            equipmentStatus.SetActive(true);
                            equipmentStatus.GetComponent<Text>().text = eq.str + "\n" + eq.attackSpd;
                            break;
                        case EquipmentType.Armor:
                            def.SetActive(true);
                            equipmentStatus.SetActive(true);
                            equipmentStatus.GetComponent<Text>().text = eq.def + "";
                            break;
                        case EquipmentType.Accessory:
                            break;
                    }
                    break;
            }
            itemImage.sprite = itemStatus.itemSprite;
            itemDesc.text = itemStatus.itemDesc;
            if (itemStatus.itemType == ItemType.Consumable)
            {
                itemName.text = itemStatus.itemName + "\t× 10";
                int price = itemStatus.price * 10;
                itemPrice.text = price.ToString();
            }
            else
            {
                itemName.text = itemStatus.itemName;
                itemPrice.text = itemStatus.price.ToString();
            }
            buyButton.GetComponent<BuyButton>().setItem(itemStatus);
            shopItemInfo.SetActive(true);
        });
    }



    public void setShopData(ShopData s)
    {
        shopData = s;
    }


}
