using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;
using Util;

namespace Debug
{
    public class ConsoleCommand : Attribute
    {
        public ConsoleCommand(string name)
        {
            UnityEngine.Debug.Log(name);
        }
    }
    
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
        private static int _consoleOffsetY;
        
        public static int MaxLines = 12;
        public static bool IsConsoleOpen { get; private set; }

        private static readonly Dictionary<string, Func<string>> CommandList_0 = new Dictionary<string, Func<string>>();
        private static readonly Dictionary<string, Func<string, string>> CommandList_1 = new Dictionary<string, Func<string, string>>();
        private static readonly Dictionary<string, Func<string, string, string>> CommandList_2 = new Dictionary<string, Func<string, string, string>>();

        public static bool IsShowStatByDefault;
        public static readonly Dictionary<string, string> CommandDescription = new Dictionary<string, string>();
        
        public static void InitConsole()
        {
            var canvas = new GameObject{ name = "ZDebugger_Canvas" };
            // canvas.AddComponent<RectTransform>();
            canvas.AddComponent<Canvas>();
            canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            
            canvas.AddComponent<CanvasScaler>();
            canvas.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvas.GetComponent<CanvasScaler>().referenceResolution = new Vector2(2560, 1440);
            canvas.AddComponent<GraphicRaycaster>();
            
            _consoleElement = new GameObject();
            _consoleElement.transform.SetParent(canvas.transform);
            
            _consoleElement.name = "ZDebugger_Console";
            _consoleElement.AddComponent<RectTransform>();
            _consoleElement.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            _consoleElement.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            _consoleElement.GetComponent<RectTransform>().anchoredPosition = new Vector3(1280, -250, 0);
            _consoleElement.GetComponent<RectTransform>().sizeDelta = new Vector2(2560, 500);
            _consoleElement.AddComponent<Image>();
            _consoleElement.GetComponent<Image>().color = new Color(0, 0, 0, 0.9f);

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
            output.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Overflow;
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
            _statElement.GetComponent<RectTransform>().anchoredPosition = new Vector3(-195, -250, 0);
            _statElement.GetComponent<RectTransform>().sizeDelta = new Vector2(340, 440);
            _statElement.AddComponent<Image>();
            _statElement.GetComponent<Image>().color = new Color(0, 0, 0, 0.8f);

            var statText = new GameObject {name = "Stat"};
            statText.transform.SetParent(_statElement.transform);
            statText.AddComponent<RectTransform>();
            statText.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            statText.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 410);

