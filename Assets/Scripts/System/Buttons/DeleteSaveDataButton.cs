using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteSaveDataButton : MonoBehaviour
{
    private Button button;
    public void Start()
    {
        button = this.GetComponent<Button>();
        // button.interactable = false;
    }

    public void OnClickButton()
    {
        Debug.Log("Delete SaveData");
        SaveData.Clear();
        SaveData.Save();
        Application.Quit();
    }
}
