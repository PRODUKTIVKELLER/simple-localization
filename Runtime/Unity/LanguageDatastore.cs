using System.Collections.Generic;
using Produktivkeller.SimpleLocalizations.Excel;
using UnityEngine;

namespace Produktivkeller.SimpleLocalizations.Unity
{
    public class LanguageDatastore
    {
        private readonly Dictionary<Language, Dictionary<string, string>> _languageCache;

        public LanguageDatastore(Dictionary<Language, Dictionary<string, string>> languageCache)
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
    }
}