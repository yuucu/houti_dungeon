using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class PauseStopButton : MonoBehaviour {

    [SerializeField]
    private GameObject pausePanel;
    private float timeScale;

    public void OnClickButtonStopPause()
    {
        pausePanel.SetActive(false);
        Time.timeScale = timeScale;
    }

    public void setTime(float i)
    {
        timeScale = i;
    }
}
