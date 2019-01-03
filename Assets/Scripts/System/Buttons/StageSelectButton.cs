using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectButton : MonoBehaviour {

    [SerializeField]
    private GameObject stageSelectPanel;
    [SerializeField]
    private StageListManager stageListManager;

    void Start()
    {
        stageListManager = stageSelectPanel.GetComponent<StageListManager>();
    }

    public void OnClick()
    {
        stageListManager.updateStageList();
        stageSelectPanel.SetActive(true);
    }
}
