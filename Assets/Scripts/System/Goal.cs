using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.CompareTag("Player"))
            if (StageDataBase.Instance.getClearFlag())
            {
                gameManager.townBack();
                StageDataBase.Instance.setClearFlag(false);
            }
            else
                StartCoroutine(gameManager.startTurn());

    }
}
