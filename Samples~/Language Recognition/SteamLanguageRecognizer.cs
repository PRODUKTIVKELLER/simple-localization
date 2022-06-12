using Produktivkeller.SimpleLocalization.Unity.Data;
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
            string steamLanguage = SteamApps.GetCurrentGameLanguage();

            if (ConfigurationProvider.Instance.SimpleLocalizationConfiguration.showDebugLogs)
            {
                Log.Debug("Trying to resolve language with the Steam API. Steam language: {}", Application.systemLanguage);
            }

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
    }
}