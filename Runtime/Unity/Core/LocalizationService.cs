using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Produktivkeller.SimpleLocalization.Code_Patterns;
using Produktivkeller.SimpleLocalization.Excel;
using Produktivkeller.SimpleLocalization.Unity.Components;
using Produktivkeller.SimpleLocalization.Unity.Data;
using Produktivkeller.SimpleLocalization.Unity.Difference;
using Produktivkeller.SimpleLocalization.Unity.Extensions;
using Produktivkeller.SimpleLocalization.Unity.Fonts;
using Produktivkeller.SimpleLogging;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
#if STEAMWORKS_NET
using Steamworks;
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

        protected override void Initialize()
        {
            _defaultFontAssetsByGameObjectId = new Dictionary<int, TMP_FontAsset>();

            if (PlayerPrefs.HasKey(PLAYER_PREF_KEY))
            {
                CurrentLanguageId = (LanguageId)Enum.Parse(typeof(LanguageId), PlayerPrefs.GetString(PLAYER_PREF_KEY));
            }
            else
            {
                LanguageId languageId = ResolveLanguageFromSteam();

                if (languageId == LanguageId.None)
                {
                    languageId = ResolveLanguageFromUnity();
                }

                if (languageId == LanguageId.None)
                {
                    languageId = ConfigurationProvider.Instance.SimpleLocalizationConfiguration.defaultLanguageId;
                }

                // TODO: Make mapping configurable by user.
                if (languageId == LanguageId.Chinese || languageId == LanguageId.ChineseSimplified || languageId == LanguageId.ChineseTraditional)
                {
                    languageId = LanguageId.ChineseSimplified;
                }

                CurrentLanguageId = languageId;
            }

            LanguageCache languageCache = LocalizationLoader.LoadConfigurationAndBuildLanguageCache();
            _localizationStorage = new LocalizationStorage(languageCache);

            InformReceivers();
        }

        private LanguageId ResolveLanguageFromUnity()
        {
            Log.Debug("Trying to resolve language with the Unity API. Unity system language: {}", Application.systemLanguage);
            LanguageId languageId = (LanguageId)((int)Application.systemLanguage + 1);

            if (ConfigurationProvider.Instance.SimpleLocalizationConfiguration.languageIds.Contains(languageId))
            {
                return languageId;
            }

            return LanguageId.None;
        }

        private LanguageId ResolveLanguageFromSteam()
        {
#if !STEAMWORKS_NET
            return LanguageId.None;
#endif

#if STEAMWORKS_NET
            string steamLanguage = SteamApps.GetCurrentGameLanguage();

            // https://partner.steamgames.com/doc/store/localization#supported_languages
            switch (steamLanguage)
            {
                case "arabic":
                    return LanguageId.Arabic;
                    break;
                case "bulgarian":
                    return LanguageId.Bulgarian;
                    break;
                case "schinese":
                    return LanguageId.ChineseSimplified;
                    break;
                case "tchinese":
                    return LanguageId.ChineseTraditional;
                    break;
                case "czech":
                    return LanguageId.Czech;
                    break;
                case "danish":
                    return LanguageId.Danish;
                    break;
                case "dutch":
                    return LanguageId.Dutch;
                    break;
                case "english":
                    return LanguageId.English;
                    break;
                case "finnish":
                    return LanguageId.Finnish;
                    break;
                case "french":
                    return LanguageId.French;
                    break;
                case "german":
                    return LanguageId.German;
                    break;
                case "greek":
                    return LanguageId.Greek;
                    break;
                case "hungarian":
                    return LanguageId.Hungarian;
                    break;
                case "italian":
                    return LanguageId.Italian;
                    break;
                case "japanese":
                    return LanguageId.Japanese;
                    break;
                case "koreana":
                    return LanguageId.Korean;
                    break;
                case "norwegian":
                    return LanguageId.Norwegian;
                    break;
                case "polish":
                    return LanguageId.Polish;
                    break;
                case "portuguese":
                    return LanguageId.Portuguese;
                    break;
                case "brazilian":
                    return LanguageId.Brazilian;
                    break;
                case "romanian":
                    return LanguageId.Romanian;
                    break;
                case "russian":
                    return LanguageId.Russian;
                    break;
                case "spanish":
                    return LanguageId.Spanish;
                    break;
                case "latam":
                    return LanguageId.Spanish;
                    break;
                case "swedish":
                    return LanguageId.Swedish;
                    break;
                case "thai":
                    return LanguageId.Thai;
                    break;
                case "turkish":
                    return LanguageId.Turkish;
                    break;
                case "ukrainian":
                    return LanguageId.Ukrainian;
                    break;
                case "vietnamese":
                    return LanguageId.Vietnamese;
                    break;
                default:
                    break;
            }

            return LanguageId.None;
#endif
            
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
            PlayerPrefs.SetString(PLAYER_PREF_KEY, languageId.ToString());
            PlayerPrefs.Save();
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
                UpdateFont(localized.gameObject);
                localized.OnLanguageHasChanged();
            }
        }

        private List<ILocalized> FindReceivers()
        {
            List<ILocalized> interfaces      = new List<ILocalized>();
            GameObject[]     rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
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
            PlayerPrefs.DeleteKey(PLAYER_PREF_KEY);
            PlayerPrefs.Save();

            Log.Debug("Deleted 'PlayerPrefs' entry for preferred language.");
        }

        [ContextMenu("Find non-localized texts")]
        private void FindNonLocalizedTexts()
        {
            List<TMP_Text> nonLocalizedTexts = new List<TMP_Text>();
            GameObject[]   rootGameObjects   = SceneManager.GetActiveScene().GetRootGameObjects();

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

        [ContextMenu("Generate localization difference")]
        private void GenerateLocalizationDifference()
        {
            new LocalizationDifferenceGenerator().Generate();
        }

        #endregion
    }
}