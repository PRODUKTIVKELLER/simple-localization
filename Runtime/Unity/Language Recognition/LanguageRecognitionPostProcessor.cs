using Produktivkeller.SimpleLocalization.Unity.Data;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Language_Recognition
{
    public abstract class LanguageRecognitionPostProcessor : MonoBehaviour
    {
        public abstract LanguageId Process(LanguageId languageId);
    }
}