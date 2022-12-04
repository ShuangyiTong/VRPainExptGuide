using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceHandling : MonoBehaviour
{
    TaskControl taskControl;
    Material material;
    // Start is called before the first frame update
    void Start()
    {
        taskControl = GameObject.Find("TaskControl").GetComponent<TaskControl>();
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        taskControl.ObjectSelected(transform.name);
        material.SetColor("_Color", Color.yellow);
    }

    private void OnCollisionExit(Collision collision)
    {
        material.SetColor("_Color", Color.white);
    }
}
