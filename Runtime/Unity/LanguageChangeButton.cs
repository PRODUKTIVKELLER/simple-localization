using UnityEngine;
using UnityEngine.UI;

namespace Unity
{
    [RequireComponent(typeof(Button))]
    public class LanguageChangeButton : MonoBehaviour, IMultiLanguageSupport
    {
        public  Language        language;
        public  GameObject      checkImage;
        private LanguageService _languageService;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(ChangeLanguage);
            UpdateCheckImageStatus();
            _languageService = LanguageService.GetInstance();
        }

        public void ChangeLanguage()
        {
            _languageService.ChangeLanguage(language);
        }

        public void OnLanguageHasChanged()
        {
            if (_languageService != null)
            {
                UpdateCheckImageStatus();
            }
        }

        private void UpdateCheckImageStatus()
        {
            checkImage.SetActive(_languageService.CurrentLanguage == language);
        }
    }
}