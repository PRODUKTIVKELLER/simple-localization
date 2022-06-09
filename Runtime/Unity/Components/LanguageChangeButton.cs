using Produktivkeller.SimpleLocalization.Unity.Core;
using Produktivkeller.SimpleLocalization.Unity.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Produktivkeller.SimpleLocalization.Unity.Components
{
    [RequireComponent(typeof(Button))]
    public class LanguageChangeButton : MonoBehaviour, ILocalized
    {
        public LanguageId languageId;
        public GameObject checkImage;
        
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(ChangeLanguage);
            UpdateCheckImageStatus();
        }

        private void ChangeLanguage()
        {
            LocalizationService.Instance.ChangeLanguage(languageId);
        }

        public void OnLanguageHasChanged()
        {
            if (LocalizationService.Instance == null)
            {
                return;
            }

            UpdateCheckImageStatus();
        }

        private void UpdateCheckImageStatus()
        {
            if (checkImage == null)
            {
                return;
            }

            checkImage.SetActive(LocalizationService.Instance.CurrentLanguageId == languageId);
        }
    }
}