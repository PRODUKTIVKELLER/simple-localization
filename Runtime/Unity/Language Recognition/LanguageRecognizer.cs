using System.Reflection;
using Produktivkeller.SimpleLocalization.Unity.Data;
using Produktivkeller.SimpleLogging;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Language_Recognition
{
    public abstract class LanguageRecognizer : MonoBehaviour
    {
        protected static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        [Tooltip("Higher priorities are evaluated first.")] [SerializeField]
        private int priority;

        internal LanguageId Recognize()
        {
            LanguageId languageId = MakeSuggestion();

            if (ConfigurationProvider.Instance.SimpleLocalizationConfiguration.languageIds.Contains(languageId))
            {
                return languageId;
            }

            if (ConfigurationProvider.Instance.SimpleLocalizationConfiguration.showDebugLogs)
            {
                Log.Debug("Recognized language {} but it is not configured in the 'Simple Localization' configuration.", languageId);
            }

            return LanguageId.None;
        }

        protected abstract LanguageId MakeSuggestion();

        internal int GetPriority()
        {
            return priority;
        }
    }
}