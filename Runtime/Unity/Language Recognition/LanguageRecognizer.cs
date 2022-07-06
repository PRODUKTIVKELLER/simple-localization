using System.Reflection;
using Produktivkeller.SimpleLocalization.Unity.Data;
using Produktivkeller.SimpleLogging;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Language_Recognition
{
    public abstract class LanguageRecognizer : MonoBehaviour
    {
        protected static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        [Tooltip("Higher priorities are evaluated first.")] [SerializeField]
        private int priority;

        public abstract LanguageId Recognize();

        internal int GetPriority()
        {
            return priority;
        }
    }
}