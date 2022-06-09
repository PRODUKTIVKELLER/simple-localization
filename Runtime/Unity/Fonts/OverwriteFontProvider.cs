using System.Collections.Generic;
using Produktivkeller.SimpleLocalization.Code_Patterns;
using Produktivkeller.SimpleLocalization.Unity.Data;
using TMPro;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Fonts
{
    internal class OverwriteFontProvider : Singleton<OverwriteFontProvider>
    {
        internal OriginalFont OriginalFont { get; private set; }

        private Dictionary<LanguageId, OverwriteFont> _overwriteFonts;

        protected override void Initialize()
        {
            Refresh();
        }

        internal void Refresh()
        {
            OverwriteFont[] overwriteFonts = Resources.LoadAll<OverwriteFont>("");
            _overwriteFonts = new Dictionary<LanguageId, OverwriteFont>();

            foreach (OverwriteFont overwriteFont in overwriteFonts)
            {
                if (overwriteFont.GetType() == typeof(OriginalFont))
                {
                    OriginalFont = (OriginalFont)overwriteFont;
                }
                else
                {
                    _overwriteFonts.Add(overwriteFont.languageId, overwriteFont);
                }
            }
        }

        private OverwriteFont GetOverwriteFont(LanguageId languageId)
        {
            if (!_overwriteFonts.ContainsKey(languageId))
            {
                return null;
            }

            return _overwriteFonts[languageId];
        }

        internal TMP_FontAsset GetOverwriteFontAsset(LanguageId languageId, TMP_FontAsset originalFontVariant)
        {
            OverwriteFont overwriteFont = GetOverwriteFont(languageId);
            if (overwriteFont == null)
            {
                return null;
            }

            return overwriteFont.GetFontAsset(OriginalFont.GetFontName(originalFontVariant));
        }
    }
}