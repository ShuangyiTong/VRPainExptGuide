using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonPressing : MonoBehaviour
{
    public bool buttonPressed = false;
    public float verticalMovingDistance = 0.015f;
    public float movingSpeed = 0.003f;
    public bool disableButton = false;
    private float originVerticalHeight;
    private TaskControl taskControl;

    // Start is called before the first frame update
    void Start()
    {
        originVerticalHeight = gameObject.transform.position.y;
        taskControl = GameObject.Find("TaskControl").GetComponent<TaskControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!disableButton)
        {
            buttonPressed = true;
            transform.GetChild(0).GetComponent<TextMeshPro>().SetText("Pick one book");
            taskControl.StartButtonPressed();
        }
    }

    private void FixedUpdate()
    {
        if (buttonPressed)
        {
            if (originVerticalHeight - gameObject.transform.position.y < verticalMovingDistance)
            {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - movingSpeed, gameObject.transform.position.z);
            }
        }
    }

    public void ResetButtonState()
    {
        buttonPressed = false;
        transform.GetChild(0).GetComponent<TextMeshPro>().SetText("Touch to continue");
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, originVerticalHeight, gameObject.transform.position.z);
    }
}
