using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartPlayer : MonoBehaviour
{

    private Button button;

    public void OnClick()
    {
        GameObject.Find("Player").GetComponent<Player>().damage(99999);
    }

    public void canClick()
    {
        if (button == null)
            button = this.GetComponent<Button>();

        GameManager gm = GameManager.Instance;
        if (gm.getGameMode() == GameManager.GameMode.Town1 || gm.getGameMode() == GameManager.GameMode.Town2)
        {
            button.interactable = false;
        }
        else
            button.interactable = true;
    }

}
