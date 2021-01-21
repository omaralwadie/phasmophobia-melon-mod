using MelonLoader;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using Console = System.Console;
using Object = UnityEngine.Object;
using String = System.String;

namespace PhasmoMelonMod
{
    public class Main : MelonMod
    {
        public override void OnApplicationStart()
        {
            BasicInject.Main();
            Console.Title = string.Format("Phasmophobia");
            MelonLogger.Log("[+] Set console title to: Phasmophobia");
        }
        public override void OnLevelWasLoaded(int level)
        {
            if(gameStarted && initializedScene == 1)
                DisableAll();
            if(initializedScene == 1)
                gameStarted = true;
        }
        public override void OnLevelWasInitialized(int level)
        {
            if (level == 1 && canRun)
            {
                canRun = false;
                new Thread(() =>
                {
                    while (isRunning)
                    {
                        coRoutine = MelonCoroutines.Start(CollectGameObjects());
                        Thread.Sleep(5000);
                    }
                }).Start();
            }
            initializedScene = level;
            MelonLogger.Log("[+] Initialized Scene: " + mapNames[initializedScene]);
        }
        public override void OnUpdate()
        {
            Keyboard keyboard = Keyboard.current;

            if (keyboard.leftArrowKey.wasPressedThisFrame)
            {
                CheatToggles.enableBI = !CheatToggles.enableBI;
                if(CheatToggles.enableBI)
                {
                    CheatToggles.enableBIGhost = true;
                    CheatToggles.enableBIMissions = true;
                    CheatToggles.enableBIPlayer = true;
                }
                else
                {
                    CheatToggles.enableBIGhost = false;
                    CheatToggles.enableBIMissions = false;
                    CheatToggles.enableBIPlayer = false;
                }
                MelonLogger.Log("[+] Basic informations: Toggled " + (CheatToggles.enableBI ? "On" : "Off"));
            }

            if (keyboard.upArrowKey.wasPressedThisFrame)
            {
                CheatToggles.enableEsp = !CheatToggles.enableEsp;
                if (!CheatToggles.enableEspGhost && !CheatToggles.enableEspPlayer && !CheatToggles.enableEspBone && !CheatToggles.enableEspOuija && !CheatToggles.enableEspEmf && !CheatToggles.enableEspFuseBox)
                {
                    CheatToggles.enableEsp = true;
                }
                if (CheatToggles.enableEsp)
                {
                    CheatToggles.enableEspGhost = true;
                    CheatToggles.enableEspPlayer = true;
                    CheatToggles.enableEspBone = true;
                    CheatToggles.enableEspOuija = true;
                    CheatToggles.enableEspEmf = true;
                    CheatToggles.enableEspFuseBox = true;
                }
                else
                {
                    CheatToggles.enableEspGhost = false;
                    CheatToggles.enableEspPlayer = false;
                    CheatToggles.enableEspBone = false;
                    CheatToggles.enableEspOuija = false;
                    CheatToggles.enableEspEmf = false;
                    CheatToggles.enableEspFuseBox = false;
                }
                MelonLogger.Log("[+] ESP: Toggled " + (CheatToggles.enableEsp ? "On" : "Off"));
            }

            if (keyboard.downArrowKey.wasPressedThisFrame)
            {
                CheatToggles.enableFullbright = !CheatToggles.enableFullbright;
                MelonLogger.Log("[+] Fullbright: Toggled " + (CheatToggles.enableFullbright ? "On" : "Off"));
                if (CheatToggles.enableFullbright == true)
                {
                    Fullbright.Enable();
                }
                else
                {
                    Fullbright.Disable();
                }
            }

            if (keyboard.insertKey.wasPressedThisFrame || keyboard.deleteKey.wasPressedThisFrame || keyboard.rightCtrlKey.wasPressedThisFrame)
            {
                CheatToggles.guiEnabled = !CheatToggles.guiEnabled;
                MelonLogger.Log("[+] GUI: Toggled " + (CheatToggles.guiEnabled ? "On" : "Off"));

                if (CheatToggles.guiEnabled)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    if (myPlayer != null)
                        myPlayer.field_Public_FirstPersonController_0.enabled = false;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    if (myPlayer != null)
                        myPlayer.field_Public_FirstPersonController_0.enabled = true;
                }
            }
            if (keyboard.hKey.wasPressedThisFrame && !CheatToggles.guiEnabled && CheatToggles.enableHotkeys)
            {
                Trolling.Hunt();
            }

