using Produktivkeller.SimpleLocalization.Unity.Data;
using System;
using System.Collections.Generic;
namespace Produktivkeller.SimpleLocalization.Unity.Preprocessing
{
    [Serializable]
    public class LanguageIdTuple
    {
        public LanguageId              languageId;
        public List<LocalizedKeyTuple> localizedKeyTouples;

        public LanguageIdTuple(LanguageId languageId)
        {
            this.languageId     = languageId;
            localizedKeyTouples = new List<LocalizedKeyTuple>();
        }
    }
}
