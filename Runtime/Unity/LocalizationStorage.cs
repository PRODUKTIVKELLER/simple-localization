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

        public string ResolveLocalizationKey(string localizationKey, Language language)
        {
            if (_languageCache[language].ContainsKey(localizationKey))
            {
                return _languageCache[language][localizationKey];
            }

            return "???" + localizationKey + "???";
        }
    }
}