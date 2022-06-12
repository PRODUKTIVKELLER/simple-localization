using System.Collections.Generic;
using System.Reflection;
using ClosedXML.Excel;
using Produktivkeller.SimpleLocalization.Unity.Core;
using Produktivkeller.SimpleLocalization.Unity.Data;
using Produktivkeller.SimpleLogging;

namespace Produktivkeller.SimpleLocalization.Excel
{
    internal class LocalizationParser
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        private LanguageCache _languageCache;

        public void Parse(IXLWorksheet xlWorksheet)
        {
            InitCache();
            LoadKeysAndLocalizations(xlWorksheet);
        }

        public LanguageCache RetrieveLanguageCache()
        {
            return _languageCache;
        }

        private void InitCache()
        {
            _languageCache = new LanguageCache();

            foreach (LanguageId languageId in SimpleLocalizationConfigurationProvider.Instance.SimpleLocalizationConfiguration.languageIds)
            {
                _languageCache.AddLanguage(languageId);
            }
        }

        private void LoadKeysAndLocalizations(IXLWorksheet xlWorksheet)
        {
            List<string> warnings = new List<string>();
            int          count    = 0;

            for (int row = 3; row <= xlWorksheet.LastRowUsed().RowNumber(); row++)
            {
                string key = xlWorksheet.Cell(row, 1).GetString();

                if (string.IsNullOrEmpty(key))
                {
                    continue;
                }

                count++;
                key = key.Trim();

                ProcessRow(xlWorksheet, warnings, key, row);
            }

            Log.Debug("Found {} entries in configuration file.", count);

            if (warnings.Count > 0)
            {
                Log.Warn("Encountered problems while reading configuration file:\n\n- " + string.Join("\n- ", warnings) + "\n");
            }
        }

        private void ProcessRow(IXLWorksheet xlWorksheet, List<string> warnings, string key, int row)
        {
            int column = 2;
            foreach (LanguageId languageId in SimpleLocalizationConfigurationProvider.Instance.SimpleLocalizationConfiguration.languageIds)
            {
                string translation = xlWorksheet.Cell(row, column).GetString();

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