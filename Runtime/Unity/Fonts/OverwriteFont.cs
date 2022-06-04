using System.Linq;
using TMPro;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Fonts
{
    [CreateAssetMenu(fileName = "Overwrite Font", menuName = "PRODUKTIVKELLER/Simple Localization/Overwrite Font")]
    public class OverwriteFont : ScriptableObject
    {
        public Language      language;
        public NamedFont[]   fontAssets;

        public TMP_FontAsset GetFontAsset(string name)
        {
            for (int i = 0; i < fontAssets.Length; i++)
            {
                if (fontAssets[i].name == name)
                {
                    return fontAssets[i].fontAsset;
                }
            }

            return null;
        }

        public string GetFontName(TMP_FontAsset fontAsset)
        {
            for (int i = 0; i < fontAssets.Length; i++)
            {
                if (fontAssets[i].fontAsset == fontAsset)
                {
                    return fontAssets[i].name;
                }
            }

            return "";
        }
    }
}
