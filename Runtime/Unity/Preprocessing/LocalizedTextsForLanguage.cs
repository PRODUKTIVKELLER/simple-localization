using System;
using System.Collections.Generic;
using Produktivkeller.SimpleLocalization.Unity.Data;

namespace Produktivkeller.SimpleLocalization.Unity.Preprocessing
{
    [Serializable]
    public class LocalizedTextsForLanguage
    {
        public LanguageId                languageId;
        public List<KeyAndLocalizedText> keyAndLocalizedTextList;

        public LocalizedTextsForLanguage(LanguageId languageId)
        {
            this.languageId         = languageId;
            keyAndLocalizedTextList = new List<KeyAndLocalizedText>();
        }
    }
}