            if (keyboard.iKey.wasPressedThisFrame && !CheatToggles.guiEnabled && CheatToggles.enableHotkeys)
            {
                Trolling.Interact();
            }

            if (keyboard.oKey.wasPressedThisFrame && !CheatToggles.guiEnabled && CheatToggles.enableHotkeys)
            {
                Trolling.Appear();
            }

            if (keyboard.pKey.wasPressedThisFrame && !CheatToggles.guiEnabled && CheatToggles.enableHotkeys)
            {
                Trolling.Idle();
            }
            if (keyboard.uKey.wasPressedThisFrame && !CheatToggles.guiEnabled && CheatToggles.enableHotkeys)
            {
                Trolling.LockDoors(3);
            }
            if (keyboard.lKey.wasPressedThisFrame && !CheatToggles.guiEnabled && CheatToggles.enableHotkeys)
            {
                Trolling.LockDoors(1);
            }
        }
        public override void OnGUI()
        {
            if (CheatToggles.guiEnabled)
            {
                if (initializedScene > 1)
                {
                    if (GUI.Toggle(new Rect(500f, 2f, 150f, 20f), CheatToggles.guiGhost, "Ghost GUI") != CheatToggles.guiGhost)
                    {
                        CheatToggles.guiGhost = !CheatToggles.guiGhost;
                        CheatToggles.guiGhostTroll = false;
                        CheatToggles.guiESP = false;
                        CheatToggles.guiHelper = false;
                        CheatToggles.guiHelperInfo = false;
                        CheatToggles.guiTroll = false;
                        CheatToggles.guiDebug = false;
                        CheatToggles.guiTest = false;
                        CheatToggles.guiFeatureCollection = false;
                    }
                    if (CheatToggles.guiGhost == true)
                    {
                        if (GUI.Toggle(new Rect(650f, 2f, 150f, 20f), CheatToggles.enableEspGhost, "Ghost ESP") != CheatToggles.enableEspGhost)
                        {
                            CheatToggles.enableEspGhost = !CheatToggles.enableEspGhost;
                            MelonLogger.Log("[+] ESP: Toggled " + (CheatToggles.enableEspGhost ? "On" : "Off"));

                        }
                        if (GUI.Toggle(new Rect(650f, 22f, 150f, 20f), CheatToggles.guiGhostTroll, "Troll Options") != CheatToggles.guiGhostTroll)
                        {
                            CheatToggles.guiGhostTroll = !CheatToggles.guiGhostTroll;
                        }
                        if (CheatToggles.guiGhostTroll == true)
                        {
                            if (GUI.Button(new Rect(800f, 2f, 150f, 20f), "Hunt") && levelController != null)
                            {
                                Trolling.Hunt();
                            }
                            if (GUI.Button(new Rect(800f, 22f, 150f, 20f), "Idle") && levelController != null)
                            {
                                Trolling.Idle();
                            }
                            if (GUI.Button(new Rect(800f, 42f, 150f, 20f), "Appear") && levelController != null)
                            {
                                Trolling.Appear();
                            }
                            if (GUI.Button(new Rect(800f, 62f, 150f, 20f), "Unappear") && levelController != null)
                            {
                                Trolling.UnAppear();
                            }
                            if (GUI.Button(new Rect(800f, 82f, 150f, 20f), "FuseBox") && levelController != null)
                            {
                                Trolling.FuseBox();
                            }
                        }
                    }
                    if (GUI.Toggle(new Rect(500f, 22f, 150f, 20f), CheatToggles.guiESP, "ESP GUI") != CheatToggles.guiESP)
                    {
                        CheatToggles.guiESP = !CheatToggles.guiESP;
                        CheatToggles.guiGhost = false;
                        CheatToggles.guiGhostTroll = false;
                        CheatToggles.guiHelper = false;
                        CheatToggles.guiHelperInfo = false;
                        CheatToggles.guiTroll = false;
                        CheatToggles.guiDebug = false;
                        CheatToggles.guiTest = false;
                        CheatToggles.guiFeatureCollection = false;
                    }
                    if (CheatToggles.guiESP == true)
                    {
                        if (GUI.Toggle(new Rect(650f, 2f, 150f, 20f), CheatToggles.enableEspGhost, "Ghost ESP") != CheatToggles.enableEspGhost)
                        {
                            CheatToggles.enableEspGhost = !CheatToggles.enableEspGhost;
                            MelonLogger.Log("[+] Ghost ESP: Toggled " + (CheatToggles.enableEspGhost ? "On" : "Off"));

                        }
                        if (GUI.Toggle(new Rect(650f, 22f, 150f, 20f), CheatToggles.enableEspPlayer, "Player ESP") != CheatToggles.enableEspPlayer)
                        {
                            CheatToggles.enableEspPlayer = !CheatToggles.enableEspPlayer;
                            MelonLogger.Log("[+] Player ESP: Toggled " + (CheatToggles.enableEspPlayer ? "On" : "Off"));

                        }
                        if (GUI.Toggle(new Rect(650f, 42f, 150f, 20f), CheatToggles.enableEspBone, "Bone ESP") != CheatToggles.enableEspBone)
                        {
                            CheatToggles.enableEspBone = !CheatToggles.enableEspBone;
                            MelonLogger.Log("[+] Bone ESP: Toggled " + (CheatToggles.enableEspBone ? "On" : "Off"));

                        }
                        if (GUI.Toggle(new Rect(650f, 62f, 150f, 20f), CheatToggles.enableEspOuija, "Ouija ESP") != CheatToggles.enableEspOuija)
                        {
                            CheatToggles.enableEspOuija = !CheatToggles.enableEspOuija;
                            MelonLogger.Log("[+] Ouija ESP: Toggled " + (CheatToggles.enableEspOuija ? "On" : "Off"));

                        }
                        if (GUI.Toggle(new Rect(650f, 82f, 150f, 20f), CheatToggles.enableEspFuseBox, "FuseBox ESP") != CheatToggles.enableEspFuseBox)
                        {
                            CheatToggles.enableEspFuseBox = !CheatToggles.enableEspFuseBox;
                            MelonLogger.Log("[+] FuseBox ESP: Toggled " + (CheatToggles.enableEspFuseBox ? "On" : "Off"));
                        }
                        if (GUI.Toggle(new Rect(650f, 102f, 150f, 20f), CheatToggles.enableEspEmf, "Emf ESP") != CheatToggles.enableEspEmf)
                        {
                            CheatToggles.enableEspEmf = !CheatToggles.enableEspEmf;
                            MelonLogger.Log("[+] Emf ESP: Toggled " + (CheatToggles.enableEspEmf ? "On" : "Off"));
                        }
                    }
                    if (GUI.Toggle(new Rect(500f, 42f, 150f, 20f), CheatToggles.guiHelper, "Helper GUI") != CheatToggles.guiHelper)
                    {
                        CheatToggles.guiHelper = !CheatToggles.guiHelper;
                        CheatToggles.guiGhost = false;
                        CheatToggles.guiGhostTroll = false;
                        CheatToggles.guiESP = false;
                        CheatToggles.guiHelperInfo = false;
                        CheatToggles.guiTroll = false;
                        CheatToggles.guiDebug = false;
                        CheatToggles.guiTest = false;
                        CheatToggles.guiFeatureCollection = false;
                    }
                    if (CheatToggles.guiHelper == true)
                    {
                        if (GUI.Toggle(new Rect(650f, 2f, 150f, 20f), CheatToggles.guiHelperInfo, "Basic Info") != CheatToggles.guiHelperInfo)
                        {
                            CheatToggles.guiHelperInfo = !CheatToggles.guiHelperInfo;
                        }
                        if (CheatToggles.guiHelperInfo == true)
                        {
                            if (GUI.Toggle(new Rect(800f, 2f, 150f, 20f), CheatToggles.enableBIGhost, "Ghost Info") != CheatToggles.enableBIGhost)
                            {
                                CheatToggles.enableBIGhost = !CheatToggles.enableBIGhost;
                                MelonLogger.Log("[+] Ghost Info: Toggled " + (CheatToggles.enableBIGhost ? "On" : "Off"));
                            }
                            if (GUI.Toggle(new Rect(800f, 22f, 150f, 20f), CheatToggles.enableBIMissions, "Missions Info") != CheatToggles.enableBIMissions)
                            {
                                CheatToggles.enableBIMissions = !CheatToggles.enableBIMissions;
                                MelonLogger.Log("[+] Missions Info: Toggled " + (CheatToggles.enableBIMissions ? "On" : "Off"));
                            }
                            if (GUI.Toggle(new Rect(800f, 42f, 150f, 20f), CheatToggles.enableBIPlayer, "Player Info") != CheatToggles.enableBIPlayer)
                            {
                                CheatToggles.enableBIPlayer = !CheatToggles.enableBIPlayer;
                                MelonLogger.Log("[+] Player Info: Toggled " + (CheatToggles.enableBIPlayer ? "On" : "Off"));
                            }
                        }
                        if (GUI.Toggle(new Rect(650f, 22f, 150f, 20f), CheatToggles.enableFullbright, "Enable Fullbright") != CheatToggles.enableFullbright)
                        {
                            CheatToggles.enableFullbright = !CheatToggles.enableFullbright;
                            MelonLogger.Log("[+] Fullbright: Toggled " + (CheatToggles.enableFullbright ? "On" : "Off"));
                            if (CheatToggles.enableFullbright)
                            {
                                Fullbright.Enable();
                            }
                            else
                            {
                                Fullbright.Disable();
                            }
                        }
                        if (GUI.Toggle(new Rect(650f, 42f, 150f, 20f), CheatToggles.enableHotkeys, "Enable Troll Hotkeys") != CheatToggles.enableHotkeys)
                        {
                            CheatToggles.enableHotkeys = !CheatToggles.enableHotkeys;
                            MelonLogger.Log("[+] Troll Hotkeys: Toggled " + (CheatToggles.enableHotkeys ? "On" : "Off"));
                        }
                    }
                    if (GUI.Toggle(new Rect(500f, 62f, 150f, 20f), CheatToggles.guiTroll, "Troll GUI") != CheatToggles.guiTroll)
                    {
                        CheatToggles.guiTroll = !CheatToggles.guiTroll;
                        CheatToggles.guiGhost = false;
                        CheatToggles.guiGhostTroll = false;
                        CheatToggles.guiESP = false;
                        CheatToggles.guiHelper = false;
                        CheatToggles.guiHelperInfo = false;
                        CheatToggles.guiDebug = false;
                        CheatToggles.guiTest = false;
                        CheatToggles.guiFeatureCollection = false;
                    }
                    if (CheatToggles.guiTroll == true)
                    {
                        if (GUI.Button(new Rect(650f, 2f, 150f, 20f), "Lock Exit Doors") && levelController != null)
                        {
                            Trolling.LockDoors(1);
                        }
                        if (GUI.Button(new Rect(650f, 22f, 150f, 20f), "Lock All Doors") && levelController != null)
                        {
                            Trolling.LockDoors(2);
                        }
                        if (GUI.Button(new Rect(650f, 42f, 150f, 20f), "Unlock Exit Doors") && levelController != null)
                        {
                            Trolling.LockDoors(3);
                        }
                        if (GUI.Button(new Rect(650f, 62f, 150f, 20f), "Unlock All Doors") && levelController != null)
                        {
                            Trolling.LockDoors(4);
                        }
                        if (GUI.Button(new Rect(650f, 82f, 150f, 20f), "Door knock") && levelController != null)
                        {
                            Trolling.EventDoorKnock();
                        }
                        if (GUI.Button(new Rect(650f, 102f, 150f, 20f), "Random Event") && levelController != null)
                        {
                            Trolling.Interact();
                        }
                    }
                    if (GUI.Toggle(new Rect(500f, 82f, 150f, 20f), CheatToggles.guiDebug, "Debug GUI") != CheatToggles.guiDebug)
                    {
                        CheatToggles.guiDebug = !CheatToggles.guiDebug;
                        CheatToggles.guiGhost = false;
                        CheatToggles.guiGhostTroll = false;
                        CheatToggles.guiESP = false;
                        CheatToggles.guiHelper = false;
                        CheatToggles.guiHelperInfo = false;
                        CheatToggles.guiTroll = false;
                        CheatToggles.guiTest = false;
                        CheatToggles.guiFeatureCollection = false;
                    }
                    if (CheatToggles.guiDebug == true)
                    {
                        if (GUI.Toggle(new Rect(650f, 2f, 150f, 20f), CheatToggles.enableDebug, "Enable Debug") != CheatToggles.enableDebug)
                        {
                            CheatToggles.enableDebug = !CheatToggles.enableDebug;
                            MelonLogger.Log("[+] Debug: Toggled " + (CheatToggles.enableDebug ? "On" : "Off"));
                        }
                    }
                    if (GUI.Toggle(new Rect(500f, 102f, 150f, 20f), CheatToggles.guiTest, "New Features") != CheatToggles.guiTest)
                    {
                        CheatToggles.guiTest = !CheatToggles.guiTest;
                        CheatToggles.guiGhost = false;
                        CheatToggles.guiGhostTroll = false;
                        CheatToggles.guiESP = false;
                        CheatToggles.guiHelper = false;
                        CheatToggles.guiHelperInfo = false;
                        CheatToggles.guiTroll = false;
                        CheatToggles.guiDebug = false;
                        CheatToggles.guiFeatureCollection = false;
                    }
                    if (CheatToggles.guiTest == true)
                    {
                        if (GUI.Button(new Rect(650f, 2f, 150f, 20f), "Random Light Use") && levelController != null)
                        {
                            LightSwitch lightSwitchr = lightSwitches[new System.Random().Next(0, lightSwitches.Count)];
                            if (lightSwitchr != null)
                            {
                                lightSwitchr.UseLight();
                            }
                            MelonLogger.Log("[+] Random Light Use");
                        }
                        if (GUI.Button(new Rect(650f, 22f, 150f, 20f), "All Lights On") && levelController != null)
                        {
                            foreach (LightSwitch lightSwitchaon in lightSwitches)
                            {
                                lightSwitchaon.TurnOn(true);
                                lightSwitchaon.TurnOnNetworked(true);
                            }
                            MelonLogger.Log("[+] All Lights On");
                        }
                        if (GUI.Button(new Rect(650f, 42f, 150f, 20f), "All Lights Off") && levelController != null)
                        {
                            foreach (LightSwitch lightSwitchaoff in lightSwitches)
                            {
                                lightSwitchaoff.TurnOff();
                                lightSwitchaoff.TurnOffNetworked(true);
                            }
                            MelonLogger.Log("[+] All Lights Off");
                        }
                        if (GUI.Button(new Rect(650f, 62f, 150f, 20f), "Blinking Lights") && levelController != null)
                        {
                            foreach (LightSwitch lightSwitchsrlb in lightSwitches)
                            {
                                lightSwitchsrlb.field_Public_PhotonView_0.RPC("FlickerNetworked", 0, Trolling.getRPCObject(0, false));
                            }
                            MelonLogger.Log("[+] Blinking Lights ");
                        }
                        if (GUI.Button(new Rect(650f, 82f, 150f, 20f), "Disable All Features") && levelController != null)
                        {
                            DisableAll();
                            MelonLogger.Log("[+] Disable All");
                        }
                    }
                    if (GUI.Toggle(new Rect(500f, 122f, 150f, 20f), CheatToggles.guiFeatureCollection, "Feature Coll. GUI") != CheatToggles.guiFeatureCollection)
                    {
                        CheatToggles.guiFeatureCollection = !CheatToggles.guiFeatureCollection;
                        CheatToggles.guiGhost = false;
                        CheatToggles.guiGhostTroll = false;
                        CheatToggles.guiESP = false;
                        CheatToggles.guiHelper = false;
                        CheatToggles.guiHelperInfo = false;
                        CheatToggles.guiTroll = false;
                        CheatToggles.guiDebug = false;
                        CheatToggles.guiTest = false;
                    }
                    if (CheatToggles.guiFeatureCollection == true)
                    {
                        if (GUI.Button(new Rect(650f, 2f, 150f, 20f), "Hunt") && levelController != null)
                        {
                            Trolling.Hunt();
                        }
                        if (GUI.Button(new Rect(650f, 22f, 150f, 20f), "Idle") && levelController != null)
                        {
                            Trolling.Idle();
                        }
                        if (GUI.Button(new Rect(650f, 42f, 150f, 20f), "Appear") && levelController != null)
                        {
                            Trolling.Appear();
                        }
                        if (GUI.Button(new Rect(650f, 62f, 150f, 20f), "Unappear") && levelController != null)
                        {
                            Trolling.UnAppear();
                        }
                        if (GUI.Button(new Rect(650f, 82f, 150f, 20f), "FuseBox") && levelController != null)
                        {
                            Trolling.FuseBox();
                        }
                        if (GUI.Button(new Rect(800f, 2f, 150f, 20f), "Lock Exit Doors") && levelController != null)
                        {
                            Trolling.LockDoors(1);
                        }
                        if (GUI.Button(new Rect(800f, 22f, 150f, 20f), "Lock All Doors") && levelController != null)
                        {
                            Trolling.LockDoors(2);
                        }
                        if (GUI.Button(new Rect(800f, 42f, 150f, 20f), "Unlock Exit Doors") && levelController != null)
                        {
                            Trolling.LockDoors(3);
                        }
                        if (GUI.Button(new Rect(800f, 62f, 150f, 20f), "Unlock All Doors") && levelController != null)
                        {
                            Trolling.LockDoors(4);
                        }
                        if (GUI.Button(new Rect(800f, 82f, 150f, 20f), "Door knock") && levelController != null)
                        {
                            Trolling.EventDoorKnock();
                        }
                        if (GUI.Button(new Rect(800f, 102f, 150f, 20f), "Random Event") && levelController != null)
                        {
                            Trolling.Interact();
                        }
                        if (GUI.Button(new Rect(950f, 2f, 150f, 20f), "Random Light Use") && levelController != null)
                        {
                            LightSwitch lightSwitchr = lightSwitches[new System.Random().Next(0, lightSwitches.Count)];
                            if (lightSwitchr != null)
                            {
                                lightSwitchr.UseLight();
                            }
                            MelonLogger.Log("[+] Random Light Use");
                        }
                        if (GUI.Button(new Rect(950f, 22f, 150f, 20f), "All Lights On") && levelController != null)
                        {
                            foreach (LightSwitch lightSwitchaon in lightSwitches)
                            {
                                lightSwitchaon.TurnOn(true);
                                lightSwitchaon.TurnOnNetworked(true);
                            }
                            MelonLogger.Log("[+] All Lights On");
                        }
                        if (GUI.Button(new Rect(950f, 42f, 150f, 20f), "All Lights Off") && levelController != null)
                        {
                            foreach (LightSwitch lightSwitchaoff in lightSwitches)
                            {
                                lightSwitchaoff.TurnOff();
                                lightSwitchaoff.TurnOffNetworked(true);
                            }
                            MelonLogger.Log("[+] All Lights Off");
                        }
                        if (GUI.Button(new Rect(950f, 62f, 150f, 20f), "Blinking Lights") && levelController != null)
                        {
                            foreach (LightSwitch lightSwitchsrlb in lightSwitches)
                            {
                                lightSwitchsrlb.field_Public_PhotonView_0.RPC("FlickerNetworked", 0, Trolling.getRPCObject(0, false));
                            }
                            MelonLogger.Log("[+] Blinking Lights");
                        }
                    }
                }
                else
                {
                    if (initializedScene == 1)
                    {
                        if (GUI.Button(new Rect(500f, 2f, 150f, 20f), "+ 1.000$") && levelController == null)
                        {
                            var money = FileBasedPrefs.GetInt("PlayersMoney", 0);
                            MelonLogger.Log("[+] Money set from " + money + " to " + (money + 1000));
                            FileBasedPrefs.SetInt("PlayersMoney", money + 1000);
                            playerStatsManager.UpdateMoney();
                        }
                        if (GUI.Button(new Rect(500f, 22f, 150f, 20f), "- 1.000$") && levelController == null)
                        {
                            var money = FileBasedPrefs.GetInt("PlayersMoney", 0);
                            MelonLogger.Log("[+] Money set from " + money + " to " + (money - 1000));
                            FileBasedPrefs.SetInt("PlayersMoney", money - 1000);
                            playerStatsManager.UpdateMoney();
                        }
                        if (GUI.Button(new Rect(500f, 42f, 150f, 20f), "+ 100XP") && levelController == null)
                        {
                            FileBasedPrefs.SetInt("myTotalExp", FileBasedPrefs.GetInt("myTotalExp", 0) + 100);
                            playerStatsManager.UpdateExperience();
                            playerStatsManager.UpdateLevel();
                            MelonLogger.Log("[+] XP: +100");
                        }
                        if (GUI.Button(new Rect(500f, 62f, 150f, 20f), "- 1.000XP") && levelController == null)
                        {
                            FileBasedPrefs.SetInt("myTotalExp", FileBasedPrefs.GetInt("myTotalExp", 0) - 1000);
                            playerStatsManager.UpdateExperience();
                            playerStatsManager.UpdateLevel();
                            MelonLogger.Log("[+] XP: -1.000");
                        }
                        GUI.SetNextControlName("changeName");
                        playerName = GUI.TextArea(new Rect(650f, 2f, 150f, 20f), playerName);
                        if (GUI.Button(new Rect(650f, 22f, 150f, 20f), "Change Name"))
                        {
                            GUI.FocusControl("changeName");
                            PhotonNetwork.NickName = playerName;
                            MelonLogger.Log("[+] Set name: " + playerName);
                        }
                    }
                }
            }

            if (CheatToggles.enableEspGhost || CheatToggles.enableEspPlayer || CheatToggles.enableEspBone || CheatToggles.enableEspOuija || CheatToggles.enableEspEmf || CheatToggles.enableEspFuseBox)
            {
                ESP.Enable();
            }

            if (CheatToggles.enableBIGhost || CheatToggles.enableBIMissions || CheatToggles.enableBIPlayer)
            {
                GUI.Label(new Rect(0f, 0f, 500f, 160f), "", "box");
            }
            if (CheatToggles.enableBIGhost)
            {
                BasicInformations.EnableGhost();
                GUI.Label(new Rect(10f, 2f, 300f, 50f), "<color=#00FF00><b>Ghost Name:</b> " + (ghostNameAge ?? "") + "</color>");
                GUI.Label(new Rect(10f, 17f, 300f, 50f), "<color=#00FF00><b>Ghost Type:</b> " + (ghostType ?? "") + "</color>");
                GUI.Label(new Rect(10f, 47f, 400f, 50f), "<color=#00FF00><b>Evidence:</b> " + (evidence ?? "") + "</color>");
                GUI.Label(new Rect(10f, 32f, 300f, 50f), "<color=#00FF00><b>Ghost State:</b> " + (ghostState ?? "") + "</color>");
                GUI.Label(new Rect(10f, 62f, 400f, 50f), "<color=#00FF00><b>Responds to:</b> " + (ghostIsShy ?? "") + "</color>");
            }
            else
            {
                BasicInformations.DisableGhost();
            }
            if (CheatToggles.enableBIMissions)
            {
                BasicInformations.EnableMissions();
            }
            if (CheatToggles.enableBIPlayer)
            {
                BasicInformations.EnablePlayer();
                GUI.Label(new Rect(10f, 77f, 300f, 50f), "<color=#00FF00><b>My Sanity:</b> " + (myPlayerSanity ?? "N/A") + "</color>");
            }
        }
        public override void OnModSettingsApplied()
        {
        }

