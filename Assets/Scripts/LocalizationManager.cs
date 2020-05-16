using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LocalizationManager
{
	public static void Initialize()
	{
		LocalizationParser.ParseFile();

        switch (Application.systemLanguage)
        {
            case SystemLanguage.German:
                LocalizationParser.Language = "German";
                break;
            case SystemLanguage.Russian:
                LocalizationParser.Language = "Russian";
                break;
            default:
                LocalizationParser.Language = "English";
                break;
        }
    }

	public static void SetLocalization(string localization)
	{
		LocalizationParser.Language = localization;
	}
}
