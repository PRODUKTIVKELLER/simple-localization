using UnityEngine;
using UnityEngine.UI;

namespace Produktivkeller.SimpleLocalization.Unity
{
    [RequireComponent(typeof(Button))]
    public class LanguageChangeButton : MonoBehaviour, ILocalized
    {
        public  Language        language;
        public  GameObject      checkImage;


        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(ChangeLanguage);
            UpdateCheckImageStatus();
        }

        private void ChangeLanguage()
        {
            LocalizationService.Instance.ChangeLanguage(language);
        }

        public void OnLanguageHasChanged()
        {
            if (LocalizationService.Instance != null)
            {
                UpdateCheckImageStatus();
            }
        }

        private void UpdateCheckImageStatus()
        {
            if (checkImage != null)
            {
                checkImage.SetActive(LocalizationService.Instance.CurrentLanguage == language); 
            }
        }
    }
}