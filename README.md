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

1. From asset store import [SteamVR](https://assetstore.unity.com/packages/tools/integration/steamvr-plugin-32647), [HUD for VR - Sterile Future](https://assetstore.unity.com/packages/2d/gui/icons/hud-for-vr-sterile-future-120259), [pixel modern office extras](https://assetstore.unity.com/packages/3d/environments/urban/pixel-modern-office-extras-225670).
2. Create a floor (a plane with material) and place objects (chairs, desks) in the scene.
3. Create a trial start button and point display.
![Task setup](imgs/BanditExampleLevelDesign.PNG)
4. Place player prefab from `SteamVR/InteractionSystem/Core/Prefabs`
5. Create a new game object called TaskControl, add [task control script]() to the game object as a component.

## Example - Physical foraging task in maze

## Example - Realistic rendering environment

# Part 2 Linking other data streams

## Collecting data streams outside Unity

## Apply stimulation feedback

## Scriptable closed-loop task control