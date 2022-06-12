using Produktivkeller.SimpleLocalization.Unity.Data;

namespace Produktivkeller.SimpleLocalization.Unity.Language_Recognition
{
    public class DefaultLanguageRecognizer : LanguageRecognizer
    {
        protected override LanguageId MakeSuggestion()
        {
            LanguageId languageId = ConfigurationProvider.Instance.SimpleLocalizationConfiguration.defaultLanguageId;

            if (ConfigurationProvider.Instance.SimpleLocalizationConfiguration.showDebugLogs)
            {
                Log.Debug("Using default language: {}", languageId);
            }

            return languageId;
        }
    }
}