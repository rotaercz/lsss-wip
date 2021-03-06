﻿using Latios;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.InputSystem;

namespace Lsss
{
    public class TitleAndMenuUpdateSystem : SubSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((TitleAndMenu titleAndMenu) =>
            {
                if (titleAndMenu.titlePanel.activeSelf)
                {
                    bool somethingPressed = false;
                    var  gamepad          = Gamepad.current;
                    if (gamepad != null)
                    {
                        if (gamepad.buttonEast.wasPressedThisFrame ||
                            gamepad.buttonNorth.wasPressedThisFrame ||
                            gamepad.buttonSouth.wasPressedThisFrame ||
                            gamepad.buttonWest.wasPressedThisFrame ||
                            gamepad.leftStickButton.wasPressedThisFrame ||
                            gamepad.rightStickButton.wasPressedThisFrame ||
                            gamepad.startButton.wasPressedThisFrame ||
                            gamepad.selectButton.wasPressedThisFrame ||
                            gamepad.leftShoulder.wasPressedThisFrame ||
                            gamepad.leftTrigger.wasPressedThisFrame ||
                            gamepad.rightShoulder.wasPressedThisFrame ||
                            gamepad.rightTrigger.wasPressedThisFrame)
                        {
                            somethingPressed = true;
                        }
                    }
                    else
                    {
                        var mouse    = Mouse.current;
                        var keyboard = Keyboard.current;
                        if (mouse != null && keyboard != null)
                        {
                            if (mouse.leftButton.wasPressedThisFrame || mouse.rightButton.wasPressedThisFrame)
                                somethingPressed = true;
                            if (keyboard.anyKey.wasPressedThisFrame)
                                somethingPressed = true;
                        }
                    }
                    if (somethingPressed)
                    {
                        titleAndMenu.titlePanel.SetActive(false);
                        titleAndMenu.menuPanel.SetActive(true);
                    }
                }
                else
                {
                    var gamepad = Gamepad.current;
                    {
                        if (gamepad != null)
                            if (gamepad.bButton.wasPressedThisFrame || gamepad.crossButton.wasPressedThisFrame)
                            {
                                titleAndMenu.menuPanel.SetActive(false);
                                titleAndMenu.titlePanel.SetActive(true);
                            }
                    }
                }
                float a                             = 0.75f * (float)math.sin(Time.ElapsedTime / titleAndMenu.pulsePeriod * 2d * math.PI_DBL) + 0.5f;
                titleAndMenu.pressToStartText.color = new UnityEngine.Color(1f, 1f, 1f, a);

                if (titleAndMenu.selectedScene.Length > 0)
                {
                    var ecb                                                             = latiosWorld.SyncPoint.CreateEntityCommandBuffer();
                    ecb.AddComponent(sceneGlobalEntity, new RequestLoadScene { newScene = titleAndMenu.selectedScene });
                }
            }).WithoutBurst().Run();
        }
    }
}

