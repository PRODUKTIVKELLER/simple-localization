#if UNITY_EDITOR
using Produktivkeller.SimpleLocalization.Excel;
using Produktivkeller.SimpleLocalization.Unity.Core;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Produktivkeller.SimpleLocalization.Unity.Preprocessing
{
    public class PreprocessingExcelFile : IPreprocessBuildWithReport
    {
        private const string _languageCacheAssetFilePath = "Assets/Resources/LanguageCache.asset";

        public int callbackOrder { get { return 0; } }

        public void OnPreprocessBuild(BuildReport report)
        {
            CreateLanguageCacheFile();
        }

        public static void CreateLanguageCacheFile()
        {
            AssetDatabase.DeleteAsset(_languageCacheAssetFilePath);

            LanguageCache languageCache = LocalizationLoader.LoadConfigurationAndBuildLanguageCache();
            AssetDatabase.CreateAsset(LanguageCacheSerializable.FromLanguageCache(languageCache), _languageCacheAssetFilePath);
        }
    }
}

#endif