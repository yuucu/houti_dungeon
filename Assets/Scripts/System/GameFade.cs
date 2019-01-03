using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFade : MonoBehaviour
{
    private GameObject fadePanel;
    private Image fadePanelImage;

    private float time;

    private bool isFading;
    private bool fadeInFlag;
    private bool fadeOutFlag;
    private bool fadeInAndOutFlag;

    private float setTime;

    void Awake()
    {
        fadePanel = this.gameObject;
        fadePanelImage = GetComponent<Image>();
        
    }

    void Start()
    {
        time = 0;
        setTime = 1;
    }

    public void hide()
    {
        fadePanel.SetActive(false);
    }

    public void fadeIn(float t)
    {
        fadePanel.SetActive(true);
        isFading = true;
        fadeInFlag = true;
        time = 0;
        setTime = t;
    }

    public void fadeOut(float t)
    {
        fadePanel.SetActive(true);
        isFading = true;
        fadeOutFlag = true;
        time = 0;
        setTime = t;
    }

    void Update()
    {
        if (isFading)
        {
            time += Time.deltaTime;

            if (fadeInFlag)
                fadePanelImage.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, time/setTime));
            else if (fadeOutFlag)
                fadePanelImage.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, time/setTime));

            if (time >= setTime)
            {
                isFading = false;
                fadeInFlag = false;
                fadeOutFlag = false;
            }
            if (fadePanelImage.color.a == 0)
                fadePanel.SetActive(false);
        }
    }

    public bool getFading()
    {
        return isFading;
    }
}
