using System;
using System.Collections.Generic;
using Produktivkeller.SimpleLocalizations.Excel;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Produktivkeller.SimpleLocalizations.Unity
{
    public class LocalizationService : MonoBehaviour
    {
        private static readonly string PLAYER_PREF_KEY = "language";

        public Language CurrentLanguage => _currentLanguage;

        private Language          _currentLanguage;
        private LanguageDatastore _languageDatastore;

        private void Initialize()
        {
            if (PlayerPrefs.HasKey(PLAYER_PREF_KEY))
            {
                _currentLanguage = (Language) Enum.Parse(typeof(Language), PlayerPrefs.GetString(PLAYER_PREF_KEY));
            }
            else if (Application.systemLanguage == SystemLanguage.German)
            {
                _currentLanguage = Language.DE;
            }
            else
            {
                _currentLanguage = Language.EN;
            }

            Dictionary<Language, Dictionary<string, string>> languageCache = ConfigurationLoader.LoadConfigurationAndBuildLanguageCache();
            _languageDatastore = new LanguageDatastore(languageCache);
        }

        public void ChangeLanguage(Language language)
        {
            _currentLanguage = language;
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

            string textWithRichTextMarkers = _languageDatastore.ResolveLocalizationKey(localizationKey, _currentLanguage);
            return ResolveRichText(textWithRichTextMarkers);
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
            foreach (ILocalizationSupport multiLanguageSupport in FindReceivers())
            {
                multiLanguageSupport.OnLanguageHasChanged();
            }
        }

        private List<ILocalizationSupport> FindReceivers()
        {
            List<ILocalizationSupport> interfaces      = new List<ILocalizationSupport>();
            GameObject[]                rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject rootGameObject in rootGameObjects)
            {
                ILocalizationSupport[] childInterfaces = rootGameObject.GetComponentsInChildren<ILocalizationSupport>();
                foreach (ILocalizationSupport childInterface in childInterfaces)
                {
                    interfaces.Add(childInterface);
                }
            }

            return interfaces;
        }

        #region Singleton

        private static LocalizationService _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else if (_instance == null)
            {
                _instance = this;
                transform.SetParent(null);
                DontDestroyOnLoad(this);
                Initialize();
            }
        }

        public static LocalizationService GetInstance()
        {
            return _instance;
        }

        #endregion
    }
}