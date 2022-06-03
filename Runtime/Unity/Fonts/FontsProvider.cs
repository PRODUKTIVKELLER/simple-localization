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
                _overwriteFonts.Add(overwriteFont.language, overwriteFont);
            }
        }

        public OverwriteFont GetOverwriteFont(Language language)
        {
            if (!_overwriteFonts.ContainsKey(language))
            {
                return null;
            }

            return _overwriteFonts[language];
        }

        public TMP_FontAsset GetOverwriteFontAsset(Language language)
        {
            OverwriteFont overwriteFont = GetOverwriteFont(language);
            if (overwriteFont == null)
            {
                return null;
            }

            return overwriteFont.fontAsset;
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
