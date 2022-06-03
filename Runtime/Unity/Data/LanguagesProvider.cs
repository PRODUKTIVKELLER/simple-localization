using System.Collections.Generic;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Data
{
    public class LanguagesProvider
    {
        private Dictionary<string, Language> _languages;

        public LanguagesProvider(Language[] languages)
        {
            _languages = new Dictionary<string, Language>();

            if (languages.Length == 0)
            {
                Debug.LogError("No language found in the Unity-Resources folder. Make sure to create at least one Language-ScriptableObject somewhere inside your Resources folder."
                    + "\nSimpleLocalization will use the obsolote mode using the Language-enum for localization now. To use the new mode, make sure to create some languages with the new Language-ScriptableObject");
               
                ///
                /// Don't create a dummy language now. Instead leave it to be null, which indicates the system to
                /// use the old obsolete system.
                ///
                //CreateDummyDefaultLanguage();
                return;
            }

            foreach (Language language in languages)
            {
                if (_languages.ContainsKey(language.countryIdentifier))
                {
                    Debug.LogError("Duplicate language found with country identifier \"" + language.countryIdentifier + "\". "
                        + "Make sure that all country identifiers of all languages are unique.");
                }
                else
                {
                    _languages.Add(language.countryIdentifier, language);
                    CheckAndSetDefaultLanguage(language);
                }
            }
        }

        private void CheckAndSetDefaultLanguage(Language language)
        {
            if (!language.isDefaultLanguage)
            {
                return;
            }

            if (language.isDefaultLanguage && DefaultLanguage != null)
            {
                Debug.LogError("More than one languages are marked as 'isDefaultLanguage'. Make sure that only one language is marked to be the default language");
                return;
            }

            DefaultLanguage = language;
        }

        private void CreateDummyDefaultLanguage()
        {
            Language dummyLanguage = new Language();
            dummyLanguage.countryIdentifier = "DUMMY_LANGUAGE";
            dummyLanguage.isDefaultLanguage = true;
            DefaultLanguage = dummyLanguage;
            _languages.Add(dummyLanguage.countryIdentifier, dummyLanguage);
        }

        public Language GetLanguageByCountryID(string countryIdentifier)
        {
            if (!_languages.ContainsKey(countryIdentifier))
            {
                Debug.LogError("Requested language not found with country identifier \"" + countryIdentifier + "\"");
                return null;
            }

            return _languages[countryIdentifier];
        }

        public Language DefaultLanguage
        {
            get; protected set;
        }

        private static LanguagesProvider _instance;
        public static LanguagesProvider Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                _instance = new LanguagesProvider(Resources.LoadAll<Language>(""));
                return _instance;
            }
        }
    }
}
