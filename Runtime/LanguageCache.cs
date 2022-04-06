using System.Collections.Generic;
using Produktivkeller.SimpleLocalizations.Unity;

namespace Produktivkeller.SimpleLocalizations
{
    public class LanguageCache
    {
        private readonly Dictionary<Language, Dictionary<string, string>> _languageCache;

        public LanguageCache()
        {
            _languageCache = new Dictionary<Language, Dictionary<string, string>>();
        }

        public void AddLanguage(Language language)
        {
            _languageCache[language] = new Dictionary<string, string>();
        }

        public void AddEntry(Language language, string key, string value)
        {
            _languageCache[language][key] = value;
        }

        public bool ContainsKey(Language language, string key)
        {
            return _languageCache[language].ContainsKey(key);
        }

        public string GetKey(Language language, string key)
        {
            return _languageCache[language][key];
        }
    }
}