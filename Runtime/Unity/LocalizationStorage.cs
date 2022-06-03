namespace Produktivkeller.SimpleLocalization.Unity
{
    public class LocalizationStorage
    {
        private readonly LanguageCache _languageCache;

        public LocalizationStorage(LanguageCache languageCache)
        {
            _languageCache = languageCache;
        }

        [System.Obsolete("Language enum is obsolete. Use SimpleLocalization.Unity.Data.Language ScriptableObject instead")]
        public string ResolveLocalizationKey(string key, Language language)
        {
            if (_languageCache.ContainsKey(language, key))
            {
                return _languageCache.GetKey(language, key);
            }

            return "???" + key + "???";
        }

        public string ResolveLocalizationKey(string key, Data.Language language)
        {
            if (_languageCache.ContainsKey(language, key))
            {
                return _languageCache.GetKey(language, key);
            }

            return "???" + key + "???";
        }
    }
}