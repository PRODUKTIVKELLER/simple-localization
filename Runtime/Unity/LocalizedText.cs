using TMPro;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedText : MonoBehaviour, ILocalized
    {
        public string translationKey;

        private TextMeshProUGUI _text;

        private void Start()
        {
            ResolveLocalization();
        }

        private void OnEnable()
        {
            ResolveLocalization();
        }

        public void OnLanguageHasChanged()
        {
            ResolveLocalization();
        }

        private void ResolveLocalization()
        {
            if (_text == null)
            {
                _text = GetComponent<TextMeshProUGUI>();
            }

            LocalizationService localizationService = LocalizationService.Instance;

            // Check is required because "OnEnable" can be called before LanguageService is initialized.
            if (localizationService)
            {
                _text.text = localizationService.ResolveLocalizationKey(translationKey);
            }
        }
    }
}