using System.Collections.Generic;
using System.Reflection;
using ExcelDataReader;
using Produktivkeller.SimpleLocalization.Unity;
using Produktivkeller.SimpleLogging;

namespace Produktivkeller.SimpleLocalization.Excel
{
    public class TranslationParser
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


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
            _languageCache.AddLanguage(Language.CHN);
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

                string key     = excelDataReader.GetString(0);
                string german  = excelDataReader.GetString(1);
                string english = excelDataReader.GetString(2);
                string chinese = excelDataReader.GetString(3);

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

                if (string.IsNullOrEmpty(chinese))
                {
                    warnings.Add($"Chinese localization is missing for key [{key}].");
                }
                else
                {
                    chinese = chinese.Trim();
                    _languageCache.AddEntry(Language.CHN, key, chinese);
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