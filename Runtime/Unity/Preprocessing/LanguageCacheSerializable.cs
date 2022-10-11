using Produktivkeller.SimpleLocalization.Unity.Core;
using Produktivkeller.SimpleLocalization.Unity.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Preprocessing
{
    [CreateAssetMenu(fileName = "Language Cache Serializable", menuName = "PRODUKTIVKELLER/Simple Localization/Language Cache Serializable")]
    public class LanguageCacheSerializable : ScriptableObject
    {
        [SerializeField] public List<LanguageIdTuple> languageIdTuples;

        public static LanguageCacheSerializable FromLanguageCache(LanguageCache languageCache)
        {
            LanguageCacheSerializable languageCacheSerializable = new LanguageCacheSerializable();
            languageCacheSerializable.languageIdTuples = new List<LanguageIdTuple>();

            List<LanguageId> languageIds = languageCache.LanguageIds;
            for (int i = 0; i < languageIds.Count; i++)
            {
                LanguageIdTuple languageIdTuple = new LanguageIdTuple(languageIds[i]);
                languageCacheSerializable.languageIdTuples.Add(languageIdTuple);

                List<(string, string)> allKeys = languageCache.GetAllKeysOfLanguage(languageIds[i]);
                for (int j = 0; j < allKeys.Count; j++)
                {
                    languageIdTuple.localizedKeyTouples.Add(new LocalizedKeyTuple()
                    {
                        localizationKey   = allKeys[j].Item1,
                        localizationValue = allKeys[j].Item2
                    });
                }
            }

            return languageCacheSerializable;
        }

        public LanguageCache ToLanguageCache()
        {
            LanguageCache languageCache = new LanguageCache();
            for (int i = 0; i < languageIdTuples.Count; i++)
            {
                LanguageIdTuple languageIdTuple = languageIdTuples[i];
                languageCache.AddLanguage(languageIdTuple.languageId);

                for (int j = 0; j < languageIdTuple.localizedKeyTouples.Count; j++)
                {
                    languageCache.AddEntry(languageIdTuple.languageId,
                        languageIdTuple.localizedKeyTouples[j].localizationKey,
                        languageIdTuple.localizedKeyTouples[j].localizationValue);
                }
            }

            return languageCache;
        }
    }
}
