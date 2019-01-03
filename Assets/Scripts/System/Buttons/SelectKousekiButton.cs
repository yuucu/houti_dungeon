using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectKousekiButton : MonoBehaviour
{

    [SerializeField]
    private GameObject content;

    [SerializeField]
    private GameObject selectPanel;
    [SerializeField]
    private UpgradeEquipmentButton upgradeButton;
    [SerializeField]
    private Button numOpenButton;
    [SerializeField]
    private SelectNumButton selectNumButton;

    [SerializeField]
    private Image itemSprite;
    [SerializeField]
    private Text itemName;

    public void OnClick()
    {
        foreach (Transform n in content.transform)
            GameObject.Destroy(n.gameObject);

        selectPanel.SetActive(true);
        ItemDataBase.Instance.createSelectItemNode(content);
        foreach (Transform t in content.transform)
        {
            Button b = t.GetComponent<Button>();
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() =>
            {
                // 0:name, 1:id, 2:value 
                string[] item = b.name.Split(',');
                itemName.text = item[0];
                selectNumButton.setMaxNum(int.Parse(item[2]));
                numOpenButton.interactable = true;

                itemSprite.sprite = ItemDataBase.Instance.getItemStatus(int.Parse(item[1])).itemSprite;
                upgradeButton.setItem(int.Parse(item[1]));
                selectPanel.SetActive(false);
            });
        }
    }
}
