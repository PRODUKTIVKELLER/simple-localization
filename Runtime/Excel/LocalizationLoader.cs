using System.IO;
using System.Reflection;
using System.Text;
using ClosedXML.Excel;
using Produktivkeller.SimpleLocalization.Unity.Core;
using Produktivkeller.SimpleLocalization.Unity.Data;
using Produktivkeller.SimpleLogging;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Excel
{
    internal static class LocalizationLoader
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        internal static LanguageCache LoadConfigurationAndBuildLanguageCache(string path)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using XLWorkbook xlWorkbook = new XLWorkbook(path);
            
            LocalizationParser localizationParser = new LocalizationParser();
            localizationParser.Parse(xlWorkbook.Worksheet(ConfigurationProvider.Instance.SimpleLocalizationConfiguration.excelTableName));
            LanguageCache languageCache = localizationParser.RetrieveLanguageCache();
            
            return languageCache;
        }

        internal static LanguageCache LoadConfigurationAndBuildLanguageCache()
        {
            return LoadConfigurationAndBuildLanguageCache(GetConfigurationPath());
        }

        internal static string GetConfigurationPath()
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