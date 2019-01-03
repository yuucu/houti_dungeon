using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterCastleButton : MonoBehaviour
{

    public void OnClick()
    {
        GameManager.Instance.enterCastle();
    }
}
