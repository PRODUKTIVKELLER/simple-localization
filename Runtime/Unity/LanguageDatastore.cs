using System.Collections.Generic;
using _modules._multi_language_support._scripts._excel;

namespace _modules._multi_language_support._scripts._unity
{
    public class LanguageDatastore
    {
        private readonly Dictionary<Language, Dictionary<string, string>> _languageCache;

        public LanguageDatastore(ConfigurationLoader configurationLoader)
        {
            _languageCache = configurationLoader.LanguageCache;
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