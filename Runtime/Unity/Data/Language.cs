using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Data
{
    [CreateAssetMenu(fileName = "Language", menuName = "PRODUKTIVKELLER/Language")]
    public class Language : ScriptableObject
    {
        public string countryIdentifier;
        public bool   isDefaultLanguage;


        public override string ToString()
        {
            return countryIdentifier;
        }
    }
}
