using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public static class LocalizationParser
{
    public static event Action OnLocalizationChanged = () => { };

    private static readonly Dictionary<string, Dictionary<string, string>> Dictionary = new Dictionary<string, Dictionary<string, string>>();
    private static string _language;

    public static string Language
    {
        get { return _language; }
        set { _language = value; OnLocalizationChanged(); }
    }

    public static string Localize(string localizationKey)
    {
        return Dictionary[Language][localizationKey];
    }

    public static void ParseFile(string path = "Localization")
    {
        if (Dictionary.Count > 0) return;

        var textAssets = Resources.LoadAll<TextAsset>(path);

        foreach (var textAsset in textAssets)
        {
            var text = textAsset.text.Replace("[Newline]", "\n");
            var matches = Regex.Matches(text, "\".+?\"");

            foreach (Match match in matches)
            {
                text = text.Replace(match.Value, match.Value.Replace("\"", null).Replace(",", "[comma]").Replace(";", "[comma]"));
            }

            var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var languages = lines[0].Trim().Split(',', ';').ToList();

            for (var i = 1; i < languages.Count; i++)
            {
                if (!Dictionary.ContainsKey(languages[i]))
                {
                    Dictionary.Add(languages[i], new Dictionary<string, string>());
                }
            }

            for (var i = 1; i < lines.Length; i++)
            {
                var columns = lines[i].Split(',', ';').Select(j => j.Replace("[comma]", ",").Replace("[comma]", ";")).ToList();
                var key = columns[0];

                for (var j = 1; j < languages.Count; j++)
                {
                    Dictionary[languages[j]].Add(key, columns[j]);
                }
            }
        }
    }
}
