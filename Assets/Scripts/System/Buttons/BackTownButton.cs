using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackTownButton : MonoBehaviour
{

    [SerializeField]
    private GameObject pausePanel;
    private Button button;
    private float timeScale = -1;

    public void Start()
    {
        button = this.GetComponent<Button>();
    }

    public void OnClickButton()
    {
        pausePanel.SetActive(false);
        Time.timeScale = timeScale;
        GameManager.Instance.townBack();
    }

    public void canClick()
    {
        if (button == null)
            button = this.GetComponent<Button>();

        GameManager gm = GameManager.Instance;
        if (gm.getGameMode() == GameManager.GameMode.Town1 || gm.getGameMode() == GameManager.GameMode.Town2)
            button.interactable = false;

        else
            button.interactable = true;
    }

    public void setTime(float i)
    {
        timeScale = i;
    }

    public void OnClickOption()
    {
        GameManager.Instance.townBack();
    }
}
