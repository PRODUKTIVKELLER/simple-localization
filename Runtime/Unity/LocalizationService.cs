using System;
using System.Collections.Generic;
using Produktivkeller.SimpleLocalization.Excel;
using Produktivkeller.SimpleLocalization.Unity.Fonts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Produktivkeller.SimpleLocalization.Unity
{
    public class LocalizationService : Singleton<LocalizationService>
    {
        private static readonly string PLAYER_PREF_KEY = "language";

        public Language CurrentLanguage => _currentLanguage;

        private Language                       _currentLanguage;
        private LocalizationStorage            _localizationStorage;
        private Dictionary<int, TMP_FontAsset> _defaultFontAssetsByGameObjectId;

        protected override void Initialize()
        {
            _defaultFontAssetsByGameObjectId = new Dictionary<int, TMP_FontAsset>();
            
            if (PlayerPrefs.HasKey(PLAYER_PREF_KEY))
            {
                _currentLanguage = (Language) Enum.Parse(typeof(Language), PlayerPrefs.GetString(PLAYER_PREF_KEY));
            }
            else if (Application.systemLanguage == SystemLanguage.German)
            {
                _currentLanguage = Language.DE;
            }
            else if (Application.systemLanguage == SystemLanguage.Chinese
                || Application.systemLanguage == SystemLanguage.ChineseSimplified
                || Application.systemLanguage == SystemLanguage.ChineseTraditional)
            {
                _currentLanguage = Language.CHN;
            }
            else
            {
                _currentLanguage = Language.EN;
            }

            LanguageCache languageCache = ConfigurationLoader.LoadConfigurationAndBuildLanguageCache();
            _localizationStorage = new LocalizationStorage(languageCache);

            InformReceivers();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            InformReceivers();
        }

        public void ChangeLanguage(Language language)
        {
            _currentLanguage = language;
            PlayerPrefs.SetString(PLAYER_PREF_KEY, language.ToString());
            PlayerPrefs.Save();
            InformReceivers();
        }

        public TMP_FontAsset GetOverwriteFont(TMP_FontAsset tmpFontAsset)
        {
            return FontsProvider.Instance.GetOverwriteFontAsset(_currentLanguage, tmpFontAsset);
        }

        public string ResolveLocalizationKey(string localizationKey)
        {
            if (string.IsNullOrEmpty(localizationKey))
            {
                return "???empty???";
            }

            string textWithRichTextMarkers = _localizationStorage.ResolveLocalizationKey(localizationKey, _currentLanguage);
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

        private void InformReceiversAboutFontChange()
        {
            foreach (ILocalized localized in FindReceivers())
            {
                UpdateFont(localized.gameObject);
            }
        }

        private List<ILocalized> FindReceivers()
        {
            List<ILocalized> interfaces      = new List<ILocalized>();
            GameObject[]               rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
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