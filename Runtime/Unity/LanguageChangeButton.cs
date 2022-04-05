using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _modules._multi_language_support._scripts._unity
{
    [RequireComponent(typeof(Button))]
    public class LanguageChangeButton : MonoBehaviour, IMultiLanguageSupport
    {
        public Language   language;
        public GameObject checkImage;

        [Inject]
        private LanguageService _languageService;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(ChangeLanguage);
            UpdateCheckImageStatus();
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