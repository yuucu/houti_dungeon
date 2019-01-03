using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;

public class ItemDataBase : SingletonMono<ItemDataBase>
{
    private ItemData itemData;
    private ConsumableItem consumableItem;
    private Player player;
    private EquipmentItemDataBase equipmentItemDataBase;

    [SerializeField]
    private List<UserItemData> userItemList;

    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        itemData = Resources.Load<ItemData>("Data/itemData");
        consumableItem = Resources.Load<ConsumableItem>("Data/consumableItem");
        equipmentItemDataBase = EquipmentItemDataBase.Instance;
        DebugText.push("ItemDataBase Awake!");
        userItemList = new List<UserItemData>();
    }


    [SerializeField]
    private GameObject itemListContent;
    [SerializeField]
    private GameObject itemNode;
    [SerializeField]
    private GameObject itemInfoPanel;

    [SerializeField]
    private ToggleGroup toggleGroup;

    [SerializeField]
    private ShortcutSelectButton shortCutButton;

    // - - - - - - - アイテム・装備インベントリの更新 - - - - - - - - -
    public void updateItemList()
    {
        shortCutButton.valueUpdate();

        foreach (Transform n in itemListContent.transform)
            GameObject.Destroy(n.gameObject);

        Toggle tgl = toggleGroup.ActiveToggles().FirstOrDefault();
        if (tgl != null)
            foreach (UserItemData item in userItemList)
            {
                if (item.itemValue > 0 && item.itemKey != 0)
                {
                    ItemStatus itemStatus = getItemStatus(item.itemKey);
                    switch (tgl.name)
                    {
                        case "Home":
                            createItemNode(itemStatus, item, itemListContent);
                            break;
                        case "Consumable":
                            if (itemStatus.itemType == ItemType.Consumable)
                                createItemNode(itemStatus, item, itemListContent);
                            break;
                        case "Collection":
                            if (itemStatus.itemType == ItemType.Collection)
                                createItemNode(itemStatus, item, itemListContent);
                            break;
                        case "Craft":
                            if (itemStatus.itemType == ItemType.Craft)
                                createItemNode(itemStatus, item, itemListContent);
                            break;
                    }

                }
            }
        else
            foreach (UserItemData item in userItemList)
            {
                if (item.itemValue > 0 && item.itemKey != 0)
                {
                    ItemStatus itemStatus = getItemStatus(item.itemKey);
                    createItemNode(itemStatus, item, itemListContent);
                }
            }

    }

    private void createItemNode(ItemStatus itemStatus, UserItemData item, GameObject content)
    {
        GameObject obj = Instantiate(itemNode);
        obj.transform.SetParent(content.transform, false);
        obj.transform.Find("itemName").GetComponent<Text>().text = itemStatus.itemName;
        obj.transform.Find("itemImage").GetComponent<Image>().sprite = itemStatus.itemSprite;
        obj.transform.Find("itemCount").GetComponent<Text>().text = "× " + item.itemValue.ToString();
        obj.transform.localScale = new Vector3(1f, 1f, 1f);
        obj.name = itemStatus.itemName + "," + itemStatus.itemID + "," + item.itemValue;

        obj.GetComponent<Button>().onClick.AddListener(() =>
        {
            itemInfoPanel.SetActive(true);
            itemInfoPanel.GetComponent<ItemInfoPanel>().set(itemStatus);
        });
    }

    // kaziyaで使います
    public void createSelectItemNode(GameObject content)
    {
        foreach (UserItemData item in userItemList)
        {
            if (item.itemValue > 0)
            {
                ItemStatus itemStatus = getItemStatus(item.itemKey);
                if (itemStatus.itemType == ItemType.Craft)
                {
                    createItemNode(itemStatus, item, content);
                }
            }
        }
    }

    // shortcutで使います
    public void createSelectShortcutNode(GameObject content)
    {
        foreach (UserItemData item in userItemList)
        {
            if (item.itemValue > 0)
            {
                ItemStatus itemStatus = getItemStatus(item.itemKey);
                if (itemStatus.itemType == ItemType.Consumable)
                {
                    createItemNode(itemStatus, item, content);
                }
            }
        }
    }


    // - - - - - -Save(), Load() - - - - - - - - - - -
    public void save()
    {
        DebugText.push("itemDataBase saving...");
        SaveData.SetList<UserItemData>("items", userItemList);
        shortCutButton.save();
    }

    public void load()
    {
        DebugText.push("itemDataBase loading...");

        userItemList.Clear();
        userItemList = SaveData.GetList<UserItemData>("items", new List<UserItemData>());
        updateItemList();

        shortCutButton.load();
    }

    // 初回起動時のみ
    public void createSaveData()
    {
        DebugText.push("create ItemDatabase Data");
        List<UserItemData> newUserItemData = new List<UserItemData>();
        UserItemData coin = new UserItemData();
        coin.itemKey = 0;
        coin.itemValue = 500;
        coin.isNew = true;
        newUserItemData.Add(coin);

        SaveData.SetList("items", newUserItemData);
    }
    // - - - - - - - -色々 - - - - - - - -


    public void add(int id, int num)
    {
        if (getItemStatus(id).itemType == ItemType.Equipment)
            equipmentItemDataBase.add(id, 1);
        else
        {
            bool alreadyGet = false;
            foreach (UserItemData item in userItemList)
            {
                if (item.itemKey == id)
                {
                    item.itemValue += num;
                    alreadyGet = true;
                }
            }

            if (!alreadyGet)
            {
                UserItemData newItem = new UserItemData();
                newItem.itemKey = id;
                newItem.itemValue = num;
                newItem.isNew = true;
                userItemList.Add(newItem);
            }
            updateItemList();
        }
    }

    public bool use(int id, int count)
    {
        foreach (UserItemData item in userItemList)
            if (item.itemKey == id)
            {
                if (item.itemValue >= count)
                {
                    item.itemValue -= count;
                    updateItemList();

                    itemEffect(id);
                    if (id != 0)
                        SystemText.Instance.updateLogText(getItemStatus(id).itemName + "を使った");
                    return true;
                }

            }
        return false;
    }


    public void sell(int id)
    {
        foreach (UserItemData item in userItemList)
        {
            if (item.itemKey == id)
            {
                int sellPrice = getPrice(id, item.itemValue);
                add(0, sellPrice);
                SystemText.Instance.updateLogText(sellPrice + "G 手に入れた");

                item.itemValue = 0;
                updateItemList();
            }
        }
    }

    public int getPrice(int id, int num)
    {
        foreach (ItemStatus item in itemData.item)
            if (item.itemID == id)
                return item.price * num / 2;

        return 0;
    }

    public void itemEffect(int id)
    {
        foreach (ConsumableItemStatus cItem in consumableItem.item)
            if (cItem.itemID == id)
            {
                switch (cItem.type)
                {
                    case ConsumableType.Attack:
                        break;
                    case ConsumableType.Life:
                        player.heal(cItem.healPoint);
                        break;
                    case ConsumableType.Buf:
                        break;
                }
            }
    }




    public ItemStatus getItemStatus(int id)
    {
        foreach (ItemStatus itemStatus in itemData.item)
            if (itemStatus.itemID == id)
                return itemStatus;

        return null;
    }

    public ConsumableItemStatus getConsumableItemStatus(int id)
    {
        foreach (ConsumableItemStatus itemStatus in consumableItem.item)
            if (itemStatus.itemID == id)
                return itemStatus;

        return null;
    }

    public int getItemValue(int id)
    {
        try
        {
            foreach (UserItemData item in userItemList)
                if (item.itemKey == id)
                    return item.itemValue;
        }
        catch (NullReferenceException a)
        {
            DebugText.push(a.Message);
        }
        return 0;
    }

    public void setPlayer(Player player)
    {
        this.player = player;
    }
}

[Serializable]
public class UserItemData
{
    public int itemKey;
    public int itemValue;
    public bool isNew;

}