using Produktivkeller.SimpleLocalization.Excel;
using Produktivkeller.SimpleLocalization.Unity.Core;
using Produktivkeller.SimpleLocalization.Unity.Preprocessing;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Produktivkeller.SimpleLocalization.Editor.Unity.Preprocessing
{
    public class Preprocess_ConvertLocalizationExcelToBinary : IPreprocessBuildWithReport
    {
        private const string LANGUAGE_CACHE_ASSET_FILE_PATH = "Assets/Resources/LanguageCache.asset";

        public int callbackOrder
        {
            get => 0;
        }

        public void OnPreprocessBuild(BuildReport buildReport)
        {
            CreateLanguageCacheFile();
        }

        private static void CreateLanguageCacheFile()
        {
            AssetDatabase.DeleteAsset(LANGUAGE_CACHE_ASSET_FILE_PATH);

            LanguageCache languageCache = LocalizationLoader.LoadConfigurationAndBuildLanguageCache();
            AssetDatabase.CreateAsset(LanguageCacheSerializable.FromLanguageCache(languageCache), LANGUAGE_CACHE_ASSET_FILE_PATH);
        }
    }
}