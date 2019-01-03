using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    void Start()
    {
        switch (GameManager.Instance.getGameMode())
        {
            case GameManager.GameMode.Town1:
                transform.position = new Vector3(94, 100, -10);
                break;
            case GameManager.GameMode.Town2:
                transform.position = new Vector3(100, 100, -10);
                break;
            case GameManager.GameMode.Stage:
                transform.position = new Vector3(100, 100, -10);
                break;
        }
    }
}
