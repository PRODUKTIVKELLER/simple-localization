using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Produktivkeller.SimpleCodePatterns.Singleton;
using Produktivkeller.SimpleLocalization.Unity.Components;
using Produktivkeller.SimpleLocalization.Unity.Data;
using Produktivkeller.SimpleLocalization.Unity.Extensions;
using Produktivkeller.SimpleLocalization.Unity.Fonts;
using Produktivkeller.SimpleLocalization.Unity.Language_Recognition;
using Produktivkeller.SimpleLogging;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Produktivkeller.SimpleLocalization.Persistence;
#if UNITY_EDITOR
using Produktivkeller.SimpleLocalization.Excel;

#else
using Produktivkeller.SimpleLocalization.Unity.Preprocessing;
#endif

namespace Produktivkeller.SimpleLocalization.Unity.Core
{
    public class LocalizationService : SingletonMonoBehaviour<LocalizationService>
    {
        private const string PLAYER_PREF_KEY = "language";

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        public LanguageId CurrentLanguageId { get; private set; }

        private LocalizationStorage            _localizationStorage;
        private Dictionary<int, TMP_FontAsset> _defaultFontAssetsByGameObjectId;
        private ISimpleLocalizationPersistence _simpleLocalizationPersistence;

        protected override void Initialize()
        {
            _defaultFontAssetsByGameObjectId = new Dictionary<int, TMP_FontAsset>();

            CurrentLanguageId = LoadLanguageIdFromPlayerPrefs();

            if (CurrentLanguageId == LanguageId.None)
            {
                CurrentLanguageId = DetermineLanguageIdFromProviders();
            }

            if (_simpleLocalizationPersistence == null)
            {
                _simpleLocalizationPersistence = new SimpleLocalizationPersistence();
            }

#if UNITY_EDITOR
            LanguageCache languageCache = LocalizationLoader.LoadConfigurationAndBuildLanguageCache();
#else
            LanguageCache languageCache = Resources.Load<LanguageCacheSerializable>("LanguageCache").ToLanguageCache();
#endif
            _localizationStorage = new LocalizationStorage(languageCache);

            InformReceivers();
        }

        private LanguageId LoadLanguageIdFromPlayerPrefs()
        {
            if (_simpleLocalizationPersistence.HasKey(PLAYER_PREF_KEY))
            {
                try
                {
                    return (LanguageId)Enum.Parse(typeof(LanguageId), _simpleLocalizationPersistence.GetString(PLAYER_PREF_KEY));
                }
                catch (ArgumentException)
                {
                    // ignored
                }
            }

            return LanguageId.None;
        }

        private LanguageId DetermineLanguageIdFromProviders()
        {
            LanguageId languageId = LanguageId.None;

            foreach (LanguageRecognizer languageRecognizer in GetComponents<LanguageRecognizer>().ToList().OrderByDescending(l => l.GetPriority()))
            {
                languageId = languageRecognizer.Recognize();

                if (languageId != LanguageId.None)
                {
                    break;
                }
            }

            foreach (LanguageRecognitionPostProcessor languageRecognitionPostProcessor in GetComponents<LanguageRecognitionPostProcessor>())
            {
                languageId = languageRecognitionPostProcessor.Process(languageId);
            }

            if (!SimpleLocalizationConfigurationProvider.Instance.SimpleLocalizationConfiguration.languageIds.Contains(languageId))
            {
                if (SimpleLocalizationConfigurationProvider.Instance.SimpleLocalizationConfiguration.showDebugLogs)
                {
                    Log.Debug("Recognized language {} but it is not configured in the 'Simple Localization' configuration. " +
                              "Using default language instead.", languageId);
                }

                languageId = SimpleLocalizationConfigurationProvider.Instance.SimpleLocalizationConfiguration.defaultLanguageId;
            }

            return languageId;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            InformReceivers();
        }

        public void ChangeLanguage(LanguageId languageId)
        {
            CurrentLanguageId = languageId;
            _simpleLocalizationPersistence.SetString(PLAYER_PREF_KEY, languageId.ToString());
            _simpleLocalizationPersistence.Save();
            InformReceivers();
        }

        public TMP_FontAsset GetOverwriteFont(TMP_FontAsset tmpFontAsset)
        {
            return OverwriteFontProvider.Instance.GetOverwriteFontAsset(CurrentLanguageId, tmpFontAsset);
        }

        public string ResolveLocalizationKey(string localizationKey)
        {
            if (string.IsNullOrEmpty(localizationKey))
            {
                return "???empty???";
            }

            string textWithRichTextMarkers = _localizationStorage.ResolveLocalizationKey(localizationKey, CurrentLanguageId);
            return ResolveRichText(textWithRichTextMarkers);
        }

