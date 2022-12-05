using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class VRController : MonoBehaviour
{
    public float speed = 0.01f;

    public GameObject head = null;

    public SteamVR_Action_Single squeezeAction = SteamVR_Input.GetAction<SteamVR_Action_Single>("Squeeze");
    private CharacterController characterController = null;

    private void Awake()
    {
        characterController = GameObject.Find("Player").GetComponent<CharacterController>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        MovementHandler();
    }

    private void MovementHandler()
    {
        float forwardSpeed = squeezeAction.GetAxis(SteamVR_Input_Sources.Any);

        if (forwardSpeed > 0)
        {
            Vector3 rotation = head.transform.rotation * Vector3.forward;
            Vector2 rotationAngle = new Vector2(rotation.x, rotation.z);
            rotationAngle.Normalize();

            Vector3 realMove = new Vector3(rotationAngle.x * speed, 0, rotationAngle.y * speed);

            characterController.Move(realMove);
        }
    }

}
