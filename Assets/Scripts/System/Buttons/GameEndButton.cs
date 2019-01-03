using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndButton : MonoBehaviour {

    private Button button;
    public void Start()
    {
        button = this.GetComponent<Button>();
        button.interactable = false;
    }
    public void OnClickButton()
    {
        GameManager.Instance.save();
        Application.Quit();
    }
}
