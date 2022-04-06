using System.IO;
using System.Reflection;
using ExcelDataReader;
using Produktivkeller.SimpleLogging;
using UnityEngine;
using UnityEngine.Networking;

namespace Produktivkeller.SimpleLocalization.Excel
{
    public static class ConfigurationLoader
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static LanguageCache LoadConfigurationAndBuildLanguageCache()
        {
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
                    Log.Debug("Could not load file at: {}. Errormessage: {}", pathToConfiguration, unityWebRequest.error);
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
                            if (reader.Name == "Localizations")
                            {
                                TranslationParser translationParser = new TranslationParser();
                                translationParser.Parse(reader);
                                languageCache = translationParser.RetrieveLanguageCache();
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