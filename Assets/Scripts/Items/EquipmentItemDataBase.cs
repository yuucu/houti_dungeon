using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EquipmentItemDataBase : SingletonMono<EquipmentItemDataBase>
{
    private ItemDataBase itemDataBase;
    private EquipmentItem equipmentItemData;

    private Player player;

    private Sprite weaponImage;
    private Sprite armorImage;
    private Sprite acceImage;

    [SerializeField]
    private List<UserEquipmentItemData> userEquipmentList;

    [SerializeField]
    private List<UserEquipmentItemData> userIsWearing;

    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        equipmentItemData = Resources.Load<EquipmentItem>("Data/equipmentItem");
        weaponImage = Resources.Load<Sprite>("Sprites/UI/weapon");
        armorImage = Resources.Load<Sprite>("Sprites/UI/armor");
        acceImage = Resources.Load<Sprite>("Sprites/UI/acce");

        userEquipmentList = new List<UserEquipmentItemData>();
        userIsWearing = new List<UserEquipmentItemData>();
    }

    void Start()
    {
        itemDataBase = ItemDataBase.Instance;
    }


    [SerializeField]
    private GameObject equipmentListContent;
    [SerializeField]
    private GameObject equipmentNode;
    [SerializeField]
    private GameObject equipmentInfoPanel;

    [SerializeField]
    private ToggleGroup toggleGroup;

    // - - - - - - - アイテム・装備インベントリの更新 - - - - - - - - -
    public void updateEquipmentList(GameObject content, ToggleGroup toggle)
    {
        if (itemDataBase == null)
            itemDataBase = ItemDataBase.Instance;

        foreach (Transform n in content.transform)
            GameObject.Destroy(n.gameObject);

        Toggle tgl = toggle.ActiveToggles().FirstOrDefault();
        if (tgl != null)
            foreach (UserEquipmentItemData equipment in userEquipmentList)
            {
                if (equipment.value > 0)
                {
                    ItemStatus itemStatus = itemDataBase.getItemStatus(equipment.itemID);
                    EquipmentItemStatus equipmentStatus = getEquipmentStatus(equipment.itemID);
                    switch (tgl.name)
                    {
                        case "Home":
                            createEquipmentNode(itemStatus, equipment, equipmentStatus, content);
                            break;
                        case "Weapon":
                            if (equipmentStatus.equipmentType == EquipmentType.Weapon)
                                createEquipmentNode(itemStatus, equipment, equipmentStatus, content);
                            break;
                        case "Armor":
                            if (equipmentStatus.equipmentType == EquipmentType.Armor)
                                createEquipmentNode(itemStatus, equipment, equipmentStatus, content);
                            break;
                        case "Accessory":
                            if (equipmentStatus.equipmentType == EquipmentType.Accessory)
                                createEquipmentNode(itemStatus, equipment, equipmentStatus, content);
                            break;
                    }
                }
            }
        else
        {
            foreach (UserEquipmentItemData equipment in userEquipmentList)
                if (equipment.value > 0)
                {
                    ItemStatus itemStatus = itemDataBase.getItemStatus(equipment.itemID);
                    EquipmentItemStatus equipmentStatus = getEquipmentStatus(equipment.itemID);
                    createEquipmentNode(itemStatus, equipment, equipmentStatus, content);
                }
        }
    }

    public void OnClick()
    {
        updateEquipmentList(equipmentListContent, toggleGroup);
    }

    private void createEquipmentNode(ItemStatus itemStatus, UserEquipmentItemData userItemData, EquipmentItemStatus equipmentItemStatus, GameObject content)
    {
        GameObject obj = Instantiate(equipmentNode);
        obj.transform.SetParent(content.transform, false);
        obj.transform.Find("equipmentName").GetComponent<Text>().text = itemStatus.itemName + "\tLv." + userItemData.lv;
        obj.transform.Find("equipmentImage").GetComponent<Image>().sprite = itemStatus.itemSprite;
        obj.transform.Find("equipmentCount").GetComponent<Text>().text = "× " + userItemData.value;

        GameObject isEq = obj.transform.Find("isEq").gameObject;
        isEq.SetActive(false);
        foreach (UserEquipmentItemData uEQ in userIsWearing)
        {
            if (uEQ.itemID == userItemData.itemID && uEQ.lv == userItemData.lv)
            {
                switch (uEQ.equipmentType)
                {
                    case EquipmentType.Weapon:
                        isEq.GetComponent<Image>().sprite = weaponImage;
                        break;
                    case EquipmentType.Armor:
                        isEq.GetComponent<Image>().sprite = armorImage;
                        break;
                    case EquipmentType.Accessory:
                        isEq.GetComponent<Image>().sprite = acceImage;
                        break;
                }
                isEq.SetActive(true);
            }
        }

        obj.transform.localScale = new Vector3(1f, 1f, 1f);
        obj.name = itemStatus.itemName + "," + itemStatus.itemID + "," + userItemData.lv + "," + userItemData.value;
        obj.GetComponent<Button>().onClick.AddListener(() =>
        {
            equipmentInfoPanel.SetActive(true);
            equipmentInfoPanel.GetComponent<EquipmentItemInfoPanel>().set(itemStatus, userItemData, isEq.activeSelf);
        });
    }


    private EquipmentItemStatus getEquipmentStatus(int id)
    {
        foreach (EquipmentItemStatus equipment in equipmentItemData.item)
            if (equipment.itemID == id)
                return equipment;

        return null;
    }

    public void add(int id, int lv)
    {

        bool alreadyGet = false;
        foreach (UserEquipmentItemData equipment in userEquipmentList)
        {
            if (equipment.itemID == id && equipment.lv == lv)
            {
                equipment.value += 1;
                alreadyGet = true;
            }
        }

        if (!alreadyGet)
        {
            UserEquipmentItemData newEq = new UserEquipmentItemData();
            EquipmentItemStatus eqStatus = getEquipmentStatus(id);
            newEq.itemID = eqStatus.itemID;

            if (eqStatus.str != 0)
                newEq.str = eqStatus.str + ((lv - 1) * 3);
            if (eqStatus.def != 0)
                newEq.def = eqStatus.def + ((lv - 1) * 3);
            newEq.attackSpd = eqStatus.attackSpd;
            newEq.equipmentType = eqStatus.equipmentType;

            if (eqStatus.hp != 0)
                newEq.hp = eqStatus.hp + ((lv - 1) * (eqStatus.hp / 2));
            newEq.moveSpd = eqStatus.moveSpd;

            newEq.lv = lv;
            newEq.value = 1;
            userEquipmentList.Add(newEq);
        }
        updateEquipmentList(equipmentListContent, toggleGroup);
    }

    public void save()
    {
        SaveData.SetList<UserEquipmentItemData>("equipments", userEquipmentList);
        SaveData.SetList<UserEquipmentItemData>("userIsWearing", userIsWearing);
    }

    public void load()
    {
        userEquipmentList.Clear();
        userEquipmentList = SaveData.GetList<UserEquipmentItemData>("equipments", new List<UserEquipmentItemData>());
        userIsWearing = SaveData.GetList<UserEquipmentItemData>("userIsWearing", new List<UserEquipmentItemData>());

        updateEquipmentList(equipmentListContent, toggleGroup);
        setPlayerStatus();
    }

    public void createSaveData()
    {
        userEquipmentList = new List<UserEquipmentItemData>();
        userIsWearing = new List<UserEquipmentItemData>();

        SaveData.SetList<UserEquipmentItemData>("equipments", userEquipmentList);
        SaveData.SetList<UserEquipmentItemData>("userIsWearing", userIsWearing);
    }


    public void setEquipment(UserEquipmentItemData equipment)
    {
        bool found = false;
        for (int i = 0; i < userIsWearing.Count; i++)
        {
            if (userIsWearing[i].equipmentType == equipment.equipmentType)
            {
                userIsWearing[i] = equipment;
                found = true;
            }
        }
        if (!found)
            userIsWearing.Add(equipment);

        updateEquipmentList(equipmentListContent, toggleGroup);
        setPlayerStatus();
    }

    public bool upgradeEq(int id, int lv, int count)
    {
        bool have = false;
        foreach (UserEquipmentItemData eq in userEquipmentList)
        {
            if (eq.itemID == id && eq.lv == lv && eq.value >= 1)
            {
                eq.value -= 1;
                takeOffEquipment(eq);
                have = true;
                SystemText.Instance.updateLogText(itemDataBase.getItemStatus(id).itemName + "を強化しました");
            }
        }
        if (have)
        {
            add(id, lv + count);
            return true;
        }
        else
            SystemText.Instance.updateLogText("所持していません");

        return false;
    }

    public void sell(UserEquipmentItemData equipment)
    {
        foreach (UserEquipmentItemData eq in userEquipmentList)
        {
            if (equipment.itemID == eq.itemID && equipment.lv == eq.lv)
            {
                int price = itemDataBase.getPrice(eq.itemID, 1);
                itemDataBase.add(0, price);
                eq.value -= 1;
                takeOffEquipment(eq);
                SystemText.Instance.updateLogText(price + "G 手に入れた");
            }
        }
    }

    public void takeOffEquipment(UserEquipmentItemData equipment)
    {
        for (int i = 0; i < userIsWearing.Count; i++)
            if (equipment.itemID == userIsWearing[i].itemID && equipment.lv == userIsWearing[i].lv)
                userIsWearing.RemoveAt(i);

        updateEquipmentList(equipmentListContent, toggleGroup);
        setPlayerStatus();
    }

    public EquipmentItemStatus getEqStatus(int id)
    {
        foreach (EquipmentItemStatus eq in equipmentItemData.item)
            if (eq.itemID == id)
                return eq;
        return null;
    }

    private void setPlayerStatus()
    {
        float eqStr = 0;
        float eqDef = 0;
        float attackSpd = 0;
        float moveSpd = 0;
        int hp = 0;
        foreach (UserEquipmentItemData eq in userIsWearing)
        {
            eqStr += eq.str;
            eqDef += eq.def;
            attackSpd += eq.attackSpd;
            moveSpd += eq.moveSpd;
            hp += eq.hp;
        }
        player.setEquipment(eqStr, eqDef, attackSpd, moveSpd, hp);

    }


    public string getIsWeapon()
    {
        foreach (UserEquipmentItemData eq in userIsWearing)
            if (eq.equipmentType == EquipmentType.Weapon)
                return itemDataBase.getItemStatus(eq.itemID).itemName + "\tLv." + eq.lv;

        return "";
    }

    public string getIsArmor()
    {
        foreach (UserEquipmentItemData eq in userIsWearing)
            if (eq.equipmentType == EquipmentType.Armor)
                return itemDataBase.getItemStatus(eq.itemID).itemName + "\tLv." + eq.lv;

        return "";
    }
    public string getIsAcce()
    {
        foreach (UserEquipmentItemData eq in userIsWearing)
            if (eq.equipmentType == EquipmentType.Accessory)
                return itemDataBase.getItemStatus(eq.itemID).itemName + "\tLv." + eq.lv;

        return "";
    }

    public void setPlayer(Player player)
    {
        this.player = player;
    }
}


[Serializable]
public class UserEquipmentItemData : EquipmentItemStatus
{
    public int lv;
    public int value;
}
