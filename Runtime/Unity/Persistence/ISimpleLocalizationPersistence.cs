namespace Produktivkeller.SimpleLocalization.Persistence
{
    public interface ISimpleLocalizationPersistence
    {
        public bool HasKey(string key);

        public void SetFloat(string key, float value);

        public float GetFloat(string key);

        public void SetString(string key, string value);

        public string GetString(string key);

        public void DeleteKey(string key);

        public void Save();
    }
}