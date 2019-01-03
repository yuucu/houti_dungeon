using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTownRightButton : MonoBehaviour
{

    [SerializeField]
    private Transform moveCamera;
    [SerializeField]
    private MoveTownLeftButton lButton;
    private Vector3 targetPos;
    private float time = 1f;
    private bool started = false;
    private float startTime;
    private Player player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    public void OnClick()
    {
        if (!lButton.isMoving())
        {
            targetPos = new Vector3(moveCamera.position.x + 6f, moveCamera.position.y, moveCamera.position.z);
            started = true;
            startTime = Time.timeSinceLevelLoad;
            player.setTargetPos(moveCamera.position.x + 6f - 1f, true);
        }
    }

    void Update()
    {
        if (started)
        {
            float diff = Time.timeSinceLevelLoad - startTime;
            if (diff > time || moveCamera.position == targetPos)
            {
                moveCamera.position = targetPos;
                started = false;
            }
            moveCamera.position = Vector3.Lerp(moveCamera.position, targetPos, diff / (time * 2));
        }

    }

    public bool isMoving()
    {
        return started;
    }
}
