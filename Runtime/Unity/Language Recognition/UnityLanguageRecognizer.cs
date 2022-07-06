using Produktivkeller.SimpleLocalization.Unity.Data;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Language_Recognition
{
    public class UnityLanguageRecognizer : LanguageRecognizer
    {
        public override LanguageId Recognize()
        {
            if (SimpleLocalizationConfigurationProvider.Instance.SimpleLocalizationConfiguration.showDebugLogs)
            {
                Log.Debug("Trying to resolve language with the Unity API. Unity system language: {}", Application.systemLanguage);
            }

            return (LanguageId)((int)Application.systemLanguage + 1);
        }
    }
}