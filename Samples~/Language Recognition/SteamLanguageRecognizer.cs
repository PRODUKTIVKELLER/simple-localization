using Produktivkeller.SimpleLocalization.Unity.Data;
using UnityEngine;
using _modules._steam._scripts;

#if STEAMWORKS_NET
using Steamworks;
#endif

namespace Produktivkeller.SimpleLocalization.Unity.Language_Recognition
{
    public class SteamLanguageRecognizer : LanguageRecognizer
    {
        protected override LanguageId MakeSuggestion()
        {
#if !STEAMWORKS_NET
            return LanguageId.None;
#endif

#if STEAMWORKS_NET
            if (!SteamManager.Initialized) {
                Log.Debug("Can't resolve language with the Steam API as Steam is not initialized, yet.");
            }

            string steamLanguage = SteamApps.GetCurrentGameLanguage();

            if (SimpleLocalizationConfigurationProvider.Instance.SimpleLocalizationConfiguration.showDebugLogs)
            {
                Log.Debug("Trying to resolve language with the Steam API. Steam language: {}", Application.systemLanguage);
            }

            // https://partner.steamgames.com/doc/store/localization#supported_languages
            switch (steamLanguage)
            {
                case "arabic":
                    return LanguageId.Arabic;
                case "bulgarian":
                    return LanguageId.Bulgarian;
                    
                case "schinese":
                    return LanguageId.ChineseSimplified;
                    
                case "tchinese":
                    return LanguageId.ChineseTraditional;
                    
                case "czech":
                    return LanguageId.Czech;
                    
                case "danish":
                    return LanguageId.Danish;
                    
                case "dutch":
                    return LanguageId.Dutch;
                    
                case "english":
                    return LanguageId.English;
                    
                case "finnish":
                    return LanguageId.Finnish;
                    
                case "french":
                    return LanguageId.French;
                    
                case "german":
                    return LanguageId.German;
                    
                case "greek":
                    return LanguageId.Greek;
                    
                case "hungarian":
                    return LanguageId.Hungarian;
                    
                case "italian":
                    return LanguageId.Italian;
                    
                case "japanese":
                    return LanguageId.Japanese;
                    
                case "koreana":
                    return LanguageId.Korean;
                    
                case "norwegian":
                    return LanguageId.Norwegian;
                    
                case "polish":
                    return LanguageId.Polish;
                    
                case "portuguese":
                    return LanguageId.Portuguese;
                    
                case "brazilian":
                    return LanguageId.Brazilian;
                    
                case "romanian":
                    return LanguageId.Romanian;
                    
                case "russian":
                    return LanguageId.Russian;
                    
                case "spanish":
                    return LanguageId.Spanish;
                    
                case "latam":
                    return LanguageId.Spanish;
                    
                case "swedish":
                    return LanguageId.Swedish;
                    
                case "thai":
                    return LanguageId.Thai;
                    
                case "turkish":
                    return LanguageId.Turkish;
                    
                case "ukrainian":
                    return LanguageId.Ukrainian;
                    
                case "vietnamese":
                    return LanguageId.Vietnamese;
            }

            return LanguageId.None;
#endif
        }
    }
}