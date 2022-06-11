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
    internal static class LocalizationLoader
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        internal static LanguageCache LoadConfigurationAndBuildLanguageCache(string path)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            UnityWebRequest unityWebRequest = UnityWebRequest.Get(path);
            unityWebRequest.SendWebRequest();
            while (!unityWebRequest.isDone)
            {
                if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError || unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Log.Debug("Could not load file at: {}. Error: {}", path, unityWebRequest.error);
                    break;
                }
            }

            LanguageCache languageCache = new LanguageCache();

            if (unityWebRequest.result != UnityWebRequest.Result.ConnectionError && unityWebRequest.result != UnityWebRequest.Result.ProtocolError)
            {
                byte[] results = unityWebRequest.downloadHandler.data;

                // https://answers.unity.com/questions/42955/codepage-1252-not-supported-works-in-editor-but-no.html
                using (MemoryStream memoryStream = new MemoryStream(results))
                {
                    using (IExcelDataReader excelDataReader = ExcelReaderFactory.CreateReader(memoryStream))
                    {
                        do
                        {
                            if (excelDataReader.Name == ConfigurationProvider.Instance.SimpleLocalizationConfiguration.excelTableName)
                            {
                                LocalizationParser localizationParser = new LocalizationParser();
                                localizationParser.Parse(excelDataReader);
                                languageCache = localizationParser.RetrieveLanguageCache();
                                break;
                            }
                        } while (excelDataReader.NextResult());
                    }
                }
            }

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