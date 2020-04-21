using System.Collections.Generic;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UniRx.Async;

[Serializable]
public class CarNameData
{
    public string[] carNamesEN;
    public string[] carNamesRU;
    public string[] carAdjectivesEN;
    public string[] carAdjectivesRU;
    public string[] carColorNamesEN;
    public string[] carColorNamesRU;
    public string[] carColors;
}


public class CarNames : MonoBehaviour
{
    public List<string> GetCarSubstanceNames = new List<string>();
    public List<string> GetCarColorNames = new List<string>();
    public List<string> GetCarAdjectiveNames = new List<string>();
    public List<Color> GetCarColors = new List<Color>();

    public async UniTask GetCarNames()
    {
        var url = Application.streamingAssetsPath + "/CarNames.json";
        var jsonText = "";
#if !UNITY_EDITOR && UNITY_ANDROID
		UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(url);
		await www.SendWebRequest();
		jsonText = www.downloadHandler.text;
#else
        jsonText = File.ReadAllText(url);
#endif

        var data = JsonUtility.FromJson<CarNameData>(jsonText);
        var dataCarNames = LocalizationSettings.CurrentLanguage == Language.en ? data.carNamesEN : data.carNamesRU;
        var dataColorNames = LocalizationSettings.CurrentLanguage == Language.en ? data.carColorNamesEN : data.carColorNamesRU;
        var dataAdjectiveNames = LocalizationSettings.CurrentLanguage == Language.en ? data.carAdjectivesEN : data.carAdjectivesRU;

        foreach (var carName in dataCarNames)
        {
            GetCarSubstanceNames.Add(carName);
        }

        foreach (var carColorName in dataColorNames)
        {
            GetCarColorNames.Add(carColorName);
        }

        foreach (var carAdjectiveName in dataAdjectiveNames)
        {
            GetCarAdjectiveNames.Add(carAdjectiveName);
        }

        var colors = data.carColors.Select(_ =>
        {
            ColorUtility.TryParseHtmlString(_, out var color);
            return color;
        });

        foreach (var carColor in colors)
        {
            GetCarColors.Add(carColor);
        }
    }
}