using Produktivkeller.SimpleLocalization.Unity.Core;
using Produktivkeller.SimpleLocalization.Unity.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Preprocessing
{
    public class LanguageCacheSerializable : ScriptableObject
    {
        [SerializeField] public List<LocalizedTextsForLanguage> localizedTextsForLanguages;

        public static LanguageCacheSerializable FromLanguageCache(LanguageCache languageCache)
        {
            LanguageCacheSerializable languageCacheSerializable = CreateInstance<LanguageCacheSerializable>();
            languageCacheSerializable.localizedTextsForLanguages = new List<LocalizedTextsForLanguage>();

            List<LanguageId> languageIds = languageCache.GetLanguageIds();
            foreach (LanguageId languageId in languageIds)
            {
                LocalizedTextsForLanguage localizedTextsForLanguage = new LocalizedTextsForLanguage(languageId);
                languageCacheSerializable.localizedTextsForLanguages.Add(localizedTextsForLanguage);

                List<(string, string)> allKeys = languageCache.GetAllKeysOfLanguage(languageId);
                for (int j = 0; j < allKeys.Count; j++)
                {
                    localizedTextsForLanguage.keyAndLocalizedTextList.Add(new KeyAndLocalizedText
                    {
                        key   = allKeys[j].Item1,
                        localizedText = allKeys[j].Item2
                    });
                }
            }

            return languageCacheSerializable;
        }

        public LanguageCache ToLanguageCache()
        {
            LanguageCache languageCache = new LanguageCache();
            
            foreach (LocalizedTextsForLanguage languageIdTuple in localizedTextsForLanguages)
            {
                languageCache.AddLanguage(languageIdTuple.languageId);

                foreach (KeyAndLocalizedText localizedKeyTuple in languageIdTuple.keyAndLocalizedTextList)
                {
                    languageCache.AddEntry(languageIdTuple.languageId,
                                           localizedKeyTuple.key,
                                           localizedKeyTuple.localizedText);
                }
            }

            return languageCache;
        }
    }
}
