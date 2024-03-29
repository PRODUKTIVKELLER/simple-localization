using System;
using System.Collections.Generic;
using System.Linq;
using Produktivkeller.SimpleLocalization.Unity.Data;
using Produktivkeller.SimpleLocalization.Unity.Preprocessing;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Core
{
    public class LanguageCache
    {
        private Dictionary<LanguageId, Dictionary<string, string>> _languageCache;

        public LanguageCache()
        {
            _languageCache = new Dictionary<LanguageId, Dictionary<string, string>>();
        }

        public void AddLanguage(LanguageId languageId)
        {
            _languageCache[languageId] = new Dictionary<string, string>();
        }

        public void AddEntry(LanguageId languageId, string key, string value)
        {
            _languageCache[languageId][key] = value;
        }

        public bool ContainsKey(LanguageId languageId, string key)
        {
            return _languageCache[languageId].ContainsKey(key);
        }

        public string GetKey(LanguageId languageId, string key)
        {
            return _languageCache[languageId][key];
        }

        public List<string> FindAllKeys()
        {
            List<string> keys = new List<string>();
            
            foreach (KeyValuePair<LanguageId, Dictionary<string, string>> languageColumn in _languageCache)
            {
                foreach (KeyValuePair<string, string> entry in languageColumn.Value)
                {
                    keys.Add(entry.Key);
                } 
            }

            return keys.Distinct().OrderBy(k => k).ToList();
        }

        public List<LanguageId> GetLanguageIds()
        {
            return _languageCache.Keys.ToList();
        }

        public List<(string, string)> GetAllKeysOfLanguage(LanguageId languageId)
        {
            return _languageCache[languageId].Select(x => (x.Key, x.Value)).ToList();
        }
    }
}