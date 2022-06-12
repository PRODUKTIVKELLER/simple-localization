using Produktivkeller.SimpleLocalization.Unity.Data;

namespace Produktivkeller.SimpleLocalization.Unity.Language_Recognition
{
    public class DefaultLanguageRecognizer : LanguageRecognizer
    {
        protected override LanguageId MakeSuggestion()
        {
            LanguageId languageId = SimpleLocalizationConfigurationProvider.Instance.SimpleLocalizationConfiguration.defaultLanguageId;

            if (SimpleLocalizationConfigurationProvider.Instance.SimpleLocalizationConfiguration.showDebugLogs)
            {
                Log.Debug("Using default language: {}", languageId);
            }

            return languageId;
        }
    }
}