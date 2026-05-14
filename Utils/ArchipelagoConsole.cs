using System.Collections.Generic;
using System.Linq;
using BepInEx;
using GnosiaArchipelagoRandomizer.Archipelago;
using UnityEngine;
using Rewired;
using UnityEngine.UIElements.UIR;
using GnosiaArchipelagoRandomizer.Utils.ConsoleCommands;

namespace GnosiaArchipelagoRandomizer.Utils
{
    // shamelessly stolen from oc2-modding https://github.com/toasterparty/oc2-modding/blob/main/OC2Modding/GameLog.cs
    public static class ArchipelagoConsole
    {
        public static bool Hidden = true;

        private static List<string> logLines = new();
        private static Vector2 scrollView;
        private static Rect window;
        private static Rect scroll;
        private static Rect text;
        private static Rect hideShowButton;

        private static GUIStyle textStyle = new();
        private static string scrollText = "";
        private static float lastUpdateTime = Time.time;
        private const int MaxLogLines = 80;
        private const float HideTimeout = 15f;

        private static string CommandText = "!help";
        public static Rect CommandTextRect;
        private static Rect SendCommandButton;

        private static Player player;

        public static bool blockMouseAndKeyboard = false;

        private static readonly List<string> commandHistory = new List<string>();
        private static int historyIndex = -1;

        public static void Awake()
        {
            UpdateWindow();
            BuiltInCommands.RegisterAll();
        }

        public static void Update()
        {
            if (!Hidden)
            {
                player = ReInput.players.GetPlayer(0);
                var keyboard = player.controllers.GetController<Keyboard>(0);

                Plugin.BepinLogger.LogInfo($"{keyboard}");

                if (keyboard.GetKeyDown(KeyCode.UpArrow))
                {
                    NavigateHistory(-1);
                }
                if (keyboard.GetKeyDown(KeyCode.DownArrow))
                {
                    NavigateHistory(1);
                }
            }
        }

        public static void LogMessage(string message)
        {
            if (message.IsNullOrWhiteSpace()) return;

            if (logLines.Count == MaxLogLines)
            {
                logLines.RemoveAt(0);
            }
            logLines.Add(message);
            Plugin.BepinLogger.LogMessage(message);
            lastUpdateTime = Time.time;
            UpdateWindow();
        }

        public static void BlockMouseInputs()
        {
            player = ReInput.players.GetPlayer(0);

            // Disable mouse inputs
            var mouseController = player.controllers.GetController(ControllerType.Mouse, 0);

            if (mouseController != null)
                mouseController.enabled = false;
        }

        public static void BlockKeyboardInputs()
        {
            player = ReInput.players.GetPlayer(0);

            //Disable keyboard inputs
            var keyboardController = player.controllers.GetController(ControllerType.Keyboard, 0);

            if (keyboardController != null)
                keyboardController.enabled = false;
        }

        public static void UnBlockKeyboardInputs()
        {
            player = ReInput.players.GetPlayer(0);

            //Re-enable keyboard inputs
            var keyboardController = player.controllers.GetController(ControllerType.Keyboard, 0);

            if (keyboardController != null)
                keyboardController.enabled = true;
        }

        private static void NavigateHistory(int direction)
        {
            if (commandHistory.Count == 0)
            {
                return;
            }

            historyIndex += direction;

            if (historyIndex < 0)
            {
                historyIndex = 0;
            }

            if (historyIndex >= commandHistory.Count)
            {
                historyIndex = commandHistory.Count;
                CommandText = "";
                return;
            }

            CommandText = commandHistory[historyIndex];
        }

        public static void OnGUI()
        {
            if (blockMouseAndKeyboard)
                BlockMouseInputs();

            if (logLines.Count == 0) return;

            if (!Hidden || Time.time - lastUpdateTime < HideTimeout)
            {
                scrollView = GUI.BeginScrollView(window, scrollView, scroll);
                GUI.Box(text, "");
                GUI.Box(text, scrollText, textStyle);
                GUI.EndScrollView();
            }

            if (GUI.Button(hideShowButton, Hidden ? "Show" : "Hide"))
            {
                Hidden = !Hidden;
                if (Hidden || !blockMouseAndKeyboard)
                    UnBlockKeyboardInputs();
                else
                    BlockKeyboardInputs();
                UpdateWindow();
            }

            // draw client/server commands entry
            if (Hidden || !ArchipelagoClient.Authenticated) return;

            CommandText = GUI.TextField(CommandTextRect, CommandText);
            if (!CommandText.IsNullOrWhiteSpace() && GUI.Button(SendCommandButton, "Send"))
            {
                commandHistory.Add(CommandText);
                historyIndex = commandHistory.Count;

                if (CommandRegistry.TryExecute(CommandText, out var result))
                {
                    LogMessage(result.Message);
                }
                else
                {
                    Plugin.ArchipelagoClient.SendMessage(CommandText);
                }
                CommandText = "";
            }
        }
        public static void UpdateWindow()
        {
            scrollText = "";

            if (Hidden)
            {
                if (logLines.Count > 0)
                {
                    scrollText = logLines[logLines.Count - 1];
                }
            }
            else
            {
                for (var i = 0; i < logLines.Count; i++)
                {
                    scrollText += "> ";
                    scrollText += logLines.ElementAt(i);
                    if (i < logLines.Count - 1)
                    {
                        scrollText += "\n\n";
                    }
                }
            }

            var width = (int)(Screen.width * 0.4f);
            int height;
            int scrollDepth;
            if (Hidden)
            {
                height = (int)(Screen.height * 0.03f);
                scrollDepth = height;
            }
            else
            {
                height = (int)(Screen.height * 0.3f);
                scrollDepth = height * 10;
            }

            window = new Rect(Screen.width / 2 - width / 2, 0, width, height);
            scroll = new Rect(0, 0, width * 0.9f, scrollDepth);
            scrollView = new Vector2(0, scrollDepth);
            text = new Rect(0, 0, width, scrollDepth);

            textStyle.alignment = TextAnchor.LowerLeft;
            textStyle.fontSize = Hidden ? (int)(Screen.height * 0.0165f) : (int)(Screen.height * 0.0185f);
            textStyle.normal.textColor = Color.white;
            textStyle.wordWrap = !Hidden;

            var xPadding = (int)(Screen.width * 0.01f);
            var yPadding = (int)(Screen.height * 0.01f);

            textStyle.padding = Hidden
                ? new RectOffset(xPadding / 2, xPadding / 2, yPadding / 2, yPadding / 2)
                : new RectOffset(xPadding, xPadding, yPadding, yPadding);

            var buttonWidth = (int)(Screen.width * 0.12f);
            var buttonHeight = (int)(Screen.height * 0.03f);

            hideShowButton = new Rect(Screen.width / 2 + width / 2 + buttonWidth / 3, Screen.height * 0.004f, buttonWidth,
                buttonHeight);

            // draw server command text field and button
            width = (int)(Screen.width * 0.4f);
            var xPos = (int)(Screen.width / 2.0f - width / 2.0f);
            var yPos = (int)(Screen.height * 0.307f);
            height = (int)(Screen.height * 0.022f);

            CommandTextRect = new Rect(xPos, yPos, width, height);

            width = (int)(Screen.width * 0.035f);
            yPos += (int)(Screen.height * 0.03f);
            SendCommandButton = new Rect(xPos, yPos, width, height);
        }
    }
}