using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Data
{
    [CreateAssetMenu(fileName = "Language", menuName = "PRODUKTIVKELLER/Simple Localization/Language")]
    public class Language : ScriptableObject
    {
        public SystemLanguage systemLanguage;
        public bool           isDefaultLanguage;
        
        public override string ToString()
        {
            return systemLanguage.ToString();
        }
    }
}