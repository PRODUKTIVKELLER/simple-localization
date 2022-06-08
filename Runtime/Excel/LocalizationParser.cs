using System.Collections.Generic;
using System.Reflection;
using ExcelDataReader;
using Produktivkeller.SimpleLocalization.Unity;
using Produktivkeller.SimpleLogging;

namespace Produktivkeller.SimpleLocalization.Excel
{
    public class TranslationParser
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);


        private LanguageCache _languageCache;

        public void Parse(IExcelDataReader excelDataReader)
        {
            InitCache();
            IgnoreHeaderRows(excelDataReader);
            LoadKeysAndLocalizations(excelDataReader);
        }

        public LanguageCache RetrieveLanguageCache()
        {
            return _languageCache;
        }

        private void InitCache()
        {
            _languageCache = new LanguageCache();

            _languageCache.AddLanguage(Language.DE);
            _languageCache.AddLanguage(Language.EN);
            _languageCache.AddLanguage(Language.ZH);
            _languageCache.AddLanguage(Language.ES);
            _languageCache.AddLanguage(Language.KO);
            _languageCache.AddLanguage(Language.NL);
            _languageCache.AddLanguage(Language.FR);
        }

        private void IgnoreHeaderRows(IExcelDataReader excelDataReader)
        {
            excelDataReader.Read();
            excelDataReader.Read();
        }

        private void LoadKeysAndLocalizations(IExcelDataReader excelDataReader)
        {
            List<string> warnings = new List<string>();
            int          count    = 0;

            for (int i = 0; i < excelDataReader.RowCount; i++)
            {
                excelDataReader.Read();

                string key               = excelDataReader.GetString(0);
                string german            = excelDataReader.GetString(1);
                string english           = excelDataReader.GetString(2);
                string french            = excelDataReader.GetString(3);
                string simplifiedChinese = excelDataReader.GetString(4);
                string dutch             = excelDataReader.GetString(5);
                string korean            = excelDataReader.GetString(6);
                string spanish           = excelDataReader.GetString(7);

                if (string.IsNullOrEmpty(key))
                {
                    continue;
                }

                count++;
                key = key.Trim();

                if (string.IsNullOrEmpty(german))
                {
                    warnings.Add($"German localization is missing for key [{key}].");
                }
                else
                {
                    german = german.Trim();
                    _languageCache.AddEntry(Language.DE, key, german);
                }

                if (string.IsNullOrEmpty(english))
                {
                    warnings.Add($"English localization is missing for key [{key}].");
                }
                else
                {
                    english = english.Trim();
                    _languageCache.AddEntry(Language.EN, key, english);
                }

                if (string.IsNullOrEmpty(french))
                {
                    warnings.Add($"Simplified French localization is missing for key [{key}].");
                }
                else
                {
                    french = french.Trim();
                    _languageCache.AddEntry(Language.FR, key, french);
                }

                if (string.IsNullOrEmpty(simplifiedChinese))
                {
                    warnings.Add($"Simplified Chinese localization is missing for key [{key}].");
                }
                else
                {
                    simplifiedChinese = simplifiedChinese.Trim();
                    _languageCache.AddEntry(Language.ZH, key, simplifiedChinese);
                }

                if (string.IsNullOrEmpty(dutch))
                {
                    warnings.Add($"Dutch localization is missing for key [{key}].");
                }
                else
                {
                    dutch = dutch.Trim();
                    _languageCache.AddEntry(Language.NL, key, dutch);
                }

                if (string.IsNullOrEmpty(korean))
                {
                    warnings.Add($"Korean localization is missing for key [{key}].");
                }
                else
                {
                    korean = korean.Trim();
                    _languageCache.AddEntry(Language.KO, key, korean);
                }

                if (string.IsNullOrEmpty(spanish))
                {
                    warnings.Add($"Spanish localization is missing for key [{key}].");
                }
                else
                {
                    spanish = spanish.Trim();
                    _languageCache.AddEntry(Language.ES, key, spanish);
                }
            }

            Log.Debug("Found {} translations in configuration file.", count);

            if (warnings.Count > 0)
            {
                Log.Warn("Encountered problems while reading configuration file:\n\n- " + string.Join("\n- ", warnings));
            }
        }
    }
}