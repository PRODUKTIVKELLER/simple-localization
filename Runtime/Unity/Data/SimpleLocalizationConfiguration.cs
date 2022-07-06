using System.Collections.Generic;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Data
{
    [CreateAssetMenu(fileName = "Simple Localization Configuration", menuName = "PRODUKTIVKELLER/Simple Localization/Simple Localization Configuration")]
    public class SimpleLocalizationConfiguration : ScriptableObject
    {
        public LanguageId       defaultLanguageId            = LanguageId.English;
        public List<LanguageId> languageIds                  = new List<LanguageId> { LanguageId.German, LanguageId.English };
        public List<LanguageId> sourceLanguageIds            = new List<LanguageId> { LanguageId.German, LanguageId.English };
        public string           excelTableName               = "Localization";
        public string           pathForDifferenceFiles       = "Simple Localization/Localization Differences";
        public string           pathForFinishedLocalizations = "Simple Localization/Finished Localizations";
        public bool             showDebugLogs                = true;
    }
}