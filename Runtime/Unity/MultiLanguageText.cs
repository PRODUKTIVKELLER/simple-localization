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
            
            _text.text = LanguageService.GetInstance().ResolveTranslationKey(translationKey);
        }
    }
}