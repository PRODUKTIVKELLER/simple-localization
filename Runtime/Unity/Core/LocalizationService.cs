using System;
using System.Collections.Generic;
using System.Reflection;
using Produktivkeller.SimpleLocalization.Code_Patterns;
using Produktivkeller.SimpleLocalization.Excel;
using Produktivkeller.SimpleLocalization.Unity.Components;
using Produktivkeller.SimpleLocalization.Unity.Data;
using Produktivkeller.SimpleLocalization.Unity.Fonts;
using Produktivkeller.SimpleLogging;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

                CurrentLanguageId = languageId;
            }

            LanguageCache languageCache = ConfigurationLoader.LoadConfigurationAndBuildLanguageCache();
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
            return LanguageId.None;
        }

        [ContextMenu("Delete Player Prefs")]
        private void DeletePlayerPrefs()
        {
            PlayerPrefs.DeleteKey(PLAYER_PREF_KEY);
            PlayerPrefs.Save();

            Log.Debug("Deleted 'PlayerPrefs' entry for preferred language.");
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
            TextMeshProUGUI text = localizedGameObject.GetComponent<TextMeshProUGUI>();

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
            text.UpdateFontAsset();
        }

        private TMP_FontAsset GetDefaultFontAsset(TextMeshProUGUI text)
        {
            int instanceID = text.gameObject.GetInstanceID();

            if (!_defaultFontAssetsByGameObjectId.ContainsKey(instanceID))
            {
                _defaultFontAssetsByGameObjectId[instanceID] = text.font;
            }

            return _defaultFontAssetsByGameObjectId[instanceID];
        }
    }
}