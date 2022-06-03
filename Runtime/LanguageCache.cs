using System.Collections.Generic;
using Produktivkeller.SimpleLocalization.Unity;

namespace Produktivkeller.SimpleLocalization
{
    public class LanguageCache
    {
        [System.Obsolete("Language enum is obsolete. Use SimpleLocalization.Unity.Data.Language ScriptableObject instead")]
        private readonly Dictionary<Language, Dictionary<string, string>> _languageCache;
        private readonly Dictionary<Unity.Data.Language, Dictionary<string, string>> _languageCacheData;

        public LanguageCache()
        {
            _languageCache = new Dictionary<Language, Dictionary<string, string>>();
            _languageCacheData = new Dictionary<Unity.Data.Language, Dictionary<string, string>>();
        }

        [System.Obsolete("Language enum is obsolete. Use SimpleLocalization.Unity.Data.Language ScriptableObject instead")]
        public void AddLanguage(Language language)
        {
            _languageCache[language] = new Dictionary<string, string>();
        }

        [System.Obsolete("Language enum is obsolete. Use SimpleLocalization.Unity.Data.Language ScriptableObject instead")]
        public void AddEntry(Language language, string key, string value)
        {
            _languageCache[language][key] = value;
        }

        [System.Obsolete("Language enum is obsolete. Use SimpleLocalization.Unity.Data.Language ScriptableObject instead")]
        public bool ContainsKey(Language language, string key)
        {
            return _languageCache[language].ContainsKey(key);
        }

        [System.Obsolete("Language enum is obsolete. Use SimpleLocalization.Unity.Data.Language ScriptableObject instead")]
        public string GetKey(Language language, string key)
        {
            return _languageCache[language][key];
        }



        public void AddLanguage(Unity.Data.Language language)
        {
            _languageCacheData[language] = new Dictionary<string, string>();
        }

        public void AddEntry(Unity.Data.Language language, string key, string value)
        {
            _languageCacheData[language][key] = value;
        }

        public bool ContainsKey(Unity.Data.Language language, string key)
        {
            return _languageCacheData[language].ContainsKey(key);
        }

        public string GetKey(Unity.Data.Language language, string key)
        {
            return _languageCacheData[language][key];
        }
    }
}