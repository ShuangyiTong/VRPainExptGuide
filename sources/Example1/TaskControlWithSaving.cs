#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class TaskControlWithSaving : MonoBehaviour
{
    public int totalTrials = 100;

    public GameObject rewardPopUp;

    private TextMeshPro rewardText;
    private bool inTrial = false;
    private float lastRewardPopUp = 0;
    private ButtonPressing button;
    private int trialCount = 0;
    private int action = 0;
    private float outcome = 0;
    private float reactiontime = 0;

    List<KeyFrame> keyFrames = new List<KeyFrame>(10000);
    // https://stackoverflow.com/questions/62598952/exporting-in-game-data-to-csv-in-unity

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
        if (inTrial){
            reactiontime += Time.deltaTime;
        }
    }

    public void ObjectSelected(string name)
    {
        if (inTrial)
        {
            if (name == "book.001")
            {
                rewardText.SetText("+1 point");
                action=1;
                outcome=1f;
            }
            else
            {
                rewardText.SetText("+0.5 point");
                action=2;
                outcome=0.5f;
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
            keyFrames.Add(new KeyFrame (trialCount, action, reactiontime, outcome));
            ToCSV();
            SaveToFile();
            reactiontime=0;
        }
    }

    public void StartButtonPressed()
    {
        inTrial = true;
    }

    public string ToCSV()
    {
        var sb = new StringBuilder("Trial,Action,ReactionTime,Outcome");
        foreach(var frame in keyFrames)
        {
            sb.Append('\n').Append(frame.Trial.ToString()).Append(',').Append(frame.Action.ToString()).Append(',').Append(frame.ReactionTime.ToString()).Append(',').Append(frame.Outcome.ToString());
        }

        return sb.ToString();
    }


    public void SaveToFile()
    {
        // Use the CSV generation from before
        var content = ToCSV();

        // The target file path e.g.
        #if UNITY_EDITOR
            var folder = Application.streamingAssetsPath;

            if(! Directory.Exists(folder)) Directory.CreateDirectory(folder);
        #else
            // var folder = Application.persistentDataPath;
            var folder = @"C:\Projects\UnityProjects\Bandit-Task-Trial\SAVED_DATA_FROM_BUILDS";
            if(! Directory.Exists(folder)) Directory.CreateDirectory(folder);
        #endif

            var filePath = Path.Combine(folder, "output.csv");

            using(var writer = new StreamWriter(filePath, false))
            {
                writer.Write(content);
            }

            // Or just
            //File.WriteAllText(content);

            Debug.Log($"CSV file written to \"{filePath}\"");

        #if UNITY_EDITOR
            AssetDatabase.Refresh();
        #endif
    }

}


// Data saving ...

[Serializable]
public class KeyFrame
{
    public int Trial;
    public int Action;
    public float ReactionTime;
    public float Outcome;

    // public KeyFrame(){}

    public KeyFrame (int trial, int action, float reactiontime, float outcome)
    {
        Trial = trial;
        Action = action;
        ReactionTime = reactiontime;
        Outcome = outcome;
    }
}