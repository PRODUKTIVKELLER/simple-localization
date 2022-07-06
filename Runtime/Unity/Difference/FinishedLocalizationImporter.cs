using System.IO;
using System.Reflection;
using ClosedXML.Excel;
using Produktivkeller.SimpleLocalization.Excel;
using Produktivkeller.SimpleLocalization.Unity.Core;
using Produktivkeller.SimpleLocalization.Unity.Data;
using Produktivkeller.SimpleLogging;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Difference
{
    public class FinishedLocalizationImporter : AbstractLocalizationProcess
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        private LanguageCache _latestFinishedLocalizationLanguageCache;

        public void ImportLatest()
        {
            Log.Debug("Loading latest finished localization ...");
            _latestFinishedLocalizationLanguageCache = LoadLatestFinishedLocalization();

            Log.Debug("Importing latest finished localization ...");
            ImportLatestFinishedLocalization();

            Log.Debug("Finished importing.");
        }

        private LanguageCache LoadLatestFinishedLocalization()
        {
            return LoadLatestFileInDirectory(GetPathForFinishedLocalizations(), "Difference");
        }

        private static string GetPathForFinishedLocalizations()
        {
            return Application.dataPath + Path.DirectorySeparatorChar +
                   SimpleLocalizationConfigurationProvider.Instance.SimpleLocalizationConfiguration.pathForFinishedLocalizations;
        }

        private void ImportLatestFinishedLocalization()
        {
            using XLWorkbook xlWorkbook = new XLWorkbook(LocalizationLoader.GetConfigurationPath());
            IXLWorksheet xlWorksheet = xlWorkbook.Worksheet(SimpleLocalizationConfigurationProvider.Instance.SimpleLocalizationConfiguration.excelTableName);

            FillInFinishedLocalizations(xlWorksheet);

            xlWorkbook.Save();
            Log.Debug("Saved configuration.");
        }

        private void FillInFinishedLocalizations(IXLWorksheet xlWorksheet)
        {
            int lastRow     = xlWorksheet.LastRowUsed().RowNumber();
            int lastColumn  = SimpleLocalizationConfigurationProvider.Instance.SimpleLocalizationConfiguration.languageIds.Count + 1;
            int firstColumn = 2 + SimpleLocalizationConfigurationProvider.Instance.SimpleLocalizationConfiguration.sourceLanguageIds.Count;

            for (int row = 3; row <= lastRow; row++)
            {
                string key = xlWorksheet.Cell(row, 1).GetString();

                for (int column = firstColumn; column <= lastColumn; column++)
                {
                    LanguageId languageId = SimpleLocalizationConfigurationProvider.Instance.SimpleLocalizationConfiguration.languageIds[column - 2];

                    if (_latestFinishedLocalizationLanguageCache.ContainsKey(languageId, key))
                    {
                        string localization = _latestFinishedLocalizationLanguageCache.GetKey(languageId, key);

                        if (!string.IsNullOrWhiteSpace(localization))
                        {
                            xlWorksheet.Cell(row, column).Value = localization;
                            Log.Debug("Setting localization in row {} and column {} for language {} to:\n\n{}\n",
                                      row, column, languageId, localization);
                        }
                    }
                }
            }
        }
    }
}