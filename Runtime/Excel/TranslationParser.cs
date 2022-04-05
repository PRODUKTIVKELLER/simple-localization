using System.Collections.Generic;
using System.Reflection;
using _modules._multi_language_support._scripts._unity;
using ExcelDataReader;
using Produktivkeller.SimpleLogging;

namespace _modules._multi_language_support._scripts._excel
{
    public class TranslationParser : AbstractParser
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private Dictionary<Language, Dictionary<string, string>> _languageCache;

        public void Parse(IExcelDataReader excelDataReader)
        {
            InitCache();
            IgnoreHeaderRows(excelDataReader);
            LoadKeysAndTranslations(excelDataReader);
        }

        public Dictionary<Language, Dictionary<string, string>> RetrieveLanguageCache()
        {
            return _languageCache;
        }

        private void InitCache()
        {
            _languageCache = new Dictionary<Language, Dictionary<string, string>>();

            _languageCache[Language.DE] = new Dictionary<string, string>();
            _languageCache[Language.EN] = new Dictionary<string, string>();
        }

        private void IgnoreHeaderRows(IExcelDataReader excelDataReader)
        {
            excelDataReader.Read();
            excelDataReader.Read();
        }

        private void LoadKeysAndTranslations(IExcelDataReader excelDataReader)
        {
            Log.Debug("Found {} translations in configuration file.", excelDataReader.RowCount);

            for (int i = 0; i < excelDataReader.RowCount; i++)
            {
                excelDataReader.Read();

                string key     = excelDataReader.GetString(0);
                string german  = excelDataReader.GetString(1);
                string english = excelDataReader.GetString(2);

                if (string.IsNullOrEmpty(key))
                {
                    continue;
                }

                key = key.Trim();

                if (string.IsNullOrEmpty(german))
                {
                    Log.Warn("German translation is missing for key {}.", key);
                }
                else
                {
                    german                           = german.Trim();
                    _languageCache[Language.DE][key] = german;
                }

                if (string.IsNullOrEmpty(english))
                {
                    Log.Warn("English translation is missing for key {}.", key);
                }
                else
                {
                    english                          = english.Trim();
                    _languageCache[Language.EN][key] = english;
                }
            }
        }
    }
}