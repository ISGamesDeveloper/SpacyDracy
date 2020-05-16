using UnityEngine;
using UnityEngine.UI;

namespace Assets.SimpleLocalization
{
	/// <summary>
	/// Localize dropdown component.
	/// </summary>
    [RequireComponent(typeof(Dropdown))]
    public class LocalizedDropdown : MonoBehaviour
    {
        public string[] LocalizationKeys;

        public void Start()
        {
            Localize();
            LocalizationParser.OnLocalizationChanged += Localize;
        }

        public void OnDestroy()
        {
            LocalizationParser.OnLocalizationChanged -= Localize;
        }

        private void Localize()
        {
	        var dropdown = GetComponent<Dropdown>();

			for (var i = 0; i < LocalizationKeys.Length; i++)
	        {
		        dropdown.options[i].text = LocalizationParser.Localize(LocalizationKeys[i]);
	        }

	        dropdown.captionText.text = LocalizationParser.Localize(LocalizationKeys[dropdown.value]);
        }
    }
}