using System.Collections.Generic;
using System.Reflection;
using ExcelDataReader;
using Produktivkeller.SimpleLocalization.Unity;
using Produktivkeller.SimpleLocalization.Unity.Data;
using Produktivkeller.SimpleLogging;

namespace Produktivkeller.SimpleLocalization.Excel
{
    internal class LocalizationParser
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

            foreach (LanguageId languageId in ConfigurationProvider.Instance.SimpleLocalizationConfiguration.languageIds)
            {
                _languageCache.AddLanguage(languageId);
            }
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

                string key = excelDataReader.GetString(0);
                
                if (string.IsNullOrEmpty(key))
                {
                    continue;
                }
                
                count++;
                key = key.Trim();
                
                ProcessRow(excelDataReader, warnings, key);
            }

            Log.Debug("Found {} entries in configuration file.", count);

            if (warnings.Count > 0)
            {
                Log.Warn("Encountered problems while reading configuration file:\n\n- " + string.Join("\n- ", warnings));
            }
        }

        private void ProcessRow(IExcelDataReader excelDataReader, List<string> warnings, string key)
        {
            int column = 1;
            foreach (LanguageId languageId in ConfigurationProvider.Instance.SimpleLocalizationConfiguration.languageIds)
            {
                string translation = excelDataReader.GetString(column);

                if (string.IsNullOrEmpty(translation))
                {
                    warnings.Add($"{languageId.ToString().ToUpper()} localization is missing for key [{key}].");
                }
                else
                {
                    translation = translation.Trim();
                    _languageCache.AddEntry(languageId, key, translation);
                }

                column++;
            }
        }
    }
}