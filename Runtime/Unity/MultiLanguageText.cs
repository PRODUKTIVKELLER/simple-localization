using TMPro;
using UnityEngine;
using Zenject;

namespace _modules._multi_language_support._scripts._unity
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class MultiLanguageText : MonoBehaviour, IMultiLanguageSupport
    {
        public string translationKey;

        [Inject]
        private LanguageService _languageService;

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

            if (_languageService != null)
            {
                _text.text = _languageService.ResolveTranslationKey(translationKey);
            }
        }
    }
}