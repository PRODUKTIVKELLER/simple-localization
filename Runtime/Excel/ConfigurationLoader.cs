using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using ExcelDataReader;
using Produktivkeller.SimpleLocalizations.Unity;
using Produktivkeller.SimpleLogging;
using UnityEngine;
using UnityEngine.Networking;

namespace Produktivkeller.SimpleLocalizations.Excel
{
    public class ConfigurationLoader : MonoBehaviour
    {
        public Dictionary<Language, Dictionary<string, string>> languageCache;

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private void Load()
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
                    Log.Debug("Could not load file at: {}. Errormessage: {}",     pathToConfiguration, unityWebRequest.error);
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
                                languageCache = translationParser.RetrieveLanguageCache();
                                break;
                            }
                        } while (reader.NextResult());
                    }
                }
            }
        }

        #region Singleton

        private static ConfigurationLoader _instance;

        private void Initialize()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Load();

            LanguageDatastore languageDatastore = LanguageDatastore.GetInstance();
            if (languageDatastore != null)
            {
                // FIXME: Workaround because order of Awake() is undefined:
                //
                // LanguageCache is initialized depending on which Awake() is called first:
                // Awake() in ConfigurationLoader or Awake() in LanguageDatastore

                LanguageDatastore.GetInstance().SetLanguageCache(languageCache);
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else if (_instance == null)
            {
                _instance = this;
                transform.SetParent(null);
                DontDestroyOnLoad(this);
                Initialize();
            }
        }

        public static ConfigurationLoader GetInstance()
        {
            return _instance;
        }

        #endregion
    }
}