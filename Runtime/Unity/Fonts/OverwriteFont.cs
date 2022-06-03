using System.Linq;
using TMPro;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Fonts
{
    [CreateAssetMenu(fileName = "OverwriteFont", menuName = "PRODUKTIVKELLER/SimpleLocalization/OverwriteFont")]
    public class OverwriteFont : ScriptableObject
    {
        public Language      language;
        public NamedFont[]   fontAssets;

        public OverwriteFont()
        {
            /*OriginalFont originalFont = Resources.LoadAll<OriginalFont>("").FirstOrDefault();

            if (originalFont != null)
            {
                fontAssets = new NamedFont[originalFonont.fontAssets.Length];
                for (int i = 0; i < originalFont.fontAssets.Length; i++)
                {
                    fontAssets[i] = new NamedFont()
                    { 
                        name = originalFont.fontAssets[i].name
                    };
                }
            }*/
        }

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
