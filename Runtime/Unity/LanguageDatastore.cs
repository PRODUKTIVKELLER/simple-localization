using System.Collections.Generic;
using Excel;
using UnityEngine;

namespace Unity
{
    public class LanguageDatastore : MonoBehaviour
    {
        private Dictionary<Language, Dictionary<string, string>> _languageCache;

        public void SetLanguageCache(Dictionary<Language, Dictionary<string, string>> languageCache)
        {
            _languageCache = languageCache;
        }

        public string ResolveTranslationKey(string translationKey, Language language)
        {
            if (_languageCache[language].ContainsKey(translationKey))
            {
                return _languageCache[language][translationKey];
            }

            return "???" + translationKey + "???";
        }
        
        #region Singleton
        
        private static LanguageDatastore _instance;
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

        private void Initialize()
        {
            ConfigurationLoader configurationLoader = ConfigurationLoader.GetInstance();
            if (configurationLoader != null)
            {
                // FIXME: Workaround because order of Awake() is undefined:
                //
                // LanguageCache is initialized depending on which Awake() is called first:
                // Awake() in ConfigurationLoader or Awake() in LanguageDatastore
                
                _languageCache = configurationLoader.languageCache;
            }
        }
        
        public static LanguageDatastore GetInstance()
        {
            return _instance;
        }

        #endregion
    }
}