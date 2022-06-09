using Produktivkeller.SimpleLocalization.Unity.Data;

namespace Produktivkeller.SimpleLocalization.Unity
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

            return "???" + key + "???";
        }
    }
}