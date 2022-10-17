#if UNITY_EDITOR
using System.IO;
using ClosedXML.Excel;
using Produktivkeller.SimpleLocalization.Unity.Core;
using Produktivkeller.SimpleLocalization.Unity.Data;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Excel
{
    public static class LocalizationLoader
    {
        public static LanguageCache LoadConfigurationAndBuildLanguageCache(string path, string worksheet = null)
        {
            FileStream       fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using XLWorkbook xlWorkbook = new XLWorkbook(fileStream);

            if (worksheet == null)
            {
                worksheet = SimpleLocalizationConfigurationProvider.Instance.SimpleLocalizationConfiguration.excelTableName;
            }

            LocalizationParser localizationParser = new LocalizationParser();
            localizationParser.Parse(xlWorkbook.Worksheet(worksheet));
            LanguageCache languageCache = localizationParser.RetrieveLanguageCache();

            return languageCache;
        }

        public static LanguageCache LoadConfigurationAndBuildLanguageCache()
        {
            return LoadConfigurationAndBuildLanguageCache(GetConfigurationPath());
        }

        public static string GetConfigurationPath()
        {
            string pathToConfiguration;

#if UNITY_EDITOR_OSX
            Log.Debug("Adjusted path to configuration for OS X.");
            pathToConfiguration = "File://" +Path.Combine(Application.streamingAssetsPath, "configuration.xlsx");
#else
            pathToConfiguration = Path.Combine(Application.streamingAssetsPath, "configuration.xlsx");
#endif
            return pathToConfiguration;
        }
    }
}
#endif