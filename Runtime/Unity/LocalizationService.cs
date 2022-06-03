using System;
using System.Collections.Generic;
using Produktivkeller.SimpleLocalization.Excel;
using Produktivkeller.SimpleLocalization.Unity.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Produktivkeller.SimpleLocalization.Unity
{
    public class LocalizationService : Singleton<LocalizationService>
    {
        private static readonly string PLAYER_PREF_KEY = "language";

        public Language CurrentLanguage => _currentLanguage;
        public Data.Language CurrentLanguageData => _currentLanguageData;

        private Language            _currentLanguage;
        private Data.Language       _currentLanguageData;
        private LocalizationStorage _localizationStorage;
        private bool                _usesLegacyEnumSystem;

        protected override void Initialize()
        {
            if (PlayerPrefs.HasKey(PLAYER_PREF_KEY))
            {
                _currentLanguage = (Language) Enum.Parse(typeof(Language), PlayerPrefs.GetString(PLAYER_PREF_KEY));
                _currentLanguageData = LanguagesProvider.Instance.GetLanguageByCountryID(PLAYER_PREF_KEY);
            }
            else if (Application.systemLanguage == SystemLanguage.German)
            {
                _currentLanguage = Language.DE;
            }
            else
            {
                _currentLanguage = Language.EN;
            }

            if (_currentLanguageData == null)
            {
                if (LanguagesProvider.Instance.DefaultLanguage != null)
                {
                    _currentLanguageData = LanguagesProvider.Instance.DefaultLanguage;
                }
                else
                {
                    Debug.LogWarning("Simple Localization uses the legacy system. Please make sure to upgrade to the new system using the Language-ScriptableObject soon");
                    _usesLegacyEnumSystem = true;
                }
            }

            LanguageCache languageCache = ConfigurationLoader.LoadConfigurationAndBuildLanguageCache();
            _localizationStorage = new LocalizationStorage(languageCache);
        }


        [System.Obsolete("Language enum is obsolete. Use SimpleLocalization.Unity.Data.Language ScriptableObject instead")]
        public void ChangeLanguage(Language language)
        {
            _currentLanguage = language;
            PlayerPrefs.SetString(PLAYER_PREF_KEY, language.ToString());
            PlayerPrefs.Save();
            InformReceivers();
        }

        public void ChangeLanguage(Data.Language language)
        {
            _currentLanguageData = language;
            PlayerPrefs.SetString(PLAYER_PREF_KEY, language.ToString());
            PlayerPrefs.Save();
            InformReceivers();
        }

        public string ResolveLocalizationKey(string localizationKey)
        {
            if (string.IsNullOrEmpty(localizationKey))
            {
                return "???empty???";
            }
            
            if (_usesLegacyEnumSystem)
            {
                string textWithRichTextMarkers = _localizationStorage.ResolveLocalizationKey(localizationKey, _currentLanguage);
                return ResolveRichText(textWithRichTextMarkers);
            }
            else
            {
                string textWithRichTextMarkers = _localizationStorage.ResolveLocalizationKey(localizationKey, _currentLanguageData);
                return ResolveRichText(textWithRichTextMarkers);
            }
        }

        private string ResolveRichText(string textWithRichTextMarkers)
        {
            textWithRichTextMarkers = textWithRichTextMarkers.Replace("<1>",  "<color=#FE9E3A>");
            textWithRichTextMarkers = textWithRichTextMarkers.Replace("</1>", "</color>");

            textWithRichTextMarkers = textWithRichTextMarkers.Replace("<2>",  "<color=#92A512>");
            textWithRichTextMarkers = textWithRichTextMarkers.Replace("</2>", "</color>");

            return textWithRichTextMarkers;
        }

        private void InformReceivers()
        {
            foreach (ILocalized multiLanguageSupport in FindReceivers())
            {
                multiLanguageSupport.OnLanguageHasChanged();
            }
        }

        private List<ILocalized> FindReceivers()
        {
            List<ILocalized> interfaces      = new List<ILocalized>();
            GameObject[]               rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject rootGameObject in rootGameObjects)
            {
                ILocalized[] childInterfaces = rootGameObject.GetComponentsInChildren<ILocalized>();
                foreach (ILocalized childInterface in childInterfaces)
                {
                    interfaces.Add(childInterface);
                }
            }

            return interfaces;
        }
    }
}