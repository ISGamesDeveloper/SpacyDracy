public enum Language
{
	ru,
	en
}

public static class LocalizationSettings
{
	public static string NewPlayer;
	public static string TakePhoto;
	public static Language CurrentLanguage;

	public static void SetLocalizationSettings(Language language)
	{
		CurrentLanguage = language;

		if (language == Language.en)
		{
			NewPlayer = "NEW PLAYER";
			TakePhoto = "TAKE PHOTO";
		}
		if (language == Language.ru)
		{
			NewPlayer = "НОВЫЙ ИГРОК";
			TakePhoto = "СДЕЛАТЬ ФОТО";
		}
	}
}
