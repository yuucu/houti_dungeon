using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterKaziyaButton : MonoBehaviour
{

    public void OnClick()
    {
        GameManager.Instance.enterKaziya();
    }
}