        private string ResolveRichText(string textWithRichTextMarkers)
        {
            textWithRichTextMarkers = textWithRichTextMarkers.Replace("<1>",  "<color=#FE9E3A>");
            textWithRichTextMarkers = textWithRichTextMarkers.Replace("</1>", "</color>");

            textWithRichTextMarkers = textWithRichTextMarkers.Replace("<2>",  "<color=#92A512>");
            textWithRichTextMarkers = textWithRichTextMarkers.Replace("</2>", "</color>");

            return textWithRichTextMarkers;
        }

        private void InformReceivers()
        {
            foreach (ILocalized localized in FindReceivers())
            {
                if (localized is LocalizedIgnore)
                {
                    continue;
                }

                UpdateFont(localized.gameObject);
                localized.OnLanguageHasChanged();
            }
        }

        private List<ILocalized> FindReceivers()
        {
            List<ILocalized> interfaces      = new List<ILocalized>();
            List<GameObject> rootGameObjects = new List<GameObject>();
            
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                rootGameObjects.AddRange(SceneManager.GetSceneAt(i).GetRootGameObjects());
            }
            
            foreach (GameObject rootGameObject in rootGameObjects)
            {
                ILocalized[] childInterfaces = rootGameObject.GetComponentsInChildren<ILocalized>(true);
                foreach (ILocalized childInterface in childInterfaces)
                {
                    interfaces.Add(childInterface);
                }
            }

            return interfaces;
        }

        public void UpdateFont(GameObject localizedGameObject)
        {
            TMP_Text text = localizedGameObject.GetComponent<TMP_Text>();

            if (!text)
            {
                return;
            }

            TMP_FontAsset defaultFontAsset   = GetDefaultFontAsset(text);
            TMP_FontAsset overwriteFontAsset = GetOverwriteFont(defaultFontAsset);

            if (overwriteFontAsset == null)
            {
                overwriteFontAsset = defaultFontAsset;
            }

            if (overwriteFontAsset == text.font)
            {
                return;
            }

            text.font = overwriteFontAsset;

            switch (text)
            {
                case TextMeshProUGUI t1:
                    t1.UpdateFontAsset();
                    break;
                case TextMeshPro t2:
                    t2.UpdateFontAsset();
                    break;
            }
        }

        public void UseCustomPersistence(ISimpleLocalizationPersistence simpleLocalizationPersistence)
        {
            _simpleLocalizationPersistence = simpleLocalizationPersistence;
        }

        private TMP_FontAsset GetDefaultFontAsset(TMP_Text text)
        {
            int instanceID = text.gameObject.GetInstanceID();

            if (!_defaultFontAssetsByGameObjectId.ContainsKey(instanceID))
            {
                _defaultFontAssetsByGameObjectId[instanceID] = text.font;
            }

            return _defaultFontAssetsByGameObjectId[instanceID];
        }

        #region Context Actions

        [ContextMenu("Delete player prefs")]
        private void DeletePlayerPrefs()
        {
            _simpleLocalizationPersistence.DeleteKey(PLAYER_PREF_KEY);
            _simpleLocalizationPersistence.Save();

            Log.Debug("Deleted 'PlayerPrefs' entry for preferred language.");
        }

        [ContextMenu("Find non-localized texts")]
        private void FindNonLocalizedTexts()
        {
            List<TMP_Text> nonLocalizedTexts = new List<TMP_Text>();
            
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                nonLocalizedTexts.AddRange(FindNonLocalizedTexts(SceneManager.GetSceneAt(i).GetRootGameObjects()));
            }
            
            if (nonLocalizedTexts.Count <= 0)
            {
                Log.Debug("All text elements are localized.");
                return;
            }

            string fullNamesWithNewLines = nonLocalizedTexts
                                           .Select(t => t.gameObject.BuildFullName())
                                           .Aggregate((a, b) => a + "\n" + b);
            Log.Warn("The following text elements are not localized:\n\n" + fullNamesWithNewLines + "\n");
        }

        private static List<TMP_Text> FindNonLocalizedTexts(GameObject[] rootGameObjects)
        {
            List<TMP_Text> nonLocalizedTexts = new List<TMP_Text>();

            foreach (GameObject rootGameObject in rootGameObjects)
            {
                TMP_Text[] texts = rootGameObject.GetComponentsInChildren<TMP_Text>(true);
                foreach (TMP_Text text in texts)
                {
                    if (text.GetComponent<ILocalized>() == null)
                    {
                        nonLocalizedTexts.Add(text);
                    }
                }
            }

            return nonLocalizedTexts;
        }

        #endregion
    }
}