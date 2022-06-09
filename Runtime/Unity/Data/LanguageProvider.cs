using System.Collections.Generic;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Data
{
    public class LanguageProvider : Singleton<LanguageProvider>
    {
        private Dictionary<SystemLanguage, Language> _languages;
        private Language                             _defaultLanguage;

        protected override void Initialize()
        {
            Language[] languages = Resources.LoadAll<Language>("");
            _languages = new Dictionary<SystemLanguage, Language>();

            if (languages.Length == 0)
            {
                Debug.LogError("No language found in the Resources folder.");
                return;
            }

            foreach (Language language in languages)
            {
                if (_languages.ContainsKey(language.systemLanguage))
                {
                    Debug.LogError($"Duplicate language found for [{language.systemLanguage}].");
                }
                else
                {
                    _languages.Add(language.systemLanguage, language);
                    SetDefaultLanguageIfApplicable(language);
                }
            }
        }

        private void SetDefaultLanguageIfApplicable(Language language)
        {
            if (!language.isDefaultLanguage)
            {
                return;
            }

            if (language.isDefaultLanguage && _defaultLanguage != null)
            {
                Debug.LogError("More than one language is marked as default.");
                return;
            }

            _defaultLanguage = language;
        }
    }
}