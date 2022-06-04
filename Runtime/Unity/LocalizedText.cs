using TMPro;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedText : MonoBehaviour, ILocalized
    {
        public string translationKey;

        private TextMeshProUGUI _text;
        private TMP_FontAsset   _defaultFontAsset;

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
                _text             = GetComponent<TextMeshProUGUI>();
                _defaultFontAsset = _text.font;
            }

            LocalizationService localizationService = LocalizationService.Instance;

            // Check is required because "OnEnable" can be called before LanguageService is initialized.
            if (localizationService)
            {
                if (translationKey.Length > 0)
                {
                    _text.text = localizationService.ResolveLocalizationKey(translationKey);
                }
                
                UpdateFont(localizationService);
            }
        }

        private void UpdateFont(LocalizationService localizationService)
        {
            TMP_FontAsset overwriteFontAsset = localizationService.GetOverwriteFont(_defaultFontAsset);
            
            if (overwriteFontAsset == null)
            {
                overwriteFontAsset = _defaultFontAsset;
            }

            if (overwriteFontAsset == _text.font)
            {
                return;
            }

            _text.font = overwriteFontAsset;
            _text.UpdateFontAsset();
        }
    }
}