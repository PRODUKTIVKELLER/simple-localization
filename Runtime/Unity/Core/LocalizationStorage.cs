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
            return _languageCache.GetKey(ConfigurationProvider.Instance.SimpleLocalizationConfiguration.defaultLanguageId, key);
            #else
            return "???" + key + "???";
            #endif
        }
    }
}