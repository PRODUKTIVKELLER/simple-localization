using System.Collections.Generic;
using System.IO;
using System.Text;
using _modules._multi_language_support._scripts._unity;
using ExcelDataReader;
using UnityEngine;
using UnityEngine.Networking;

namespace _modules._multi_language_support._scripts._excel
{
    public class ConfigurationLoader
    {
        public Dictionary<Language, Dictionary<string, string>> LanguageCache;

        public ConfigurationLoader()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Load();
        }

        private void Load()
        {
#if UNITY_EDITOR_OSX
            Debug.Log("Adjusted path to configuration for Mac");
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
                    break;
                }
            }

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
                            if (reader.Name == "Translations")
                            {
                                TranslationParser translationParser = new TranslationParser();
                                translationParser.Parse(reader);
                                LanguageCache = translationParser.RetrieveLanguageCache();
                                break;
                            }
                        } while (reader.NextResult());
                    }
                }
            }
        }
    }
}