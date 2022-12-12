# A guide to set up virtual reality experiments

## Table of Contents
- [A guide to set up virtual reality experiments](#a-guide-to-set-up-virtual-reality-experiments)
  - [Table of Contents](#table-of-contents)
  - [Overview](#overview)
    - [What are the new things in VR compared to sitting in front of a traditional computer screen](#what-are-the-new-things-in-vr-compared-to-sitting-in-front-of-a-traditional-computer-screen)
    - [What are the challenges/differences of a VR experiment](#what-are-the-challengesdifferences-of-a-vr-experiment)
- [Part 1: Simple Task Setup](#part-1-simple-task-setup)
  - [Preparation](#preparation)
    - [Hardware](#hardware)
    - [Software](#software)
  - [Example - Simple Bandit Task](#example---simple-bandit-task)
    - [Steps](#steps)
  - [Example - Physical foraging task in a maze](#example---physical-foraging-task-in-a-maze)
    - [Steps](#steps-1)
  - [Example - Realistic rendering environment](#example---realistic-rendering-environment)
- [Part 2 Linking other data streams](#part-2-linking-other-data-streams)
  - [Collecting and saving more data streams](#collecting-and-saving-more-data-streams)
    - [Collect data in Unity](#collect-data-in-unity)
      - [Motion tracking data](#motion-tracking-data)
      - [Eye tracking data](#eye-tracking-data)
      - [Integrating A Heart Rate (HR) Monitor into Unity VR](#integrating-a-heart-rate-hr-monitor-into-unity-vr)
    - [Collect data outside Unity](#collect-data-outside-unity)
  - [Apply stimulation feedback](#apply-stimulation-feedback)
    - [Unity inter-process communication with TCP sockets](#unity-inter-process-communication-with-tcp-sockets)
    - [Unified data streaming software](#unified-data-streaming-software)
  - [Closed-loop task control with ad-hoc scripts](#closed-loop-task-control-with-ad-hoc-scripts)

## Overview

Virtual reality (VR) is a new technology that brings new ways of presenting visual stimuli and interaction with a virtual environment, which makes it valuable for designing novel experimental paradigms. This guide is intended to help researchers with some experience setting up computer-based experiments (e.g. Psychopy, Psychtoolbox etc.) to get started with VR experiments.

### What are the new things in VR compared to sitting in front of a traditional computer screen

VR has been studied for many years. Enormous engineering effort has been put into making the virtual experience more realistic. Current VR technology advances might be categorized into two topics: vision and interaction. Vision is benefit from the fast-developing computer graphics technology, for example, faster graphics processing and algorithm render virtual environment to realistic visual stimuli, and higher pixel density makes it possible to present a high-resolution image on the screen of a wearable headset. Interaction advances are probably attributed to more robust and simpler motion-tracking technology. Interactions are done by interacting with virtual objects in 3D space. This is much more natural than using a mouse and a keyboard.

### What are the challenges/differences of a VR experiment

In terms of the actual development, as we will see shortly, VR games use the same development platform as other non-VR games. It only changes two things: the way it outputs graphics (binocular vision) and the way of interaction (motion capture and controllers). A game engine or the VR manufacturer's software package usually does most of the things for us. What more important to us is VR's realistic experience can cause some necessary changes to existing paradigms.

- VR environments and all objects within are 3D. If you want to present a 2D cue, it needs to be on some plane in 3D. You would always allow the vision rotates when people rotate their head, otherwise, it can cause dizziness. Therefore, the 3D spatial location of a cue relative to the subject is important.
- VR experiments usually run continuously. In a monitor-based task, a cue displays on a screen, and the subject presses a key. In VR, instead of pressing a key instantly, it can take a certain amount of time to complete an action. For example, reaching an object in a go/no-go task with real arm movement does not complete instantly but rather a continuous action.

One can always fall back to using a VR headset as a big screen, and all interaction is done by pressing keys. In that case, it might be easier to run a monitor-based task rather than seeking to use VR.

# Part 1: Simple Task Setup
## Preparation

### Hardware

VR headset: There are two main-stream VR headsets. One is PC-based and the headset is essentially a display. HTC Vive is the main manufacturer of PC-based VR. The other one is a standalone headset which the headset has a mobile operating system installed. Standalone headsets have generally poorer performance when running on their own, but they can be connected to PCs and acts as a display for PC VR as well. We currently use HTC Vive Pro Eye for lab experiments. It was the only headset with built-in eye-tracking functionality when we made the purchase. However, more companies are developing high-end VR headsets now. Recently released Pico 4 Enterprise and Meta Quest Pro are also equipped with eye tracking. We are keen to test their possibility to be used in the lab environment. Pico 4 Enterprise and Meta Quest Pro are both standalone headsets.

We still prefer to use PC-based VR, not only because PC is more powerful but also PC can handle other data streams from other devices better (for example, EEG data). VR applications usually require good graphics hardware to run the game. Hardware updates frequently, so here we only show our currently running lab PC's specifications.

- CPU: Intel 11900KF
- GPU: NVIDIA RTX 3090 24GB
- Memory: 64GB DDR4
- Storage: 1TB SSD

Other lab/clinical equipment that was used previously in conventional experiments is usually applicable. The biggest issue is probably whether the device is portable and has acceptable motion artifacts when used with VR.

### Software

Operating system: We would recommend using Windows 10 / 11 operating system.

Game engine: VR environment is inherently 3D. Therefore, a 3D game engine could be a great platform to develop VR applications. Game engines are productivity software with many tools to help develop games efficiently. Popular publicly accessible 3D game engines with VR integration support include [Unity](https://unity.com/), [Unreal](https://www.unrealengine.com/en-US), and [CryEngine](https://www.cryengine.com/). All three are free to use with licenses for non-commercial use. Unity is probably the easiest one to start among all three. We use **Unity 2019.4 LTS** for our current studies. It would be best to follow [Unity's official tutorials](https://learn.unity.com/) if you are new to game development.

SteamVR: [SteamVR](https://store.steampowered.com/app/250820/SteamVR/) is a widely used VR development and runtime software. SteamVR provides software development kits for developers to make VR games in Unity and other game engines. It also provides useful interaction libraries to develop VR games quickly. SteamVR is developed by Valve Corporation who owns a major video game distribution platform Steam. Therefore, if you want to obtain a copy of SteamVR, you need to download it through Steam. If you download SteamVR through Steam, whenever you launch SteamVR, Steam would launch automatically. However, if you are registered as a business user of Valve's hardware partner HTC Vive, you can obtain an offline version of SteamVR through Vive. 

SteamVR has not been updated for a while. As more players come to the VR industry, we could see a more diverse VR ecosystem.

## Example - Simple Bandit Task
The first example is a simple bandit task where participants are asked to pick one book on the desk. Different books give different rewards.
### Steps

1. **Assets import**: From asset store import [SteamVR](https://assetstore.unity.com/packages/tools/integration/steamvr-plugin-32647), [pixel modern office extras](https://assetstore.unity.com/packages/3d/environments/urban/pixel-modern-office-extras-225670).
2. **Level design**:
   - Create a floor by creating a new plane. You can attach certain textures (e.g. wood) to the plane.
   - Place objects (chairs, desks) from the assets we just imported on the floor.
   - Place player prefab from `SteamVR/InteractionSystem/Core/Prefabs`
3. **UI design**: 
   - Create a trial start button with a cube and a TextMeshPro object (or UI text) and place them on the desk.
   - Create a scoreboard display in front of the desk using TextMeshPro object (or UI text) object.
![Task setup](imgs/BanditExampleLevelDesign.PNG)
Figure 1: Unity editor screenshot of the bandit task
4. **Task execution**: Create a new game object called TaskControl, and add [task control script (complete version)](sources/Example1/TaskControl.cs) to the game object as a component. Using a separate script to control the global task progress is a neat way to write the task execution logic. This task is simple. The participant touches the start button, then the options (books) are now available to be chosen. Once the participant picks a choice (by touching the book), reward points is displayed in front of them. The start button is now available to be pressed again. We do this step by step.

   - Everything starts with the participant pressing the button. We use colliders to do this (so in fact it is touching the button rather than pressing the button). So we create a [button pressing script (complete version)](sources/Example1/ButtonPressing.cs) and attach it to the button object so that when the button is touched by hand, `OnCollisionEnter` will be called (if the button has a collider. If not, simply add a box collider to the button game object).
      ```C#
      private void OnCollisionEnter(Collision collision)
        {
            if (!disableButton)
            {
                buttonPressed = true;
                transform.GetChild(0).GetComponent<TextMeshPro>().SetText("Pick one book");
                taskControl.StartButtonPressed();
            }
        }
      ```
      This function does three things: set the button state to pressed which is a member variable that we should define it earlier (see [the complete version](sources/Example1/ButtonPressing.cs)). We also then change the instruction to "Pick one book" shown on the button. Note here we make the text object a child object of the button so that we can find it by calling `transform.GetChild(0)`. There is no other child, so the first one with index 0 is the text object. Lastly, we need to tell the task control script that this button is pressed by calling the public member function `StartButtonPressed` of `TaskControl`.

      We then would like to have some animation effects for the button. When the button is pressed, we would like to have it gradually lowered down to create a 'pressed' feeling. The distance and speed require manual tweaking. It is called in `FixedUpdate` so that it is updated at a constant rate. If you want to rather update it in `Update`, where two calls are called in a variable time interval, you need to calculate the moving distance using time passed since the last frame to ensure the button moves at deterministic speed.
      ```C#
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
      ```
      The `TaskControl` component then knows the button is now pressed through the function called `StartButtonPressed`, it simply sets a flag indicating it is now in a new trial and it will then handle the book choice correctly.

   - Next, we need another script for each choice (book): [choice handling (complete version)](sources/Example1/ChoiceHandling.cs). It does two things: a. tell `TaskControl` if the hand has collided with a book and which book is in collision with, which means if the participant made a choice, b. highlight the book by changing the material when the hand is in collision with the book. This is done by the following code
      ```C#
      private void OnCollisionEnter(Collision collision)
      {
          taskControl.ObjectSelected(transform.name);
          material.SetColor("_Color", Color.yellow);
      }

      private void OnCollisionExit(Collision collision)
      {
          material.SetColor("_Color", Color.white);
      }
      ```
      Let us go back to `TaskControl.ObjectSelected`. If it is in Trial (after the button is pressed), the reward points are set depending on the choice. Here two choices are associated with deterministically reward, so it is a boring bandit task. If the total trial count `trialCount` exceeds the maximum number of trials `totalTrials`, we will disable the button and the participant won't be able to start a new trial. 
      ```C#
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
      ```
      The reward points pop-up is only shown temporarily. We record the time it is set active, then turn it off after 1 second in the `Update` function.
      ```C#
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
      ```
   This completes the whole simple experiment design. Of course, we still need to save the behavioural data and possibly connecting and synchronising with other devices like EEG, which will be covered in part 2. Unfortunately due to [Unity Asset Store EULA](https://unity.com/legal/as-terms), we may not distribute the full project containing Asset Store materials, but you can download a build version here: [Example1_Build](Build/Example1-Build.7z).

5. **(Optional) Saving user data**: In order to save user data like choices and reaction times for each trial, we need to use serialization in C#, which allows us to save object into a stream of bytes to store or transmit into memory. This is necesaary as we don't have a handy library equivalent to Pandas in Python which we can use to store or manipulate csv data. As you'll see in tomorrow's tutorial, the same concept is used in the control panel used for communicating with other devices. We will modify the `TaskControl` script to include the functionality of saving data, modified script can be found here: [TaskControlWithSaving.cs](sources/Example1/TaskControlWithSaving.cs). Replace your old `TaskControl.cs` script with this modified one.


You would essentially create a `KeyFrame` object, equivalent to each row in a Pandas DataFrame (if you are familiar with Pandas). 
```C#
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
```

And write a string to a file and save it as csv.
```C#
public string ToCSV()
    {
        var sb = new StringBuilder("Trial,Action,ReactionTime,Outcome");
        foreach(var frame in keyFrames)
        {
            sb.Append('\n').Append(frame.Trial.ToString()).Append(',').Append(frame.Action.ToString()).Append(',').Append(frame.ReactionTime.ToString()).Append(',').Append(frame.Outcome.ToString());
        }

        return sb.ToString();
    }

```
You will also need to change all of the references to `TaskControl` in other scripts (such as `ChoiceHanlding.cs` and `ButtonPressing.cs`) to `TaskControlWithSaving`. You can see a sample output (saved) csv [here](sources/Example1/output.csv). Just like how you have saved the data to csv, you can also read from a csv with trials and outcomes to have stochastic and non-stationary outcomes (but that is beyond the scope of this tutorial due to time constraints). 

## Example - Physical foraging task in a maze
The second task is a maze task where participants navigate through a maze and collect gold bar that is scattered in the maze. 
### Steps

1. **Assets import**: From asset store download and import [HUD for VR - Sterile Future](https://assetstore.unity.com/packages/2d/gui/icons/hud-for-vr-sterile-future-120259), [Outdoor Ground Textures](https://assetstore.unity.com/packages/2d/textures-materials/floors/outdoor-ground-textures-12555), [18 High Resolution Wall Textures](https://assetstore.unity.com/packages/2d/textures-materials/brick/18-high-resolution-wall-textures-12567), [Medieval Gold](https://assetstore.unity.com/packages/3d/props/medieval-gold-14162), [SteamVR](https://assetstore.unity.com/packages/tools/integration/steamvr-plugin-32647).
2. **Level design**: 
    - The maze: You can create a maze by deforming 3d cubes into walls. We provide a very simple maze to download to use as an example: [Maze package](unity_packages/Maze.unitypackage). Place the maze in the scene with position being `(0,0,0)`.
    - Player and UI: In this example, we use a virtual dashboard as the main UI for people to interact with. You can download and import our pre-coded UI dashboard here: [Player UI](unity_packages/PlayerUI_NoSteamVRInput.unitypackage). SteamVR input may be not set correctly. To do that, click Window->SteamVR Input. Create a menu binary input if it does not exist. Save and generate. Open Binding UI, and check if all inputs have been assigned. Adjust canvas size if necessary in Board->Canvas Layer->CanvasLocationControl->Canvas.
    - Then place prefabs `Resources/Prefabs/Board` and `Resources/Prefabs/Players` in the scene. Assign properties under Board UI Manager correctly. This includes `Menu Button`, `Trigger`, `Left Hand`, and `Right Hand`. The Left hand and right hand should be from the Player's hands. 
    - Make `Medieval_Gold/i_gnot` a prefab saved in `Resource/Prefabs/i_gnot` by dragging it to the scene and drag to the corresponding asset folder. Edit the prefab, and attach `Interactable` and `Throwable` component to it. Disable `Use Gravity` and enable `Is Kinematic` in Rigid body component. Attach a `Mesh Collider` component to it. Tick `Convex` option for `Mesh Collider`.
![Task Setup](imgs/MazeLevelDesign.PNG)
Figure 2: Unity editor screenshot of the maze task
1. **Task execution**: 
     - Now we have a UI dashboard. It can be turned on/off by pressing the menu button binded to the controller. We can implement this by adding the following code to the `Update` function (See [complete Task control script](sources/Example2/TaskControl.cs)).
        ```C#
        if (UIManager.userAction == "MenuButtonPressed")
        {
            toggleUI ^= 1;
            UIManager.SetBoardDisplay(Convert.ToBoolean(toggleUI));
            UIManager.userAction = "";
        }
        ```
        When the menu button is pressed, the `UIManager.userAction` will be set to "MenuButtonPressed". We check it in every `Update` function call and take action (i.e. change the dashboard display status). We need to reset it back to "". The way we do it is different from conventional UI handling where you process everything in some callback function triggered by a key pressing. It is however more modular and keeps the entire control logic within the `Update` function. This advantage will be more obvious when it comes to our external interactive control interface. 
     - The task is very simple. It consists of two states: `Idle` and `InTrial`. For game design, it is helpful to refer to the [Finite-state machine (FSM)](https://en.wikipedia.org/wiki/Finite-state_machine) as the model. Here, we have two states. When the participant clicks start, the task state transits from `Idle` to `InTrial` and spawn a gold bar for the participant to pick up. When the participant picks up the gold bar (wait for 0.5 second), or time is up, the task state goes back to `Idle` again. 
        ```C#
        Player player = Player.instance;
        if (player)
        {
            foreach (Hand hand in player.hands)
            {
                GameObject attachedObject = hand.currentAttachedObject;
                if (attachedObject != null)
                {
                    if (!foundObject)
                    {
                        foundObject = true;
                        foundObjectTimer = Time.time;
                    }
                }
            }
        }

        if (foundObject)
        {
            if (Time.time - foundObjectTimer > 0.5)
            {
                UIManager.SetBoardDisplay(true);
                UIManager.SetMainText("You found the object! Click to continue");
                toggleUI = 1;

                TaskState = "Idle";
                foundObject = false;
                foundObjectTimer = 0;
                Destroy(GameObject.Find("goldbar"));
            }
        }

        if ((Time.time - trialStartTime > 60) && !foundObject)
        {
            UIManager.SetBoardDisplay(true);
            UIManager.SetMainText("Time is up! Click to to try again");
            toggleUI = 1;
            TaskState = "Idle";
            Destroy(GameObject.Find("goldbar"));
        }
        ```
2. **Going through walls**: Now we have completed the task execution logic. One might already notice our walls in the virtual world is useless! If you don't have a wall in the real world, then you can go through a wall in the virtual world. Generally, there are three (or four) ways of dealing with this:
    
    1. Avoid using walls. Don't add walls unless you really need to.
    2. Fade the screen to black when hitting the wall.
    3. Use controller buttons to move forward rather than real body movement.
    4. Stop the player's camera going into the wall by adding an offset to the camera if the participant is moving in the wall direction. 
   
   In commercial VR game development, all strategies are used. However, when we are running experiments, it could be better to use one single moving strategy. For example, in a VR game, you are usually allowed to move with controller buttons as well as real body movement. Unless you want to compare people's preference of this two interaction ways, I think in experiments, to keep the behavioural data more controlled, it is better to either use controllers only or use real body movement. 

   **Fade to black on collision**: The key thing here is to overlay a black layer to the screen output. `OpenVR.Compositor` has a function called `FadeToColor` that allows VR compositor to overlay a colour layer on top of the the game visual output. We also ned to detect head collisions with the wall. It is not a good idea to put a collider on the VR camera itself because in Unity the collision can only be detected when there is a rigid body attached to one of the collided objects. We attach the [following script (complete version)](sources/Example2/DarkenOnCollision.cs) to `Player` -> `FollowHead` -> `HeadCollider`, make sure `Is Kinematic` is disabled for `HeadCollider`:
   ```C#
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
   ```
   The code will put a 0.5% transparent mask when the head collides with some objects and removes the mask if the collision exits.

   **Movement controlled by hand controller**: Add a character controller component to the Player (tracking origin) game object and attach [this script](sources/Example2/VRController.cs) to activate game object should enable moving in the head direction by pressing the  `squeeze` button on the controller.

   A new problem with this approach is the collision detection is only enabled for the collider created at Player's position. This is not the same as the actual VR headset position unless the participant sits in the centre of the tracking area. We can either ask the participant to sit in the centre of the tracking region we pre-calibrated, or disable the position tracking. To disable tracking, you can make the Player scale extremely small. Or for a more robust solution, if you are using Unity XR mode (at the beginning when you import SteamVR package), change `SteamVR/InteractionSystem/Core/Scripts/Player.cs` line 268, 269 to:
   ```C#
   if (hmd.GetComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>() == null)
    {
        hmd.gameObject.AddComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>();
        // changed this to enable only rotation tracking
        hmd.gameObject.GetComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>().trackingType = UnityEngine.SpatialTracking.TrackedPoseDriver.TrackingType.RotationOnly;
   }
   ```
   which changes the `TrackedPoseDriver` to `RotationOnly`, and replace `SteamVR_Behaviour_Pose` component for each hand with the following `Custom_Behaviour_Pose` component:
    ```C#
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
    ```
    We also need to modify `com.unity.xr.legacyinputhelpers\Runtime\TrackedPoseDriver\TrackedPoseDriver.cs` to expose `trackedPosition`.
    
    One thing to note is once you disable real movements in VR, it is important to make sure participants' heads are still. Moving physically while not moving visually can cause strong motion sickness. 

    **Add offset to the headset**: Another popular solution in VR games is to add offset to the headset if the headset is moving into the virtual wall. However, one problem with this is the offset can add up and it could be too much that the centre of the virtual world is on the edge of the real world. Usually, those games implement a reset feature that resets the virtual centre to the current position. For running experiments, this extra unnatural re-centre feature could be a confounder for the behavioural data depending on the research question of your study.
## Example - Realistic rendering environment

The above examples use Unity standard render pipeline which uses [forward rendering](https://docs.unity3d.com/Manual/RenderingPaths.html). It is fast and friendly to low-performance computers / mobile devices, but the graphics quality is not great. Other render pipeline like [High Definition Render Pipeline (HDRP)](https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@14.0/manual/index.html) uses deferred shading as the default rendering path which gives much better quality but is much more computationally expensive.
![Jungle](imgs/JungleHDRPExample.PNG)
Figure 3: Unity editor screenshot of an HDRP rendered environment
One of our experiments in a realistic jungle environment.

# Part 2 Linking other data streams

For this part, we will discuss more things happening outside Unity. For more information and code bases of our software infrastructure, please refer to [PainLabInteractiveControlPanel
](https://github.com/ShuangyiTong/PainLabInteractiveControlPanel).
## Collecting and saving more data streams
Perhaps, one key difference between Unity-based studies and other conventional 2D studies in programming is Unity has higher concurrency and demands higher real-time performance. Conventional experiments have clear steps, something like "displaying cue" -> "subject response" -> "reward/punishment". Unity is running through all active game object's components and each `Update` function should be completed quickly rather than doing something and waiting in PsychoPy. This can be done through asynchronous function calls or multi-threading. 
### Collect data in Unity
To collect data in Unity, one needs to write or utilize packages that read external data into Unity, which is usually C# programming. 
#### Motion tracking data
Point tracking can be added separately with [Vive Tracker](https://www.vive.com/uk/accessory/tracker3/). By attaching a `SteamVR_Behaviour_Pose` component to some game object and correctly configuring Pose input (Note a [SteamVR existing issue](https://steamcommunity.com/app/250820/discussions/4/3949029052301918254/)), the tracking data then can be retrieved from the game object's position.

#### Eye tracking data
With the HTC Vive Pro Eye headset, eye-tracking data can be collected in Unity through [Eye and Facial Tracking SDK](https://developer.vive.com/resources/vive-sense/eye-and-facial-tracking-sdk/). It not only tracks the direction of the eye ball, but also provides real-time pupil sizes. We used this eye tracking data to track what objects participant were currently looking at. You can [download our Unity Prefabs](unity_packages/Eyetracker.unitypackage) here if you have installed the SDK.
#### Integrating A Heart Rate (HR) Monitor into Unity VR

Goals of this lab are to discuss and demonstrate additional techniques for collecting sensor data and making it available for use within VR. We’ll work with the following concepts:
-   Bluetooth-based sensor connectivity and data integration
-   Simply SQL database usage
-   Database integration into UnityVR
-   Synchronous versus asynchronous communication
-   Inter-process data communication patterns
-   Using a database to asynchronously capture and provide data to VR applications
-   Key processing constructs inside virtual reality development products like Unity, especially Coroutines and how they differ from frame rate-coupled processes  
The lab will focus on asynchronously collecting HR data from an optical sensor and displaying it in real time within a Unity virtual reality environment. 

We are going to use an inexpensive Bluetooth device (Polar Verity Sense), pair it with the Windows laptop, and read the HR data from the device and continuously display the subject’s heart rate inside Unity VR.  

Polar Verity Sense is an optical heart rate monitor that provides freedom of movement and multiple options for viewing and recording your heart rate. With Bluetooth®, ANT+, and internal 

The device is a versatile high-quality optical heart rate sensor that measures heart rate from your arm or temple. It's a great alternative to heart rate chest straps or wrist based devices. 

![](imgs/WristHeartRateBand.png)

For more information, please refer to this document [Integrating A Heart Rate Sensor Unity VR -- Written by Doug Nelson](docs/Integrating%20A%20Heart%20Rate%20Sensor%20Unity%20VR%20--%20Written%20by%20Doug%20Nelson.docx)

### Collect data outside Unity
Collecting data outside Unity usually requires some type of communication between Unity and the program collecting the data. Some protocol like [labstreaminglayer](https://github.com/sccn/labstreaminglayer) has Unity package that can help with that. But sometimes, just recording the data from some other external software (e.g. [BrainVision Recorder](https://brainvision.com/products/recorder/)) works as well. The key question is how to synchronize data recorded in a separate software without communicating with Unity. A simple way is to use timestamps. Both Unity and other third-party software records timestamp from the operating system's clock (For researchers who are more familiar with marker synchronization rather than a sequence of timestamps, you can check out [this article by Labstreaminglayer developer](https://labstreaminglayer.readthedocs.io/info/time_synchronization.html)). 

We did develop a series of software aims to resolve this issue in one solution. We developed an application layer simple bi-directional data exchange protocol over TCP (PainLabProtocol). PainLabProtocol is similar to Labstreaminglayer but less sophisticated. Its performance may be poorer compared to other protocols like Labstreaminglayer or more efficient game server protocols over UDP, but it emphasizes data readability (transmit and saved in JavaScript) and flexibility in data format. The key software here is an interactive control panel ([PainLabInteractiveControlPanel](https://github.com/ShuangyiTong/PainLabInteractiveControlPanel)). It acts as the server to collect data from different devices that speak PainLabProtocol including [Unity](https://github.com/ShuangyiTong/PainLabDeviceNIDAQDotNet4.5VS2012/blob/master/PainLabDeviceNIDAQDotNet4.5VS2012/PainlabProtocol.cs), [Arduino](https://github.com/ShuangyiTong/PainLabDeviceEmbedded), [NI DAQ](https://github.com/ShuangyiTong/PainLabDeviceNIDAQDotNet4.5VS2012), [Azure speech recognition](https://github.com/ShuangyiTong/PainLabDeviceVoiceRecognitionAzure), and [Brain Products, g.tec LSL connector](https://github.com/ShuangyiTong/PainLabLSLCompatibilityLayerLiveAmp). It is similar to data acquisition software like [BrainVision Recorder](https://brainvision.com/products/recorder/) or [Labrecorder](https://github.com/labstreaminglayer/App-LabRecorder), but it also includes the functionality to send commands to the device and allows you to plug in scripts to control the task easily in real-time.

![CP](imgs/AllInOneControlPanel.PNG)
Figure 4: All in one interactive control panel
![CP2](imgs/InteractiveControlPanelScriptTab.PNG)
Figure 5: Interactive panel controlled by script with ad-hoc parameter modification during experiment
## Apply stimulation feedback

### Unity inter-process communication with TCP sockets

We enable inter-process communication between Unity and outside devices for real-time data transmission via TCP sockets and socket API. This can be done in various programming languages. Here we show a brief example of two-way communication between the server (Python script) and client (Unity GameObject with C# script), where the server can also work for processing tasks on external devices (e.g. real shock generation of the stimulators). Especially for the stimulation feedback, the Unity client will first know when and how to generate the stimulation when the task is ongoing, and send the corresponding message to the Python server, where you can meanwhile process a real stimulation through an I/O Device connecting the server and stimulator. A detailed [example](https://github.com/Chronowanderer/CogPainExp-TCP-connection) of its application will be explained afterwards.

The following TCP server script is built in Python, meanwhile keeps listening to messages from the Unity client. Make this as an independent script and run it with Python before launching the Unity task. 
```python
import time
import socket
import os

# Make sure the following address setup is consistent between Python server and Unity client.
HOST = "127.0.0.1"
PORT = 60000

class SocketConnection:
    def __init__(self, host, port):
        self.address = (host, port)

    def connect(self): # Server implementation
        server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        server.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        server.bind(self.address)
        server.listen(5)
        try:
            connectionFlag = False
            if not connectionFlag:
                clientsocket, _ = server.accept()
                # print("Connection accepted.")
            while True:
                connectionFlag = self.read_from_client(clientsocket) # Keep listening message from client.
            if not connectionFlag:
                clientsocket.close()
        except IOError as e:
            print(e.strerror)

    def read_from_client(self, clientsocket): # Listen message from client.
        content = None
        try:
            input = clientsocket.recv(4096)
            if input:
                content = input.decode('ascii')
        except IOError as e:
            print(e.strerror)

        if content:
            return True
        else:
            return False
    
    def send_to_client(self, clientsocket, content = ""): # Send message to client.
    # Not implemented in this brief example. Feel free to use/modify this function with your own needs.
        try:
            clientsocket.send(content)
            # print(time.time(), ': Message ', output, ' sent.')
            pass
        except IOError as e:
            print(e.strerror)
        
        return 0

if __name__ == "__main__":
    SocketConnection(HOST, PORT).connect() # Launch the server.

```

The following TCP client script is built in Unity, meanwhile keeping listening messages from the Python server. Attach this C# script to an empty GameObject in a scene that is only called once (usually as the first scene when launching the Unity task). The `DontDestroyOnLoad` function would ensure the client to exist in the task for a persistent connection with the server. 
```C#
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;


public class UnityCallPython : MonoBehaviour
{
    [Tooltip("IP address")]
    public string ip = "127.0.0.1";
    [Tooltip("Port number")]
    public int port = 60000;
    
    [Tooltip("Scene names")]
    public string expScene;
    
    [Tooltip("Blocking time (sec)")]
    public float maxWaitingTime = 30f;
    public float sceneBlockingTime = 1f;

    public Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    
    long startTime;
    
    // Client setup with finding server through the corresponding address.
    private void Awake()
    {
        try
        {
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            Debug.Log("Connected!");
            Shock(0, -100); // Send a test stimulation message.
        }
        catch (Exception)
        {
            Debug.Log("Connection Error!");
        }

        DontDestroyOnLoad(this.gameObject);
        startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }
    
    // Keep listening from server for the task trigger signal. A real application is to wait for the TR pulse from MRI scanner to launch the task.
    private void Update()
    {
        long currentTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        if (currentTime - startTime > sceneBlockingTime * 1000)
        {
            ReceiveTR(maxWaitingTime);
            SceneManager.LoadScene(expScene);
        }
    }
    
    // Stimulation message.
    public void Shock(int bodyPart, float shockingDuration)
    {
        SendMessage(bodyPart.ToString(), shockingDuration.ToString());
    }
    
    // Send message to server.
    public void SendMessage(params string[] argvs)
    {
        string content = "";
        if (argvs != null)
        {
            foreach (string item in argvs)
            {
                content += " " + item;
            }
        }

        try
        {
            byte[] byteData = Encoding.ASCII.GetBytes(content);
            clientSocket.Send(byteData);
        }
        catch (ArgumentNullException ane)
        {
            Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
        }
        catch (SocketException se)
        {
            Console.WriteLine("SocketException : {0}", se.ToString());
        }
        catch (Exception e)
        {
            Console.WriteLine("Unexpected exception : {0}", e.ToString());
        }
    }
    
    // Receive message from the server for the task trigger signal. 
    public void ReceiveTR(float maxTime)
    {
        long time = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();

        bool TR_flag = false;
        bool TR_first = false; // ignoring the first jammed input

        while (new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds() - time < maxTime)
        {
            // Waiting for TR signal to start the task
            try
            {
                string data = null;
                byte[] bytes = null;
                bytes = new byte[1024];
                int bytesRec = clientSocket.Receive(bytes);
                data = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                if (Int32.Parse(data[data.Length - 1].ToString()) > 0)
                {
                    if (TR_first)
                    {
                        TR_flag = true;
                        break;
                    }
                    TR_first = true;
                }
            }
            catch (Exception e)
            {
                TR_flag = true;
                Debug.Log("TR Retrieval Error!");
                Debug.Log(e);
                break;
            }
            Shock(0, -100); // Send a test stimulation message.
        }
        if (!TR_flag)
        {
            Debug.Log("TR Retrieval exceeds Tolerance Time!");
        }
    }

}
```
See a more detailed [example](https://github.com/Chronowanderer/CogPainExp-TCP-connection) of its application on real-time task stimulation. Two different stimulators (DS5) are connected to an external I/O Device (NI USB-6212) through different ports, which is then connected to the Python server. The I/O Device is also connected with a 1-0 task trigger (e.g. for TR pulse from the scanner), of which the signal comes to the server and then is sent to the Unity client to launch the task at an appropriate time. On the other hand, the Unity client is attached to a persistent non-visible GameObject in the Unity task. When the player has done something for a stimulation, the corresponding stimulation information message is directly sent from the Unity client to the Python server, which then generates a real stimulation from those stimulators with the help of the I/O device. Both the 1-0 trigger data and stimulation data are recorded and saved based on the server system time, so that to enable the temporal alignment between task stimulation and trigger timing.

### Unified data streaming software

With our [interactive control panel](https://github.com/ShuangyiTong/PainLabInteractiveControlPanel), here is a simple example that sends shock every 1 second.
```javascript
const stimulator_id = "SC91BBkyiIWxnJMipKYk";
var last_shocked = Date.now();

actionFunction = (device_id, dataframe) => {
    if (Date.now() - last_shocked > 1000) {
        sendCommand(stimulator_id, "normalised_current_level", 3);
        last_shocked = Date.now();
    }
}
```
In this example, this JavaScript script code is driven by `actionFunction`. Whenever there is new data comes in, this `actionFunction` is called. This resembles the `Update` function in Unity. It has two parameters `device_id` and `dataframe`. Users can write code in the `actionFunction` to give feedback based on the data in the `dataframe`. Here we only give shocks to participants based on a timer. In the next section, we will utilize this control panel for more complex data interactions. 
## Closed-loop task control with ad-hoc scripts
We can move the control script out of Unity to the interactive control panel. This would make it easier to interact with other data streams and gives closed-loop task control with more flexibility.

We start from the second example by importing [this package](unity_packages/PainlabProtocol.unitypackage). After importing the package, we can remove the Task Control component from `TaskControl` game object and add `PainlabTask` component that we just imported from the package to it.

There are already some functionalities implemented in `PainlabTask` including position tracking, object that is being picked up, and some UI functions. We are now rewrite everything from Task Control component in JavaScript running in the control panel. We still need a little bit help from the client side, which is in C# code. The only help we need is to write a C# code snippet in `PainlabTask` to help us generate the gold bar. We add two member variables to `TaskControlFrame`:
```C#
public int generate_goldbar = -1;
public int destroy_goldbar = -1;
```
Values default to `-1`, we use this as our null/bottom type, meaning the value is not set in the control frame. It is slightly abusing the `int` type's original definition in C#, but it saves much time to write JSON parser ourselves.
In the `ApplyControlData` function body, add the generating gold bar snippet:
```C#
if (generate_goldbar != -1)
{
    GameObject goldBar = Resources.Load<GameObject>("Prefabs/i_gnot");
    var rnd = new System.Random(DateTime.Now.Millisecond);
    double tick_1 = rnd.NextDouble();
    double tick_2 = rnd.NextDouble();
    GameObject initGoldBar = UnityEngine.Object.Instantiate(goldBar, new Vector3((float)(5 * tick_1 - 4.0), 0.5f, (float)(5 * tick_2 - 0.5)), Quaternion.identity);
    initGoldBar.name = "goldbar";
}
if (destroy_goldbar != -1)
{
    UnityEngine.Object.Destroy(UnityEngine.GameObject.Find("goldbar"));
}
```
It is also recommended to add the following items to 'data_to_control' object in `Resources/device_descriptor.txt` but not necessary:
```JSON
"generate_goldbar": "int",
"destroy_goldbar": "int"
```
Now we have everything done on the Unity side, we can work on the control panel side. The control script is basically a JavaScript rewritten of the C# version.
```JavaScript
const { TouchBarSlider } = require("electron");
const { transpose } = require("underscore");

const UNITYVR_VR_DEVICE_ID = "o8Y6VNWF7orzDfPGCrJh";
var TaskState = "Idle";
var current_ui_board = 1;
var trial_start_time = 0;
var found_object = false;
var found_object_timer = 0;

actionFunction = (device_id, dataframe) => {
    if (dataframe["user_action"] == "MenuButtonPressed") {
        current_ui_board ^= 1; // toggle 0 and 1
        sendCommand(UNITYVR_VR_DEVICE_ID, "show_ui_board", current_ui_board);
        if (current_ui_board == 1) {
            sendCommand(UNITYVR_VR_DEVICE_ID, "activate_confirm_button", 1);
        }
    }

    if (TaskState == "Idle") {
        if (dataframe["board_command"] == "ConfirmButtonPressed") {
            sendMultipleCommands(UNITYVR_VR_DEVICE_ID, {
                "set_board_main_text": "Find the Gold Bar!",
                "show_ui_board": 0,
                "generate_goldbar": 1
            });
            current_ui_board = 0;
            trial_start_time = Date.now();
            TaskState = "InTrial";
        }
    } else if (TaskState == "InTrial") {
        if (dataframe["pickable_object_attached"] != "") {
            if (!found_object) {
                found_object = true;
                found_object_timer = Date.now();
            }
        }

        if (found_object) {
            if (Date.now() - found_object_timer > 500) {
                sendMultipleCommands(UNITYVR_VR_DEVICE_ID, {
                    "set_board_main_text": "You found the object! Click to continue",
                    "show_ui_board": 1,
                    "destroy_goldbar": 1
                });
                current_ui_board = 1;
                found_object = false;
                found_object_timer = 0;
                TaskState = "Idle";
            }
        } else if (Date.now() - trial_start_time > 60000) {
            sendMultipleCommands(UNITYVR_VR_DEVICE_ID, {
                "set_board_title_text": "Time is up! Click to to try again",
                "destroy_goldbar": 1
            });
            TaskState = "Idle";
        }
    }
}
```
One can then add a painful shock as feedback when picking up the object easily by just add one more `sendCommand` line:
```JavaScript
...
        found_object_timer = Date.now();
        sendCommand("SC91BBkyiIWxnJMipKYk", "normalised_current_level", 3);
    }
...
```
