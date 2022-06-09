using System.IO;
using System.Reflection;
using System.Text;
using ExcelDataReader;
using Produktivkeller.SimpleLocalization.Unity.Core;
using Produktivkeller.SimpleLocalization.Unity.Data;
using Produktivkeller.SimpleLogging;
using UnityEngine;
using UnityEngine.Networking;

namespace Produktivkeller.SimpleLocalization.Excel
{
    internal static class ConfigurationLoader
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        public static LanguageCache LoadConfigurationAndBuildLanguageCache()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

#if UNITY_EDITOR_OSX
            Log.Debug("Adjusted path to configuration for OS X.");
            string pathToConfiguration = "File://" +Path.Combine(Application.streamingAssetsPath, "configuration.xlsx");
#else
            string pathToConfiguration = Path.Combine(Application.streamingAssetsPath, "configuration.xlsx");
#endif

            UnityWebRequest unityWebRequest = UnityWebRequest.Get(pathToConfiguration);
            unityWebRequest.SendWebRequest();
            while (!unityWebRequest.isDone)
            {
                if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
                {
                    Log.Debug("Could not load file at: {}. Error: {}", pathToConfiguration, unityWebRequest.error);
                    break;
                }
            }

            LanguageCache languageCache = new LanguageCache();

            if (!unityWebRequest.isNetworkError && !unityWebRequest.isHttpError)
            {
                byte[] results = unityWebRequest.downloadHandler.data;

                // https://answers.unity.com/questions/42955/codepage-1252-not-supported-works-in-editor-but-no.html
                using (var memoryStream = new MemoryStream(results))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(memoryStream))
                    {
                        do
                        {
                            if (reader.Name == ConfigurationProvider.Instance.SimpleLocalizationConfiguration.excelTableName)
                            {
                                LocalizationParser localizationParser = new LocalizationParser();
                                localizationParser.Parse(reader);
                                languageCache = localizationParser.RetrieveLanguageCache();
                                break;
                            }
                        } while (reader.NextResult());
                    }
                }
            }

            return languageCache;
        }
    }
}