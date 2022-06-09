using Produktivkeller.SimpleLocalization.Unity.Data;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity
{
    public class OnlyActiveForLanguage : MonoBehaviour, ILocalized
    {
        public LanguageId languageId;

        private void Start()
        {
            RefreshActiveState();
        }

        private void OnEnable()
        {
            RefreshActiveState();
        }

        public void OnLanguageHasChanged()
        {
            RefreshActiveState();
        }

        private void RefreshActiveState()
        {
            LocalizationService localizationService = LocalizationService.Instance;

            // Check is required because "OnEnable" can be called before LanguageService is initialized.
            if (localizationService)
            {
                gameObject.SetActive(localizationService.CurrentLanguageId == languageId);
            }
        }
    }
}
