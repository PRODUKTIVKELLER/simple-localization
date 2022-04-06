namespace Produktivkeller.SimpleLocalization.Unity
{
    public class LocalizationStorage
    {
        private readonly LanguageCache _languageCache;

        public LocalizationStorage(LanguageCache languageCache)
        {
            _languageCache = languageCache;
        }

        public string ResolveLocalizationKey(string key, Language language)
        {
            if (_languageCache.ContainsKey(language, key))
            {
                return _languageCache.GetKey(language, key);
            }

            return "???" + key + "???";
        }
    }
}