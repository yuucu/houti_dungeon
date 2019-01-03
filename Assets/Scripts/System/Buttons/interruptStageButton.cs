using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interruptStageButton : MonoBehaviour {

    public void OnClick()
    {
        StageDataBase.Instance.setRestartStage();
    }
}
