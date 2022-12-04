using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskControl : MonoBehaviour
{
    public int totalTrials = 100;

    public GameObject rewardPopUp;

    private TextMeshPro rewardText;
    private bool inTrial = false;
    private float lastRewardPopUp = 0;
    private ButtonPressing button;
    private int trialCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        rewardPopUp.SetActive(false);
        rewardText = rewardPopUp.GetComponent<TextMeshPro>();

        button = GameObject.Find("Button").GetComponent<ButtonPressing>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rewardPopUp.activeSelf)
        {
            if (Time.time - lastRewardPopUp > 1)
            {
                rewardPopUp.SetActive(false);
            }
        }
    }

    public void ObjectSelected(string name)
    {
        if (inTrial)
        {
            if (name == "book.001")
            {
                rewardText.SetText("+1 point");
            }
            else
            {
                rewardText.SetText("+0.5 point");
            }
            rewardPopUp.SetActive(true);
            lastRewardPopUp = Time.time;
            inTrial = false;
            button.ResetButtonState();
            trialCount += 1;
            if (trialCount >= totalTrials)
            {
                button.disableButton = true;
            }
        }
    }

    public void StartButtonPressed()
    {
        inTrial = true;
    }


}
