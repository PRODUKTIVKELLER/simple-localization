using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Fonts
{
    public class FontsProvider
    {
        private Dictionary<Language, OverwriteFont> _overwriteFonts;

        public FontsProvider(OverwriteFont[] overwriteFonts)
        {
            _overwriteFonts = new Dictionary<Language, OverwriteFont>();

            foreach (OverwriteFont overwriteFont in overwriteFonts)
            {
                if (overwriteFont.GetType() == typeof(OriginalFont))
                {
                    OriginalFont = (OriginalFont)overwriteFont;
                }
                else
                {
                    _overwriteFonts.Add(overwriteFont.language, overwriteFont);
                }
            }
        }

        protected OriginalFont OriginalFont
        {
            get; set;
        }

        public OverwriteFont GetOverwriteFont(Language language)
        {
            if (!_overwriteFonts.ContainsKey(language))
            {
                return null;
            }

            return _overwriteFonts[language];
        }

        public TMP_FontAsset GetOverwriteFontAsset(Language language, TMP_FontAsset originalFontVariant)
        {
            OverwriteFont overwriteFont = GetOverwriteFont(language);
            if (overwriteFont == null)
            {
                return null;
            }

            return overwriteFont.GetFontAsset(OriginalFont.GetFontName(originalFontVariant));
        }


        private static FontsProvider _instance;
        public static FontsProvider Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                _instance = new FontsProvider(Resources.LoadAll<OverwriteFont>(""));
                return _instance;
            }
        }
    }
}
