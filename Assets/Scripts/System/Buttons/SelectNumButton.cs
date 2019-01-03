using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectNumButton : MonoBehaviour
{

    [SerializeField]
    private Text numText;
    [SerializeField]
    private Text numText2;
    [SerializeField]
    private UpgradeEquipmentButton upgradeButton;

    private int maxNum = 1;

    public void OnClickPlus()
    {

        int i = int.Parse(numText.text) + 1;
        if (i > maxNum)
        {
            i = 1;
        }
        numText.text = i + "";
    }

    public void OnClickMinus()
    {
        int i = int.Parse(numText.text) - 1;
        if (i <= 0)
        {
            i = maxNum;
        }
        numText.text = i + "";
    }

    public void OnClickOk()
    {
        int i = int.Parse(numText.text);
        numText2.text = i + "";
        upgradeButton.setPrice();
    }

    public void setMaxNum(int num)
    {
        maxNum = num;
    }
}
