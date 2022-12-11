using System;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TaskControl : MonoBehaviour
{
    BoardUIManager UIManager;
    /* Task state is one of 
     * Idle | InTrial
     */
    string TaskState;
    int toggleUI = 1;
    float trialStartTime;

    void Start()
    {
        UIManager = GameObject.Find("Board").GetComponent<BoardUIManager>();
        //UIManager.SetBoardDisplay(true);
        UIManager.ActivateConfirmButton(true);

        TaskState = "Idle";
    }

    void GenerateItems()
    {
        GameObject goldBar = Resources.Load<GameObject>("Prefabs/i_gnot");
        var rnd = new System.Random(DateTime.Now.Millisecond);
        double tick_1 = rnd.NextDouble();
        double tick_2 = rnd.NextDouble();
        GameObject initGoldBar = Instantiate(goldBar, new Vector3((float)(5 * tick_1 - 4.0), 0.5f, (float)(5 * tick_2 - 0.5)), Quaternion.identity);
        initGoldBar.name = "goldbar";
    }

    // Update is called once per frame
    void Update()
    {
        // Universal action
        if (UIManager.userAction == "MenuButtonPressed")
        {
            toggleUI ^= 1;
            UIManager.SetBoardDisplay(Convert.ToBoolean(toggleUI));
            UIManager.userAction = "";
        }

        if (TaskState == "Idle")
        {
            if (UIManager.boardCommand == "ConfirmButtonPressed")
            {
                GenerateItems();
                UIManager.SetBoardDisplay(false);
                toggleUI = 0;
                trialStartTime = Time.time;
                UIManager.boardCommand = "";
                TaskState = "InTrial";
            }
        }
        else if (TaskState == "InTrial")
        {
            bool foundObject = false;
            Player player = Player.instance;
            if (player)
            {
                foreach (Hand hand in player.hands)
                {
                    GameObject attachedObject = hand.currentAttachedObject;
                    if (attachedObject != null)
                    {
                        foundObject = true;
                    }
                }
            }

            if (foundObject)
            {
                UIManager.SetBoardDisplay(true);
                UIManager.SetMainText("You found the object! Click to continue");
                toggleUI = 1;
                TaskState = "Idle";
                Destroy(GameObject.Find("goldbar"));
            }

            if ((Time.time - trialStartTime > 60) && !foundObject)
            {
                UIManager.SetBoardDisplay(true);
                UIManager.SetMainText("Time is up! Click to to try again");
                toggleUI = 1;
                TaskState = "Idle";
                Destroy(GameObject.Find("goldbar"));
            }
        }
    }
}