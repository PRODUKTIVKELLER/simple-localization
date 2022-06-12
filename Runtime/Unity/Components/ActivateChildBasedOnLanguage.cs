using System.Collections.Generic;
using Produktivkeller.SimpleLocalization.Unity.Core;
using Produktivkeller.SimpleLocalization.Unity.Data;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Components
{
    public class ActivateChildBasedOnLanguage : MonoBehaviour, ILocalized
    {
        public List<LanguageId> languageIds;
        public List<GameObject> children;

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
            if (!localizationService)
            {
                return;
            }

            bool anyIsActive = false;

            for (int i = 0; i < languageIds.Count; i++)
            {
                bool active = localizationService.CurrentLanguageId == languageIds[i];

                if (active)
                {
                    anyIsActive = true;
                }

                children[i].SetActive(active);
            }

            if (anyIsActive)
            {
                return;
            }

            for (int i = 0; i < languageIds.Count; i++)
            {
                bool active = SimpleLocalizationConfigurationProvider.Instance.SimpleLocalizationConfiguration.defaultLanguageId == languageIds[i];
                children[i].SetActive(active);
            }
        }
    }
}