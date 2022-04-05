using TMPro;
using UnityEngine;

namespace Unity
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class MultiLanguageText : MonoBehaviour, IMultiLanguageSupport
    {
        public string translationKey;

        private TextMeshProUGUI _text;

        private void Start()
        {
            ResolveTranslation();
        }

        private void OnEnable()
        {
            ResolveTranslation();
        }

        public void OnLanguageHasChanged()
        {
            ResolveTranslation();
        }

        private void ResolveTranslation()
        {
            if (_text == null)
            {
                _text = GetComponent<TextMeshProUGUI>();
            }

            LanguageService languageService = LanguageService.GetInstance();

            // Check is required because "OnEnable" can be called before LanguageService is initialized.
            if (languageService)
            {
                _text.text = languageService.ResolveTranslationKey(translationKey);
            }
        }
    }
}