            statText.AddComponent<Text>();
            statText.GetComponent<Text>().color = Color.white;
            statText.GetComponent<Text>().fontSize = 30;
            statText.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Overflow;
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
                        Error($"Command `{cmd[0]}({string.Join(", ", cmd.Skip(1))})` not found!");
                    }
                    else
                    {
                        var result = "";
                        if (cmd.Length == 1) result = ExecuteCommand(cmd[0]);
                        if (cmd.Length == 2) result = ExecuteCommand(cmd[0], cmd[1]);
                        if (cmd.Length == 3) result = ExecuteCommand(cmd[0], cmd[1], cmd[2]);
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
            if (!IsShowStatByDefault) HideStat();
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
            }, "Show: stat");
            
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
            }, "Hide: stat");
            
            SetCommand("clear", () =>
            {
                Logs.Clear();
                _consoleOffsetY = 0;
                DrawConsole();
                return null;
            }, "Clear logs");
            
            SetCommand("help", () =>
            {
                var p0 = string.Join("\n", CommandList_0.Keys.Select(x =>
                {
                    CommandDescription.TryGetValue(x + "()", out var description);
                    return x + "() - " + description;
                }));
                var p1 = string.Join("\n", CommandList_1.Keys.Select(x =>
                {
                    CommandDescription.TryGetValue(x + "(a)", out var description);
                    return x + "(a) - " + description;
                }));
                var p2 = string.Join("\n", CommandList_2.Keys.Select(x =>
                {
                    CommandDescription.TryGetValue(x + "(a, b)", out var description);
                    return x + "(a, b) - " + description;
                }));
                return "\n" + p0 + "\n" + p1 + "\n" + p2;
            }, "Show command list with description");
        }
        
        public static void SetCommand(string command, Func<string> action, string description = "")
        {
            CommandList_0.Add(command, action);
            CommandDescription.Add($"{command}()", description);
        }
        
        public static void SetCommand(string command, Func<string, string> action, string description = "")
        {
            CommandList_1.Add(command, action);
            CommandDescription.Add($"{command}(a)", description);
        }

        public static void SetCommand(string command, Func<string, string, string> action, string description = "")
        {
            CommandList_2.Add(command, action);
            CommandDescription.Add($"{command}(a, b)", description);
        }
          
        public static bool HasCommand(string command, int argsLength)
        {
            if (CommandList_0.ContainsKey(command) && argsLength == 0) return true;
            if (CommandList_1.ContainsKey(command) && argsLength == 1) return true;
            if (CommandList_2.ContainsKey(command) && argsLength == 2) return true;
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

        public static string ExecuteCommand(string command, string arg0, string arg1)
        {
            return CommandList_2.TryGetValue(command, out var cmd) ? cmd?.Invoke(arg0, arg1) : null;
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
            var str = data.ToString().Split('\n');
            Logs.Add($"<color=orange><b>[{DateTime.Now:HH:mm:ss.fff}]</b></color> " + "<color=red>" + str[0] + "</color>");
            for (var i = 1; i < str.Length; i++) Logs.Add("<color=red>" + str[i] + "</color>");

            DrawConsole();
        }
        
        public static void Log(object data)
        {
            var str = data.ToString().Split('\n');
            Logs.Add($"<color=orange><b>[{DateTime.Now:HH:mm:ss.fff}]</b></color> " + str[0]);
            for (var i = 1; i < str.Length; i++) Logs.Add(str[i]);
            
            DrawConsole();
        }

        private static void DrawConsole()
        {
            var outText = "";

            for (var i = Math.Max(0, Logs.Count - MaxLines); i < Logs.Count; i++)
            {
                try
                {
                    outText += $"{Logs[i - _consoleOffsetY]}";
                    if (i != Logs.Count - 1) outText += "\n";
                }
                catch
                {
                    // ignored
                }
            }

            _output.text = outText;
        }

        private static string FindMostSuitableCommand(string text)
        {
            var p0 = CommandList_0.Keys.FirstOrDefault(x => x.StartsWith(text));
            if (p0 != null) return p0;
            var p1 = CommandList_1.Keys.FirstOrDefault(x => x.StartsWith(text));
            if (p1 != null) return p1;
            var p2 = CommandList_2.Keys.FirstOrDefault(x => x.StartsWith(text));
            if (p2 != null) return p2;
            
            return text;
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
            if (IsConsoleOpen && Input.GetKeyDown(KeyCode.Tab))
            {
                _input.text = FindMostSuitableCommand(_input.text);
                _input.caretPosition = 1000;
            }
            
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                _historyId = History.Count;
                ToggleConsole();
            }

            if (Time.frameCount % 30 == 0)
            {
                var outStat = new List<string>
                {
                    $"<color=orange>TIME </color> {Time.time:##.##}",
                    $"<color=orange>FRAME</color> {Time.frameCount}",
                    $"<color=orange>DLTM </color> {Time.deltaTime}",
                    $"<color=orange>FPS  </color> {Math.Round(1.0f / Time.smoothDeltaTime)}",
                    $"<color=orange>RAMC </color> {Profiler.GetTotalAllocatedMemoryLong() / 1048576f:##.##} MB",
                    $"<color=orange>RAMT </color> {SystemInfo.systemMemorySize / 1024f:##.##} GB",
                    $"<color=orange>RES  </color> {Screen.width}x{Screen.height}",
                    $"<color=orange>FREQ </color> {SystemInfo.processorFrequency} MHz",
                    $"<color=orange>CORE </color> {SystemInfo.processorCount}",
                    $"<color=orange>GRAPH</color> {SystemInfo.graphicsDeviceType.ToString()}",
                    $"<color=orange>VRAM </color> {SystemInfo.graphicsMemorySize} MB",
                    $"<color=orange>CARD </color> {Regex.Replace(SystemInfo.graphicsDeviceName, @"radeon|nvidia|geforce|amd", "", RegexOptions.IgnoreCase).Trim()}",
                };

                _stat.text = string.Join("\n", outStat);
            }
            
            if (Input.mouseScrollDelta.y < 0)
            {
                _consoleOffsetY -= 1;
                if (_consoleOffsetY <= 0) _consoleOffsetY = 0;
                DrawConsole();
            }
            if (Input.mouseScrollDelta.y > 0 && Logs.Count > MaxLines)
            {
                _consoleOffsetY += 1;
                if (_consoleOffsetY > Logs.Count - MaxLines)
                    _consoleOffsetY = Logs.Count - MaxLines;
                DrawConsole();
            }
        }
    }
}