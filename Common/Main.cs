using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MelonLoader;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using Console = System.Console;
using String = System.String;

namespace PhasmoMelonMod
{
    public class Main : MelonMod
    {
        public override void OnApplicationStart()
        {
            Console.Title = string.Format("Phasmophobia");
            MelonLogger.Log("[+] Set console title to: Phasmophobia");
        }
        public override void OnLevelWasLoaded(int level)
        {
            MelonCoroutines.Stop(coRoutine);
        }
        public override void OnLevelWasInitialized(int level)
        {
            if (level == 1 && firstRun)
            {
                new Thread(() =>
                {
                    while (true)
                    {
                        coRoutine = MelonCoroutines.Start(CollectGameObjects());
                        Thread.Sleep(5000);
                    }
                }).Start();
                firstRun = false;
            }
        }
        public override void OnUpdate()
        {
            Keyboard keyboard = Keyboard.current;

            if (keyboard.leftArrowKey.wasPressedThisFrame)
            {
                CheatToggles.enableBasicInformations = !CheatToggles.enableBasicInformations;
                MelonLogger.Log("[+] Basic informations: Toggled " + (CheatToggles.enableBasicInformations ? "On" : "Off"));
            }

            if (keyboard.upArrowKey.wasPressedThisFrame)
            {
                CheatToggles.enableEsp = !CheatToggles.enableEsp;
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
        }
        public override void OnGUI()
        {
            GUI.Label(new Rect(10f, 160f, 100f, 50f), "<b><color=#A302B5>ESP:</color> " + (CheatToggles.enableEsp ? "<color=#00C403>On" : "<color=#C40000>Off") + " </color></b>");
            GUI.Label(new Rect(10f, 175f, 100f, 50f), "<b><color=#A302B5>Fullbright:</color> " + (CheatToggles.enableFullbright ? "<color=#00C403>On" : "<color=#C40000>Off") + " </color></b>");
            GUI.Label(new Rect(10f, 190f, 100f, 50f), "<b><color=#A302B5>Basic Info:</color> " + (CheatToggles.enableBasicInformations ? "<color=#00C403>On" : "<color=#C40000>Off") + " </color></b>");

            if (CheatToggles.enableBasicInformations)
            {
                BasicInformations.Enable();

                GUI.Label(new Rect(0f, 0f, 500f, 160f), "", "box");
                GUI.Label(new Rect(10f, 2f, 300f, 50f), "<color=#00FF00><b>Ghost Name:</b> " + (ghostNameAge ?? "N/A") + "</color>");
                GUI.Label(new Rect(10f, 17f, 300f, 50f), "<color=#00FF00><b>Ghost Type:</b> " + (ghostType ?? "N/A") + "</color>");
                GUI.Label(new Rect(10f, 47f, 400f, 50f), "<color=#00FF00><b>Evidence:</b> " + (evidence ?? "N/A") + "</color>");
                GUI.Label(new Rect(10f, 32f, 300f, 50f), "<color=#00FF00><b>Ghost State:</b> " + (ghostState ?? "N/A") + "</color>");
                GUI.Label(new Rect(10f, 62f, 400f, 50f), "<color=#00FF00><b>Responds to:</b> " + (ghostIsShy ?? "N/A") + "</color>");
                GUI.Label(new Rect(10f, 77f, 300f, 50f), "<color=#00FF00><b>My Sanity:</b> " + (myPlayerSanity ?? "N/A") + "</color>");
            }

            if (CheatToggles.enableEsp)
                ESP.Enable();
        }
        public override void OnModSettingsApplied()
        {
        }

        IEnumerator CollectGameObjects()
        {
            cameraMain = Camera.main;
            yield return new WaitForSeconds(0.15f);

            dnaEvidences = Object.FindObjectsOfType<DNAEvidence>().ToList<DNAEvidence>();
            yield return new WaitForSeconds(0.15f);

            gameController = Object.FindObjectOfType<GameController>();
            yield return new WaitForSeconds(0.15f);

            ghostAI = Object.FindObjectOfType<GhostAI>();
            yield return new WaitForSeconds(0.15f);

            ghostAIs = Object.FindObjectsOfType<GhostAI>().ToList<GhostAI>();
            yield return new WaitForSeconds(0.15f);

            ghostInfo = Object.FindObjectOfType<GhostInfo>();
            yield return new WaitForSeconds(0.15f);

            ouijaBoards = Object.FindObjectsOfType<OuijaBoard>().ToList<OuijaBoard>();
            yield return new WaitForSeconds(0.15f);

            player = Object.FindObjectOfType<Player>() ?? null;
            yield return new WaitForSeconds(0.15f);

            playerAnim = player.field_Public_Animator_0 ?? null;
            yield return new WaitForSeconds(0.15f);

            boneTransform = playerAnim.GetBoneTransform(HumanBodyBones.Head) ?? null;
            yield return new WaitForSeconds(0.15f);

            light = boneTransform.GetComponent<Light>() ?? null;
            yield return new WaitForSeconds(0.15f);
            

            yield return null;
        }

        public static Transform boneTransform;
        public static Camera cameraMain;
        public static List<DNAEvidence> dnaEvidences;
        public static GameController gameController;
        public static GhostAI ghostAI;
        public static List<GhostAI> ghostAIs;
        public static GhostController ghostController;
        public static GhostEventPlayer ghostEventPlayer;
        public static GhostInfo ghostInfo;
        public static Light light;
        public static List<OuijaBoard> ouijaBoards;
        public static PhotonView photonView;
        public static Player player;
        public static Animator playerAnim;
        public static String ghostNameAge;
        public static String ghostType;
        public static String evidence;
        public static String ghostState;
        public static String ghostIsShy;
        public static String myPlayerSanity;
        private static object coRoutine;
        private static bool firstRun = true;
    }
}
