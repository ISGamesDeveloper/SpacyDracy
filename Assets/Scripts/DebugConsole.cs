using System;
using System.Collections.Generic;
using UnityEngine;

namespace Kinomo.Debugging
{
	public class DebugConsole : MonoBehaviour
	{
		private KeyCode toggleKey = KeyCode.BackQuote;
		private List<Log> logs = new List<Log>();
		private Vector2 scrollPosition;
		private bool show;
		private bool collapse;
		private int margin = 50;
		private Rect titleBarRect = new Rect(0, 0, 10000, 20);
		private GUIContent clearLabel = new GUIContent("Clear", "Clear the contents of the console.");
		private GUIContent collapseLabel = new GUIContent("Collapse", "Hide repeated messages.");

		private static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>()
																			{
																				{LogType.Assert, Color.white},
																				{LogType.Error, Color.red},
																				{LogType.Exception, Color.red},
																				{LogType.Log, Color.white},
																				{LogType.Warning, Color.yellow},
																			};

		private void OnEnable()
		{
			Application.logMessageReceived += HandleLog;
		}

		private void OnDisable()
		{
			Application.logMessageReceived -= HandleLog;
		}

		private void Update()
		{
			if (Input.GetKeyDown(toggleKey))
			{
				show = !show;
			}
		}

		private void OnGUI()
		{
			if (GUILayout.Button("Button"))
			{
				show = !show;
			}

			if (!show)
			{
				return;
			}

			GUI.Window(
				123456,
				new Rect(margin, margin, Screen.width - (margin * 2), Screen.height - (margin * 2)),
				ConsoleWindow,
				"Console");
		}

		private void ConsoleWindow(int windowID)
		{
			scrollPosition = GUILayout.BeginScrollView(scrollPosition);

			for (int i = 0; i < logs.Count; i++)
			{
				var log = logs[i];

				if (collapse)
				{
					var messageSameAsPrevious = i > 0 && log.message == logs[i - 1].message;

					if (messageSameAsPrevious)
					{
						continue;
					}
				}

				GUI.contentColor = logTypeColors[log.type];
				GUILayout.Label(log.message);
			}

			GUILayout.EndScrollView();

			GUI.contentColor = Color.white;

			GUILayout.BeginHorizontal();

			if (GUILayout.Button(clearLabel))
			{
				logs.Clear();
			}

			collapse = GUILayout.Toggle(collapse, collapseLabel, GUILayout.ExpandWidth(false));

			GUILayout.EndHorizontal();

			GUI.DragWindow(titleBarRect);
		}

		private void HandleLog(string message, string stackTrace, LogType type)
		{
			logs.Add(new Log
			{
				message = message,
				stackTrace = stackTrace,
				type = type,
			});
		}

		private struct Log
		{
			public string message;
			public string stackTrace;
			public LogType type;
		}
	}
} 