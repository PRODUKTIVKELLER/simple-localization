using Produktivkeller.SimpleLocalization.Unity.Data;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Language_Recognition
{
    public class UnityLanguageRecognizer : LanguageRecognizer
    {
        protected override LanguageId MakeSuggestion()
        {
            if (ConfigurationProvider.Instance.SimpleLocalizationConfiguration.showDebugLogs)
            {
                Log.Debug("Trying to resolve language with the Unity API. Unity system language: {}", Application.systemLanguage);
            }

            return (LanguageId)((int)Application.systemLanguage + 1);
        }
    }
}