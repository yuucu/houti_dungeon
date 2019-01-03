using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageListManager : MonoBehaviour
{
    [SerializeField]
    private GameObject stageListContent;
    [SerializeField]
    private GameObject stageNode;
    [SerializeField]
    private GameObject interruptPanel;
    [SerializeField]
    private GameObject stageInfoPanel;

    [SerializeField]
    private GameObject interruptStageInfoPanel;

    private StageData[] stageData;

    void Awake()
    {
        stageData = Resources.LoadAll<StageData>("Data/StageData/");
    }

    public void updateStageList()
    {

        stageInfoPanel.SetActive(false);
        interruptPanel.SetActive(false);

        if (stageData == null)
            stageData = Resources.LoadAll<StageData>("Data/StageData/");

        // 初期化
        foreach (Transform t in stageListContent.transform)
            GameObject.Destroy(t.gameObject);

        foreach (StageData stage in stageData)
        {
            UserStageData userStageData = null;
            foreach (UserStageData uStage in StageDataBase.Instance.getUserStageData())
            {
                if (uStage.stageID == stage.stageID)
                    userStageData = uStage;
            }

            GameObject obj = Instantiate(stageNode);
            obj.transform.SetParent(stageListContent.transform, false);
            obj.transform.Find("stageName").GetComponent<Text>().text = stage.stageName;
            obj.transform.Find("stageImage").GetComponent<Image>().sprite = stage.stageSprite;
            obj.transform.Find("stageDesc").GetComponent<Text>().text = stage.desc;
            if (userStageData != null)
                obj.transform.Find("stageClearFlag").gameObject.SetActive(userStageData.completeStage);
            else
                obj.transform.Find("stageClearFlag").gameObject.SetActive(false);


            obj.transform.localScale = new Vector3(1f, 1f, 1f);
            obj.name = stage.name;

            obj.GetComponent<Button>().onClick.AddListener(() =>
            {
                // set Stage Info Panel
                stageInfoPanel.SetActive(true);
                stageInfoPanel.transform.Find("headPanel/stageSelectInfoPanel/stageImage").gameObject.GetComponent<Image>().sprite = stage.stageSprite;
                stageInfoPanel.transform.Find("headPanel/stageSelectInfoPanel/stageName").gameObject.GetComponent<Text>().text = stage.stageName;
                stageInfoPanel.transform.Find("headPanel/stageSelectInfoPanel/stageDesc").gameObject.GetComponent<Text>().text = stage.desc;

                int stageTurn = stage.turn.Count * 10;
                int maxTurn = 0;
                if (userStageData != null)
                    maxTurn = userStageData.maxReachTurn;
                else
                    maxTurn = 0;

                stageInfoPanel.transform.Find("headPanel/Text").gameObject.GetComponent<Text>().text = "Turn数\t:\t" + stageTurn.ToString() + "\n\n" + "最大到達Turn\t:\t" + maxTurn;

                GameObject startButton = stageInfoPanel.transform.Find("footPanel/stageStartButton").gameObject;
                Button b = startButton.GetComponent<Button>();
                b.onClick.RemoveAllListeners();
                b.onClick.AddListener(() =>
                {
                    StageDataBase.Instance.setCurrentStage(stage);
                });
            });
        }

        setInterruptPanel();
    }


    public void setInterruptPanel()
    {
        UserLastPlayStageSaveData userLast = StageDataBase.Instance.getLastPlayStage();
        if (userLast != null)
        {
            if (userLast.exist)
            {
                interruptPanel.SetActive(true);
                StageData stage = StageDataBase.Instance.getStage(userLast.stageID);
                foreach (Transform child in interruptStageInfoPanel.transform)
                {
                    if (child.name == "stageName")
                        child.gameObject.GetComponent<Text>().text = stage.stageName;
                    if (child.name == "turnText")
                        child.gameObject.GetComponent<Text>().text = " Turn " + userLast.turn + " から再開します";
                    if (child.name == "stageImage")
                        child.gameObject.GetComponent<Image>().sprite = stage.stageSprite;
                }
            }
        }
    }
}
