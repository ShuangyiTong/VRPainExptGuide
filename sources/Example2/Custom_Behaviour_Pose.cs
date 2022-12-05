using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Valve.VR;

public class Custom_Behaviour_Pose : SteamVR_Behaviour_Pose
{
    public GameObject VRCamera;
    
    protected override void UpdateTransform()
    {
        CheckDeviceIndex();

        if (origin != null)
        {
            Vector3 trackedPosition = poseAction[inputSource].localPosition;
            Vector3 hmdTrackedPosition = VRCamera.GetComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>().trackedPosition;
            // Vector3 hmdTrackedPosition = VRCamera.GetComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>().originPose.position;
            // Vector3 hmdTrackedPosition = VRCamera.GetComponent<UnityEngine.SpatialTracking.TrackedPoseDriverDataDescription>().GetPoseData().position;
            // print(hmdTrackedPosition);

            transform.position = origin.transform.TransformPoint(trackedPosition - hmdTrackedPosition);
            transform.rotation = origin.rotation * poseAction[inputSource].localRotation;
        }
        else
        {
            transform.localPosition = poseAction[inputSource].localPosition;
            transform.localRotation = poseAction[inputSource].localRotation;
        }
    }
}
