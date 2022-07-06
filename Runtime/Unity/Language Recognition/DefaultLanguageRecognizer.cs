using Produktivkeller.SimpleLocalization.Unity.Data;

namespace Produktivkeller.SimpleLocalization.Unity.Language_Recognition
{
    public class DefaultLanguageRecognizer : LanguageRecognizer
    {
        public override LanguageId Recognize()
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