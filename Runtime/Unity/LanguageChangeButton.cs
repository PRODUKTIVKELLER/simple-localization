using Unity;
using UnityEngine;
using UnityEngine.UI;

namespace Produktivkeller.SimpleLocalizations.Unity
{
    [RequireComponent(typeof(Button))]
    public class LanguageChangeButton : MonoBehaviour, ILocalizationSupport
    {
        public  Language        language;
        public  GameObject      checkImage;
        private LocalizationService _localizationService;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(ChangeLanguage);
            UpdateCheckImageStatus();
            _localizationService = LocalizationService.GetInstance();
        }

        public void ChangeLanguage()
        {
            _localizationService.ChangeLanguage(language);
        }

        public void OnLanguageHasChanged()
        {
            if (_localizationService != null)
            {
                UpdateCheckImageStatus();
            }
        }

        private void UpdateCheckImageStatus()
        {
            checkImage.SetActive(_localizationService.CurrentLanguage == language);
        }
    }
}