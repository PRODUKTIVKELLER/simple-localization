using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Persistence
{
    public class SimpleLocalizationPersistence : ISimpleLocalizationPersistence
    {
        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        public float GetFloat(string key)
        {
            return PlayerPrefs.GetFloat(key);
        }

        public void Save()
        {
            PlayerPrefs.Save();
        }

        public void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public string GetString(string key)
        {
            return PlayerPrefs.GetString(key);
        }

        public void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }
    }
}