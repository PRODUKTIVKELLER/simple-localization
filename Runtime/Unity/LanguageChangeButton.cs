using UnityEngine;
using UnityEngine.UI;

namespace Produktivkeller.SimpleLocalization.Unity
{
    [RequireComponent(typeof(Button))]
    public class LanguageChangeButton : MonoBehaviour, ILocalized
    {
        
        [System.Obsolete("Language enum is obsolete. Use SimpleLocalization.Unity.Data.Language ScriptableObject instead")]
        public  Language        language;
        public  Data.Language   languageData;
        public  GameObject      checkImage;


        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(ChangeLanguage);
            UpdateCheckImageStatus();
        }

        private void ChangeLanguage()
        {
            if (language != null)
            {
                LocalizationService.Instance.ChangeLanguage(language);
            }

            if (languageData != null)
            {
                LocalizationService.Instance.ChangeLanguage(languageData);
            }
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