        private void DisableAll()
        {
            CheatToggles.enableBI = false;
            CheatToggles.enableBIGhost = false;
            CheatToggles.enableBIMissions = false;
            CheatToggles.enableBIPlayer = false;

            CheatToggles.enableEsp = false;
            CheatToggles.enableEspGhost = false;
            CheatToggles.enableEspPlayer = false;
            CheatToggles.enableEspBone = false;
            CheatToggles.enableEspOuija = false;
            CheatToggles.enableEspEmf = false;
            CheatToggles.enableEspFuseBox = false;

            CheatToggles.enableFullbright = false;
            Fullbright.Disable();
        }

        private Player GetLocalPlayer()
        {
            if (players == null || players.Count == 0)
            {
                return null;
            }
            if (players.Count == 1)
            {
                return players[0];
            }
            foreach (Player player in players)
            {
                if (player != null && player.field_Public_PhotonView_0 != null && player.field_Public_PhotonView_0.AmOwner)
                {
                    return player;
                }
            }
            return null;
        }

        IEnumerator CollectGameObjects()
        {
            cameraMain = Camera.main;
            yield return new WaitForSeconds(0.15f);
            Debug.Out("cameraMain");

            dnaEvidences = Object.FindObjectsOfType<DNAEvidence>().ToList<DNAEvidence>() ?? null;
            yield return new WaitForSeconds(0.15f);
            Debug.Out("dnaEvidences");

            doors = Object.FindObjectsOfType<Door>().ToList<Door>() ?? null;
            yield return new WaitForSeconds(0.15f);
            Debug.Out("doors");

            fuseBox = Object.FindObjectOfType<FuseBox>() ?? null;
            yield return new WaitForSeconds(0.15f);
            Debug.Out("fuseBox");

            gameController = Object.FindObjectOfType<GameController>() ?? null;
            yield return new WaitForSeconds(0.15f);
            Debug.Out("gameController");

            ghostAI = Object.FindObjectOfType<GhostAI>() ?? null;
            yield return new WaitForSeconds(0.15f);
            Debug.Out("ghostAI");

            ghostAIs = Object.FindObjectsOfType<GhostAI>().ToList<GhostAI>() ?? null;
            yield return new WaitForSeconds(0.15f);
            Debug.Out("ghostAIs");

            ghostActivity = Object.FindObjectOfType<GhostActivity>() ?? null;
            yield return new WaitForSeconds(0.15f);
            Debug.Out("ghostActivity");

            ghostInfo = Object.FindObjectOfType<GhostInfo>() ?? null;
            yield return new WaitForSeconds(0.15f);
            Debug.Out("ghostInfo");

            levelController = Object.FindObjectOfType<LevelController>() ?? null;
            yield return new WaitForSeconds(0.15f);
            Debug.Out("levelController");

            lightSwitch = Object.FindObjectOfType<LightSwitch>() ?? null;
            yield return new WaitForSeconds(0.15f);
            Debug.Out("lightSwitch");

            lightSwitches = Object.FindObjectsOfType<LightSwitch>().ToList<LightSwitch>() ?? null;
            yield return new WaitForSeconds(0.15f);
            Debug.Out("lightSwitches");

            soundController = Object.FindObjectOfType<SoundController>() ?? null;
            yield return new WaitForSeconds(0.15f);
            Debug.Out("soundController");

            ouijaBoards = Object.FindObjectsOfType<OuijaBoard>().ToList<OuijaBoard>() ?? null;
            yield return new WaitForSeconds(0.15f);
            Debug.Out("ouijaBoards");

            windows = Object.FindObjectsOfType<Window>().ToList<Window>() ?? null;
            yield return new WaitForSeconds(0.15f);
            Debug.Out("ouijaBoards");

            if (Object.FindObjectOfType<Player>() != null)
            {
                player = Object.FindObjectOfType<Player>() ?? null;
                yield return new WaitForSeconds(0.15f);
                Debug.Out("player");

                players = Object.FindObjectsOfType<Player>().ToList<Player>() ?? null;
                yield return new WaitForSeconds(0.15f);
                Debug.Out("players");

                playerStatsManager = Object.FindObjectOfType<PlayerStatsManager>() ?? null;
                yield return new WaitForSeconds(0.15f);
                Debug.Out("playerStatsManager");

                myPlayer = GetLocalPlayer() ?? player;
                Debug.Out("myPlayer");
                yield return new WaitForSeconds(0.15f);

                playerAnim = myPlayer.field_Public_Animator_0 ?? null;
                yield return new WaitForSeconds(0.15f);
                Debug.Out("playerAnim");

                if (playerAnim != null)
                {
                    boneTransform = playerAnim.GetBoneTransform(HumanBodyBones.Head) ?? null;
                    yield return new WaitForSeconds(0.15f);
                    Debug.Out("boneTransform");
                }
            }

            if (levelController != null)
            {
                photonView = ghostAI.field_Public_PhotonView_0 ?? null;
                yield return new WaitForSeconds(0.15f);
                Debug.Out("photonView");

                emf = Object.FindObjectsOfType<EMF>().ToList<EMF>() ?? null;
                yield return new WaitForSeconds(0.15f);
                Debug.Out("emf");

                //emfData = Object.FindObjectOfType<EMFData>() ?? null;
                //yield return new WaitForSeconds(0.15f);
                //Debug.Out("emfData");
            }

            serverManager = Object.FindObjectOfType<ServerManager>() ?? null;
            yield return new WaitForSeconds(0.15f);
            Debug.Out("serverManager");

            Debug.Out("-----------------------------");
            yield return null;
        }

