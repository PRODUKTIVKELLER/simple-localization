using Produktivkeller.SimpleLocalization.Unity.Data;

namespace Produktivkeller.SimpleLocalization.Unity.Core
{
    public class LocalizationStorage
    {
        private readonly LanguageCache _languageCache;

        public LocalizationStorage(LanguageCache languageCache)
        {
            _languageCache = languageCache;
        }

        public string ResolveLocalizationKey(string key, LanguageId languageId)
        {
            if (_languageCache.ContainsKey(languageId, key))
            {
                return _languageCache.GetKey(languageId, key);
            }

#if IS_PRODUCTION
            LanguageId defaultLanguageId = SimpleLocalizationConfigurationProvider.Instance.SimpleLocalizationConfiguration.defaultLanguageId;

            if (_languageCache.ContainsKey(defaultLanguageId, key))
            {
                return _languageCache.GetKey(defaultLanguageId, key);
            }
#endif

            return "???" + key + "???";
        }
    }
}