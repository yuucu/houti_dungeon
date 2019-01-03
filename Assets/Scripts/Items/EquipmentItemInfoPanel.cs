using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentItemInfoPanel : MonoBehaviour
{

    [SerializeField]
    private Image equipmentInfoImage;
    [SerializeField]
    private Text equipmentInfoName;
    [SerializeField]
    private Text equipmentInfoDesc;

    [SerializeField]
    private GameObject attackStatus;

    [SerializeField]
    private GameObject defStatus;


    [SerializeField]
    private Text equipmentStatus;

    [SerializeField]
    private GameObject equipmentUseButton;

    public void set(ItemStatus item, UserEquipmentItemData equipment, bool isEq)
    {
        attackStatus.SetActive(false);
        defStatus.SetActive(false);

        equipmentInfoImage.sprite = item.itemSprite;
        equipmentInfoName.text = item.itemName;
        equipmentInfoDesc.text = item.itemDesc;

        equipmentUseButton.GetComponent<EquipmentUseButton>().set(equipment, isEq);

        if (equipment.equipmentType == EquipmentType.Weapon)
        {
            attackStatus.SetActive(true);
            equipmentStatus.text = equipment.str + "\n" + equipment.attackSpd;
        }

        if (equipment.equipmentType == EquipmentType.Armor)
        {
            defStatus.SetActive(true);
            equipmentStatus.text = equipment.def + "";
        }
        if (equipment.equipmentType == EquipmentType.Accessory)
        {
            equipmentStatus.text = "";
        }
    }


}