        public static Transform boneTransform;
        public static Camera cameraMain;
        public static List<DNAEvidence> dnaEvidences;
        public static List<Door> doors;
        public static GameController gameController;
        public static GhostAI ghostAI;
        public static List<GhostAI> ghostAIs;
        public static List<EMF> emf;
        public static EMFData emfData;
        public static FuseBox fuseBox;
        public static GhostActivity ghostActivity;
        public static GhostInteraction ghostInteraction;
        public static GhostController ghostController;
        public static GhostEventPlayer ghostEventPlayer;
        public static GhostInfo ghostInfo;
        public static List<InventoryItem> items;
        public static LevelController levelController;
        public static Light light;
        public static LightSwitch lightSwitch;
        public static List<LightSwitch> lightSwitches;
        public static Player myPlayer;
        public static List<OuijaBoard> ouijaBoards;
        public static PhotonView photonView;
        public static Player player;
        public static List<Player> players;
        public static Animator playerAnim;
        public static PlayerStatsManager playerStatsManager;
        public static ServerManager serverManager;
        public static SoundController soundController;
        public static List<Window> windows;
        public static String ghostNameAge;
        public static String ghostType;
        public static String evidence;
        public static String ghostState;
        public static String ghostIsShy;
        public static String myPlayerSanity;
        public static String[] mapNames = { "Opening Scene", "Lobby", "Tanglewood Street", "Ridgeview Road House", "Edgefield Street House", "Asylum", "Brownstone High School", "Bleasdale Farmhouse", "Grafton Farmhouse", "Prison" };
        public static String inSight = "";
        public static int initializedScene;
        private static bool gameStarted = false;
        private static object coRoutine;
        private static bool canRun = true;
        private static bool isRunning = true;
        private static String playerName;
    }
}
