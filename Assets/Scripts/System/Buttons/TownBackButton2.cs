using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownBackButton2 : MonoBehaviour
{

    public void OnClick()
    {
        if (Application.loadedLevelName == "Yadoya")
            GameManager.Instance.townBackYadoya();

        else
            GameManager.Instance.townBack2();
    }
}
