using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenKaziyaInfoPanelButton : MonoBehaviour
{

    [SerializeField]
    private Image eqImage;
    [SerializeField]
    private Text eqText;
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private Text itemText;
    [SerializeField]
    private Text price;
    [SerializeField]
    private Text num;
    [SerializeField]
    private Text num2;
    [SerializeField]
    private Button numSelectButton;

    [SerializeField]
    private Sprite defSprite;

    [SerializeField]
    private UpgradeEquipmentButton button;

    public void OnClick()
    {
        eqImage.sprite = defSprite;
        eqText.text = "選択する";
        itemImage.sprite = defSprite;
        itemText.text = "選択する";
        price.text = "-\tG";
        button.reset();
        num.text = "1";
        num2.text = "1";
        numSelectButton.interactable = false;
    }
}
