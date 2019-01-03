using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour {

    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private PauseStopButton pauseStopButton;
    [SerializeField]
    private BackTownButton backTownButton;

    public void OnClickButtonPause()
    {
        backTownButton.setTime(Time.timeScale);
        pauseStopButton.setTime(Time.timeScale);
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }
}
