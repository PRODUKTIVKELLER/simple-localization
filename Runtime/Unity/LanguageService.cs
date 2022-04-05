using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Produktivkeller.SimpleLocalizations.Unity
{
    public class LanguageService : MonoBehaviour
    {
        private static readonly string PLAYER_PREF_KEY = "language";

        public Language CurrentLanguage => _currentLanguage;

        private Language _currentLanguage;

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
        }

        public void ChangeLanguage(Language language)
        {
            _currentLanguage = language;
            PlayerPrefs.SetString(PLAYER_PREF_KEY, language.ToString());
            PlayerPrefs.Save();
            InformReceivers();
        }

        public string ResolveTranslationKey(string translationKey)
        {
            if (translationKey == null || translationKey.Length == 0)
            {
                return "???empty???";
            }

            string textWithRichtTextMarkers = LanguageDatastore.GetInstance().ResolveTranslationKey(translationKey, _currentLanguage);
            return ResolveRichText(textWithRichtTextMarkers);
        }

        private string ResolveRichText(string textWithRichtTextMarkers)
        {
            textWithRichtTextMarkers = textWithRichtTextMarkers.Replace("<1>",  "<color=#FE9E3A>");
            textWithRichtTextMarkers = textWithRichtTextMarkers.Replace("</1>", "</color>");

            textWithRichtTextMarkers = textWithRichtTextMarkers.Replace("<2>",  "<color=#92A512>");
            textWithRichtTextMarkers = textWithRichtTextMarkers.Replace("</2>", "</color>");

            return textWithRichtTextMarkers;
        }

        private void InformReceivers()
        {
            foreach (IMultiLanguageSupport multiLanguageSupport in FindReceivers())
            {
                multiLanguageSupport.OnLanguageHasChanged();
            }
        }

        private List<IMultiLanguageSupport> FindReceivers()
        {
            List<IMultiLanguageSupport> interfaces      = new List<IMultiLanguageSupport>();
            GameObject[]                rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject rootGameObject in rootGameObjects)
            {
                IMultiLanguageSupport[] childInterfaces = rootGameObject.GetComponentsInChildren<IMultiLanguageSupport>();
                foreach (IMultiLanguageSupport childInterface in childInterfaces)
                {
                    interfaces.Add(childInterface);
                }
            }

            return interfaces;
        }

        #region Singleton

        private static LanguageService _instance;

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

        public static LanguageService GetInstance()
        {
            return _instance;
        }

        #endregion
    }
}