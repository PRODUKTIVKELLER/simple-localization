using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using Produktivkeller.SimpleLocalization.Excel;
using Produktivkeller.SimpleLocalization.Unity.Core;
using Produktivkeller.SimpleLocalization.Unity.Data;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Difference
{
    public class LocalizationDifferenceGenerator
    {
        private LanguageCache _currentLanguageCache;
        private LanguageCache _latestDifferenceLanguageCache;

        public void Generate()
        {
            _currentLanguageCache          = LoadCurrentLocalizationFile();
            _latestDifferenceLanguageCache = LoadLatestDifferenceFile();

            BuildDifferenceFile();
        }

        private LanguageCache LoadLatestDifferenceFile()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetPathForDifferenceFiles());

            List<FileInfo> fileInfos = new List<FileInfo>();

            if (directoryInfo.Exists)
            {
                fileInfos = directoryInfo
                            .GetFiles()
                            .ToList()
                            .OrderByDescending(f => f.Name).ToList();
            }
            else
            {
                Directory.CreateDirectory(directoryInfo.FullName);
            }

            if (fileInfos.Count == 0)
            {
                return null;
            }

            return LocalizationLoader.LoadConfigurationAndBuildLanguageCache(fileInfos[0].FullName);
        }

        private static string GetPathForDifferenceFiles()
        {
            return Application.dataPath + Path.DirectorySeparatorChar + ConfigurationProvider.Instance.SimpleLocalizationConfiguration.pathForDifferenceFiles;
        }

        private LanguageCache LoadCurrentLocalizationFile()
        {
            return LocalizationLoader.LoadConfigurationAndBuildLanguageCache();
        }

        private void BuildDifferenceFile()
        {
            if (ThereIsNoDifferenceFileYet())
            {
                GenerateDifferenceFromCompleteLocalizationFile();
                return;
            }

            GenerateDifferenceByComparingLocalizationWithLatestDifferenceFile();
        }

        private void GenerateDifferenceByComparingLocalizationWithLatestDifferenceFile()
        {
            GenerateDifferenceFile(_latestDifferenceLanguageCache);
        }

        private void GenerateDifferenceFromCompleteLocalizationFile()
        {
            GenerateDifferenceFile(new LanguageCache());
        }

        private void GenerateDifferenceFile(LanguageCache latestDifferenceLanguageCache)
        {
            List<string> currentKeys          = _currentLanguageCache.FindAllKeys();
            List<string> latestDifferenceKeys = latestDifferenceLanguageCache.FindAllKeys();

            using XLWorkbook xlWorkbook = new XLWorkbook();

            AddWorksheets(xlWorkbook);

            xlWorkbook.SaveAs(GetPathForDifferenceFiles() + Path.DirectorySeparatorChar + $"Localization Difference - {DateTime.Now:yy-MM-dd HH-mm-ss}.xlsx");
        }

        private void AddWorksheets(XLWorkbook xlWorkbook)
        {
            using XLWorkbook currentConfigurationWorkbook = new XLWorkbook(LocalizationLoader.GetConfigurationPath());
            IXLWorksheet xlWorksheet = currentConfigurationWorkbook.Worksheet(ConfigurationProvider.Instance.SimpleLocalizationConfiguration.excelTableName);

            xlWorksheet.CopyTo(xlWorkbook, "Complete");
            IXLWorksheet differenceWorksheet = xlWorksheet.CopyTo(xlWorkbook, "Difference");

            for (int row = 3; row <= differenceWorksheet.LastRowUsed().RowNumber(); row++)
            {
                for (int column = 1; column <= ConfigurationProvider.Instance.SimpleLocalizationConfiguration.languageIds.Count + 1; column++)
                {
                    differenceWorksheet.Cell(row, column).Value = null;
                }
            }
        }

        private bool ThereIsNoDifferenceFileYet()
        {
            return _latestDifferenceLanguageCache == null;
        }
    }
}