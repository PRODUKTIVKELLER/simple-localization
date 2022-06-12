using Produktivkeller.SimpleLocalization.Unity.Data;

namespace Produktivkeller.SimpleLocalization.Unity.Language_Recognition
{
    public class AlwaysUseSimplifiedChineseForAnyChineseVariant : LanguageRecognitionPostProcessor
    {
        public override LanguageId Process(LanguageId languageId)
        {
            if (languageId == LanguageId.Chinese || languageId == LanguageId.ChineseSimplified || languageId == LanguageId.ChineseTraditional)
            {
                return LanguageId.ChineseSimplified;
            }

            return languageId;
        }
    }
}