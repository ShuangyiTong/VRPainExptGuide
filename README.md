# A guide to set up virtual reality experiments

## Table of Contents
- [A guide to set up virtual reality experiments](#a-guide-to-set-up-virtual-reality-experiments)
  - [Table of Contents](#table-of-contents)
  - [Overview](#overview)
    - [What are the new things in VR compared to a traditional computer](#what-are-the-new-things-in-vr-compared-to-a-traditional-computer)
    - [What are the challenges / differences of a VR experiment](#what-are-the-challenges--differences-of-a-vr-experiment)
- [Part 1: Simple Task Setup](#part-1-simple-task-setup)
  - [Preparation](#preparation)
    - [Hardware](#hardware)
    - [Software](#software)
  - [Example - Simple Bandit Task](#example---simple-bandit-task)
    - [Steps](#steps)
  - [Example - Physical foraging task in maze](#example---physical-foraging-task-in-maze)
  - [Example - Realistic rendering environment](#example---realistic-rendering-environment)
- [Part 2 Linking other data streams](#part-2-linking-other-data-streams)
  - [Collecting data streams outside Unity](#collecting-data-streams-outside-unity)
  - [Apply stimulation feedback](#apply-stimulation-feedback)
  - [Scriptable closed-loop task control](#scriptable-closed-loop-task-control)

## Overview

Virtual reality (VR) is a new technology that brings new ways of presenting visual stimuli and interaction with a virtual environment, which makes it valuable for designing novel experiment paradigms. This guide is intended to help researchers with some experience setting up computer-based experiments (e.g. Psychopy, Psychtoolbox etc.) to get started with VR experiments.

### What are the new things in VR compared to a traditional computer

VR has been studied many years. Enormous engineering effort has been put into making virtual experience more realistic. Current VR technology advances might be categorised into two topics: vision and interaction. Vision is benefit from the fast developing computer graphics technology, for example, faster graphics processing and algorithm render virtual environment to realistic visual stimuli, and higher pixel density makes it possible to present a high resolution image on the screen of a wearable headset. Interaction advances are probably attributed to more robust and simpler motion tracking technology. Interaction are done by interacting with virtual objects in 3D space. This is much more natural than using a mouse and a keyboard.

### What are the challenges / differences of a VR experiment

In terms of the actual development, as we will see shortly, VR games use the same development platform as other non-VR games. It only changes two things: the way it outputs graphics (binocular vision) and the way of interaction (motion capture and controllers). A game engine or the VR manufacturer's software package usually does most of the things for us. What more important for us is VR's realistic experience can cause some necessary changes to existing paradigms.

- VR environments and all objects within are 3D. If you want to present a 2D cue, it needs to be on some plane in 3D. You would always allow the vision rotates when people rotate their head, otherwise it can cause dizziness. Therefore, the 3D spatial location of a cue relative to the subject is important.
- VR experiments usually run continuously. In a monitor-based task, a cue displays on a screen, and the subject presses a key. In VR, instead of pressing a key instantly, it can take certain amount of time to complete an action. For example, reaching an object in a go / no go task with real arm movement does not complete instantly but rather a continuous action.

One can always fall back to use VR headset as a big screen, and all interaction is done by pressing keys. In that case, it might be easier to run a monitor-based task rather than seeking to use VR.

# Part 1: Simple Task Setup
## Preparation

### Hardware

VR headset: There are two main stream VR headsets. One is PC-based and the headset is essentially a display. HTC Vive is the main manufacturer for PC-based VR. The other one is a standalone headset which the headset has a mobile operating system installed. Standalone headset has generally poorer performance when running on its own, but it can be connected to PC and acts as a display for PC VR as well. We currently use HTC Vive Pro Eye for lab experiments. It was the only headset with built-in eye tracking functionality when we made the purchase. However, there are more companies developing high-end VR headsets now. Recently released Pico 4 Enterprise and Meta Quest Pro are also equipped with eye tracking. We are keen to test their possibility to be used in lab environment. Pico 4 Enterprise and Meta Quest Pro are both standalone headsets.

We still prefer to use PC-based VR, not only because PC is more powerful but also PC can handle other datastream from other devices better (for example, EEG data). VR application usually require a good graphics hardware to run the game. Hardware updates frequently, so here we only show our currently running lab PC's specifications.

- CPU: Intel 11900KF
- GPU: NVIDIA RTX 3090 24GB
- Memory: 64GB DDR4
- Storage: 1TB SSD

Other lab / clinical equipment that were used previously in conventional experiments are usually applicable. The biggest issue is probably whether the device is portable and has acceptable motion artefacts when used with VR.

### Software

Operating system: We would recommend to use Windows 10 / 11 operating system.
Game engine: 

## Example - Simple Bandit Task

### Steps

1. **Assets import**: From asset store import [SteamVR](https://assetstore.unity.com/packages/tools/integration/steamvr-plugin-32647), [pixel modern office extras](https://assetstore.unity.com/packages/3d/environments/urban/pixel-modern-office-extras-225670).
2. **Level design**:
   - Create a floor by create a new plane. You can attach certain texture (e.g. wood) to the plane.
   - Place objects (chairs, desks) from the assets we just imported on the floor.
   - Place player prefab from `SteamVR/InteractionSystem/Core/Prefabs`
3. **UI design**: 
   - Create a trial start button with a cube and a TextMeshPro object (or UI text) placing on the desk.
   - Create a score board display in front of the desk using TextMeshPro object (or UI text) object.
![Task setup](imgs/BanditExampleLevelDesign.PNG)
1. **Task execution**: Create a new game object called TaskControl, add [task control script (complete version)](sources/Example1/TaskControl.cs) to the game object as a component. Using a separate script to control the global task progress is a neat way to write the task execution logic. This task is simple. The participant touches the start button, the options (books) are now available to choose. Once the participant picks a choice (by touching the book), reward points displayed in front of them. The start button is now available to be pressed again. We do this step by step.

   - Everything starts from the participant pressing the button. We use collider to do this (so in fact it is touching the button rather than pressing the button). So we create a [button pressing script (complete version)](sources/Example1/ButtonPressing.cs) and attach it to the button object so that when the button is touched with hand, `OnCollisionEnter` will be called (if the button has a collider. If not, simply add a box collider to the button gameobject).
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
      This function does three things: set the button state to pressed which is a member variable that we should defined it earlier (see [the complete version](sources/Example1/ButtonPressing.cs)). We also then change the instruction to "Pick one book" shown on the button. Note here we make the text object a child object of the button so that we can find it by calling `transform.GetChild(0)`. There is no other child, so the first one with index 0 is the text object. Lastly, we need to tell the task control script that this button is pressed by calling the public member function `StartButtonPressed` of `TaskControl`.

      We then would like to have some animation effect for the button. When the button is pressed, we would like to have it gradually lowering down to create a 'pressed' feeling. The distance and speed requires manual tweaking. It is called in `FixedUpdate` so that it is update at constant rate. If you want to rather update it in `Update`, which two calls are called in a variable time interval, you need to calculate the moving distance using time passed since last frame to ensure the button moves at deterministic speed.
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
      The `TaskControl` component then knows the button is now pressed through the function call `StartButtonPressed`, it simply sets a flag indicating it is now in a new trial and it will then handle the book choice correctly.

   - Next we need another script for each choice (book): [choice handling (complete version)](sources/Example1/ChoiceHandling.cs). It does two things: a. tell `TaskControl` if the hand has collided a book and which book is in collision, which means if the participant made a choice, b. highlight the book by changing the material when the hand is in collision with the book. This is done by the following code
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
      Let us go back to `TaskControl.ObjectSelected`. If it is in Trial (after button is pressed), the reward points is set depending on the choice. Here two choices are associated deterministically reward, so it is a boring bandit task. If total trial count `trialCount` exceeds maximum trials `totalTrials`, we will disable the button and the participant won't be able to start a new trial. 
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
      The reward points pop-up is only shown temporary. We record the time it is set active, then turn it off after 1 second in the `Update` function.
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
   This completes the whole simple experiment design. Of course, we still need to save the behavioural data and possibly connecting and synchronise with other devices like EEG, which will be covered in part 2. Unforturnately due to [Unity Asset Store EULA](https://unity.com/legal/as-terms), we may not distribute the full project containing Asset Store materials, but you can download a build version here: 

## Example - Physical foraging task in maze

1. From asset store import [HUD for VR - Sterile Future](https://assetstore.unity.com/packages/2d/gui/icons/hud-for-vr-sterile-future-120259), 
## Example - Realistic rendering environment

# Part 2 Linking other data streams

## Collecting data streams outside Unity

## Apply stimulation feedback

## Scriptable closed-loop task control