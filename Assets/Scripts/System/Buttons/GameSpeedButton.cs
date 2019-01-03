using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameSpeedButton : MonoBehaviour
{

    private GameSpeed state;

    [SerializeField]
    private Image image;
    [SerializeField]
    private Sprite normalSprite;
    [SerializeField]
    private Sprite hayaiSprite;
    [SerializeField]
    private Sprite metyaSprite;


    void Start()
    {
        state = GameSpeed.Normal;
        image.sprite = normalSprite;
    }

    public void OnButtonClick()
    {
        switch (state)
        {
            case GameSpeed.Normal:
                state = GameSpeed.Hayai;
                image.sprite = hayaiSprite;
                Time.timeScale = 2f;
                break;
            case GameSpeed.Hayai:
                state = GameSpeed.Metya;
                image.sprite = metyaSprite;
                Time.timeScale = 3f;
                break;
            case GameSpeed.Metya:
                state = GameSpeed.Normal;
                image.sprite = normalSprite;
                Time.timeScale = 1f;
                break;
        }
    }

    void Update()
    {
        switch (state)
        {
            case GameSpeed.Normal:
                image.sprite = normalSprite;
                break;
            case GameSpeed.Hayai:
                image.sprite = hayaiSprite;
                break;
            case GameSpeed.Metya:
                image.sprite = metyaSprite;
                break;
        }
    }
}

public enum GameSpeed
{
    Normal,
    Hayai,
    Metya
}
