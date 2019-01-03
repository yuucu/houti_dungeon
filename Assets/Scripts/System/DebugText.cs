using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DebugText : MonoBehaviour
{
    private static List<string> log = new List<string>();
    private static Text debugFIeld;
    private GUIStyle style;
    private Rect labelRect;

    void Start()
    {
        style = new GUIStyle();
        style.fontSize = 10;
        style.fontStyle = FontStyle.Normal;
        style.normal.textColor = Color.black;
        labelRect = new Rect(10, 50, Screen.width, Screen.height);
    }

    public static void push(string msg)
    {
        log.Add(msg);
        if (log.Count > 15)
            log.RemoveAt(0);
    }

    void OnGUI()
    {
        string outMessage = "";
        foreach (string msg in log)
            outMessage += msg + System.Environment.NewLine;

        GUI.Label(labelRect, outMessage, style);
    
    }
    
}
