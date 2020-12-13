using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;
using Util;

namespace Debug
{
    public class ZDebugger
    {
        private static GameObject _consoleElement;
        private static GameObject _statElement;
        
        private static Text _stat;
        private static Text _output;
        private static InputField _input;
        private static readonly List<string> Logs = new List<string>();
        private static readonly List<string> History = new List<string>();
        private static int _historyId;
        
        public static int MaxLines = 12;
        public static bool IsConsoleOpen { get; private set; }

        private static readonly Dictionary<string, Func<string>> CommandList_0 = new Dictionary<string, Func<string>>();
        private static readonly Dictionary<string, Func<string, string>> CommandList_1 = new Dictionary<string, Func<string, string>>();
        
        public static void InitConsole()
        {
            var canvas = GameObject.Find("Canvas");
            
            _consoleElement = new GameObject();
            _consoleElement.transform.SetParent(canvas.transform);
            
            _consoleElement.name = "ZDebugger_Console";
            _consoleElement.AddComponent<RectTransform>();
            _consoleElement.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            _consoleElement.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            _consoleElement.GetComponent<RectTransform>().anchoredPosition = new Vector3(1280, -250, 0);
            _consoleElement.GetComponent<RectTransform>().sizeDelta = new Vector2(2560, 500);
            _consoleElement.AddComponent<Image>();
            _consoleElement.GetComponent<Image>().color = new Color(0, 0, 0, 0.8f);

            var output = new GameObject {name = "Output"};
            output.transform.SetParent(_consoleElement.transform);
            
            output.AddComponent<RectTransform>();
            output.GetComponent<RectTransform>().anchoredPosition = new Vector2(-2, 26);
            output.GetComponent<RectTransform>().sizeDelta = new Vector2(2530, 422);
            output.AddComponent<Text>();
            output.GetComponent<Text>().color = Color.white;
            output.GetComponent<Text>().fontSize = 30;
            output.GetComponent<Text>().supportRichText = true;
            output.GetComponent<Text>().alignment = TextAnchor.LowerLeft;
            output.GetComponent<Text>().font = Font.CreateDynamicFontFromOSFont("Consolas", 30);
            
            _output = output.GetComponent<Text>();
            
            var input = new GameObject { name = "Input" };
            input.transform.SetParent(_consoleElement.transform);

            input.AddComponent<RectTransform>();
            input.GetComponent<RectTransform>().anchoredPosition = new Vector2(-2, -216);
            input.GetComponent<RectTransform>().sizeDelta = new Vector2(2530, 42);
            input.AddComponent<InputField>();
            input.GetComponent<InputField>().transition = Selectable.Transition.None;

            var inputText = new GameObject {name = "Text"};
            inputText.transform.SetParent(input.transform);
            inputText.AddComponent<RectTransform>();
            inputText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            inputText.GetComponent<RectTransform>().sizeDelta = new Vector2(2530, 42);
            
            inputText.AddComponent<Text>();
            inputText.GetComponent<Text>().color = Color.white;
            inputText.GetComponent<Text>().fontSize = 30;
            inputText.GetComponent<Text>().supportRichText = false;
            inputText.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
            inputText.GetComponent<Text>().font = Font.CreateDynamicFontFromOSFont("Consolas", 30);
            
            input.GetComponent<InputField>().textComponent = inputText.GetComponent<Text>();
            
            _input = input.GetComponent<InputField>();

            _statElement = new GameObject {name = "ZDebugger_Stat"};
            _statElement.transform.SetParent(canvas.transform);
            _statElement.AddComponent<RectTransform>();
            _statElement.GetComponent<RectTransform>().anchorMin = new Vector2(1, 1);
            _statElement.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            _statElement.GetComponent<RectTransform>().anchoredPosition = new Vector3(-195, -145, 0);
            _statElement.GetComponent<RectTransform>().sizeDelta = new Vector2(340, 240);
            _statElement.AddComponent<Image>();
            _statElement.GetComponent<Image>().color = new Color(0, 0, 0, 0.8f);

            var statText = new GameObject {name = "Stat"};
            statText.transform.SetParent(_statElement.transform);
            statText.AddComponent<RectTransform>();
            statText.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            statText.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 200);

            statText.AddComponent<Text>();
            statText.GetComponent<Text>().color = Color.white;
            statText.GetComponent<Text>().fontSize = 30;
            statText.GetComponent<Text>().font = Font.CreateDynamicFontFromOSFont("Consolas", 30);
            
            _stat = statText.GetComponent<Text>();
            
