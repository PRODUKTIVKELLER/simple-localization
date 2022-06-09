using System.Collections.Generic;
using Produktivkeller.SimpleLocalization.Unity.Data;
using TMPro;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Fonts
{
    [CreateAssetMenu(fileName = "Overwrite Font", menuName = "PRODUKTIVKELLER/Simple Localization/Overwrite Font")]
    public class OverwriteFont : ScriptableObject
    {
        public LanguageId      languageId;
        public List<NamedFont> namedFonts;

        public TMP_FontAsset GetFontAsset(string fontAssetName)
        {
            for (int i = 0; i < namedFonts.Count; i++)
            {
                if (namedFonts[i].name == fontAssetName)
                {
                    return namedFonts[i].fontAsset;
                }
            }

            return null;
        }

        public string GetFontName(TMP_FontAsset fontAsset)
        {
            for (int i = 0; i < namedFonts.Count; i++)
            {
                if (namedFonts[i].fontAsset == fontAsset)
                {
                    return namedFonts[i].name;
                }
            }

            return "";
        }

        [ContextMenu("Load font names from original font.")]
        private void LoadFontNameFromOriginalFont()
        {
            namedFonts = new List<NamedFont>();

            OverwriteFontProvider.Instance.Refresh();
            
            foreach (NamedFont namedFont in OverwriteFontProvider.Instance.OriginalFont.namedFonts)
            {
                namedFonts.Add(new NamedFont { name = namedFont.name });
            }
        }
    }
}