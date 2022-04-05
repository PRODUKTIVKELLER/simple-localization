using System.Collections.Generic;
using Excel;
using UnityEngine;

namespace Unity
{
    public class LanguageDatastore : MonoBehaviour
    {
        private Dictionary<Language, Dictionary<string, string>> _languageCache;

        private void Initialize()
        {
            _languageCache = ConfigurationLoader.GetInstance().languageCache;
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
            }
        }

        private void Start()
        {
            if (_instance == null)
            {
                Initialize();
            }
        }

        public static LanguageDatastore GetInstance()
        {
            return _instance;
        }

        #endregion
    }
}