            _input.onEndEdit.AddListener(text => {
                if (text.Trim() == "") return;
                
                // Store cmd to history
                History.Add(text);
                _historyId = History.Count;
                
                var cmd = text.Split(' ');
                try
                {
                    if (!HasCommand(cmd[0], cmd.Length - 1))
                    {
                        Log(text);
                        Error($"Command `{cmd[0]}()'{cmd.Length - 1}` not found!");
                    }
                    else
                    {
                        var result = "";
                        if (cmd.Length == 1) result = ExecuteCommand(cmd[0]);
                        if (cmd.Length == 2) result = ExecuteCommand(cmd[0], cmd[1]);
                        Log(text);
                        if (!string.IsNullOrEmpty(result)) Log(result);
                    }
                }
                catch (Exception e)
                {
                    Log(text);
                    Error(e);
                }
                
                _input.text = "";
                Focus();
            });

            InitStdCommands();
            
            HideConsole();
            HideStat();
        }

        private static void InitStdCommands()
        {
            SetCommand("show", item =>
            {
                switch (item)
                {
                    case "stat":
                        ShowStat();
                        return "";
                    default:
                        throw new Exception("Unknown arg");
                        break;
                }
            });
            
            SetCommand("hide", item =>
            {
                switch (item)
                {
                    case "stat":
                        HideStat();
                        return "";
                    default:
                        throw new Exception("Unknown arg");
                        break;
                }
            });
        }
        
        public static void SetCommand(string command, Func<string> action)
        {
            CommandList_0.Add(command, action);
        }
        
        public static void SetCommand(string command, Func<string, string> action)
        {
            CommandList_1.Add(command, action);
        }

        public static bool HasCommand(string command, int argsLength)
        {
            if (CommandList_0.ContainsKey(command) && argsLength == 0) return true;
            if (CommandList_1.ContainsKey(command) && argsLength == 1) return true;
            return false;
        }
        
        public static string ExecuteCommand(string command)
        {
            return CommandList_0.TryGetValue(command, out var cmd) ? cmd?.Invoke() : null;
        }
        
        public static string ExecuteCommand(string command, string arg0)
        {
            return CommandList_1.TryGetValue(command, out var cmd) ? cmd?.Invoke(arg0) : null;
        }

        public static void HideConsole()
        {
            _consoleElement.SetActive(false);
        }

        public static void ShowConsole()
        {
            _consoleElement.SetActive(true);
        }
        
        public static void HideStat()
        {
            _statElement.SetActive(false);
        }

        public static void ShowStat()
        {
            _statElement.SetActive(true);
        }
        
        public static void ToggleConsole()
        {
            _input.text = "";
            
            IsConsoleOpen = !IsConsoleOpen;
            if (IsConsoleOpen) ShowConsole();
            else HideConsole();
            
            DrawConsole();
            Focus();
        }

        private static void Focus()
        {
            if (IsConsoleOpen)
            {
                _input.Select();
                _input.ActivateInputField();
            }
        }
        
        public static void Error(object data)
        {
            Logs.Add("<color=red>" + data + "</color>");
            DrawConsole();
        }
        
        public static void Log(object data)
        {
            Logs.Add(data.ToString());
            DrawConsole();
        }

        private static void DrawConsole()
        {
            var outText = "";

            for (var i = Math.Max(0, Logs.Count - MaxLines); i < Logs.Count; i++)
            {
                try
                {
                    outText += $"{Logs[i]}";
                    if (i != Logs.Count - 1) outText += "\n";
                }
                catch
                {
                    // ignored
                }
            }

            _output.text = outText;
        }

        public static void Update()
        {
            if (IsConsoleOpen && Input.GetKeyDown(KeyCode.UpArrow))
            {
                _historyId -= 1;
                if (_historyId <= 0) _historyId = 0;
                
                _input.text = History[_historyId];
            }
            if (IsConsoleOpen && Input.GetKeyDown(KeyCode.DownArrow))
            {
                _historyId += 1;
                if (_historyId > History.Count - 1) _historyId = History.Count - 1;
                
                _input.text = History[_historyId];
            }

            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                _historyId = History.Count;
                ToggleConsole();
            }

            if (Time.frameCount % 30 == 0)
            {
                _stat.text = $"FPS: {Math.Round(1.0f / Time.smoothDeltaTime)}\nRAM: {Profiler.GetTotalAllocatedMemoryLong() / 1048576f:##.##} MB\nRES: {Screen.width}x{Screen.height}";
            }
        }
    }
}