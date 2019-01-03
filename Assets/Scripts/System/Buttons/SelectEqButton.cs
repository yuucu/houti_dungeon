using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectEqButton : MonoBehaviour
{

    [SerializeField]
    private GameObject content;
    [SerializeField]
    private ToggleGroup toggle;

    [SerializeField]
    private GameObject selectPanel;
    [SerializeField]
    private UpgradeEquipmentButton upgradeButton;

    [SerializeField]
    private Image eqSprite;
    [SerializeField]
    private Text eqName;

    public void OnClick()
    {
        selectPanel.SetActive(true);
        EquipmentItemDataBase.Instance.updateEquipmentList(content, toggle);
        foreach (Transform t in content.transform)
        {
            Button b = t.GetComponent<Button>();
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() =>
            {
                // 0:name, 1:id, 2:lv, 3:value 
                string[] eq = b.name.Split(',');
                eqName.text = eq[0] + "\tLv." + eq[2];

                eqSprite.sprite = ItemDataBase.Instance.getItemStatus(int.Parse(eq[1])).itemSprite;
                upgradeButton.setEq(int.Parse(eq[1]), int.Parse(eq[2]));
                selectPanel.SetActive(false);
            });
        }
    }
}
