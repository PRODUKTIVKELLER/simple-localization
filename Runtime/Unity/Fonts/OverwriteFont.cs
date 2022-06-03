using TMPro;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Fonts
{
    [CreateAssetMenu(fileName = "OverwriteFont", menuName = "PRODUKTIVKELLER/SimpleLocalization/OverwriteFont")]
    public class OverwriteFont : ScriptableObject
    {
        public Language      language;
        public TMP_FontAsset fontAsset;
    }
}
