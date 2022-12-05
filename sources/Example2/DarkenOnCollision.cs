using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class DarkenOnCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        SteamVR_Fade.View(new Color(0, 0, 0, 0.995f), 0.1f);
    }

    void OnCollisionExit(Collision collision)
    {
        SteamVR_Fade.View(new Color(0, 0, 0, 0), 0.1f);
    }
}
