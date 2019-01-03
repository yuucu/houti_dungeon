using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUseButton : MonoBehaviour
{
    private EquipmentItemDataBase equipmentDB;
    private UserEquipmentItemData data;

    private bool putOn = true;

    public void set(UserEquipmentItemData data, bool isEq)
    {
        this.data = data;
        if (!isEq)
        {
            transform.Find("Text").GetComponent<Text>().text = "装備する";
            putOn = true;
        }
        else
        {
            transform.Find("Text").GetComponent<Text>().text = "はずす";
            putOn = false;

        }
    }

    public void OnClick()
    {
        if (equipmentDB == null)
            equipmentDB = EquipmentItemDataBase.Instance;

        if (putOn)
            equipmentDB.setEquipment(data);
        else
            equipmentDB.takeOffEquipment(data);
    }


    [SerializeField]
    private GameObject sellPanel;
    [SerializeField]
    private Button sellButton;
    [SerializeField]
    private Text priceText;

    public void OnClickSell()
    {
        priceText.text = ItemDataBase.Instance.getPrice(data.itemID, 1) + " G で売りますか？";
        sellPanel.SetActive(true);
        sellButton.onClick.RemoveAllListeners();
        sellButton.onClick.AddListener(() =>
        {
            EquipmentItemDataBase.Instance.sell(data);
        });
    }

}
