using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ClosedXML.Excel;
using Produktivkeller.SimpleLocalization.Excel;
using Produktivkeller.SimpleLocalization.Unity.Core;
using Produktivkeller.SimpleLocalization.Unity.Data;
using Produktivkeller.SimpleLogging;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Difference
{
    public class LocalizationDifferenceGenerator
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        private LanguageCache _currentLanguageCache;
        private LanguageCache _latestDifferenceLanguageCache;
        private List<string>  _currentKeys;
        private List<string>  _latestDifferenceKeys;
        private bool          _differenceContainsEntries;

        public void Generate()
        {
            _currentLanguageCache          = LoadCurrentLocalizationFile();
            _latestDifferenceLanguageCache = LoadLatestDifferenceFile();

            BuildDifferenceFile();
            
            Log.Debug("Finished generating localization difference.");
        }

        private LanguageCache LoadLatestDifferenceFile()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(GetPathForDifferenceFiles());

            List<FileInfo> fileInfos = new List<FileInfo>();

            if (directoryInfo.Exists)
            {
                fileInfos = directoryInfo
                            .GetFiles()
                            .Where(f => !f.Name.EndsWith(".meta"))
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
            return Application.dataPath + Path.DirectorySeparatorChar + SimpleLocalizationConfigurationProvider.Instance.SimpleLocalizationConfiguration.pathForDifferenceFiles;
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
            _currentKeys          = _currentLanguageCache.FindAllKeys();
            _latestDifferenceKeys = latestDifferenceLanguageCache.FindAllKeys();

            using XLWorkbook xlWorkbook = new XLWorkbook();

            AddWorksheets(xlWorkbook);

            if (!_differenceContainsEntries)
            {
                Log.Debug("No differences found.");
                return;
            }
            
            string path = GetPathForDifferenceFiles() + Path.DirectorySeparatorChar + $"Localization Difference - {DateTime.Now:yy-MM-dd HH-mm-ss}.xlsx";
            xlWorkbook.SaveAs(path);
            Log.Debug("Saved new localization difference: {}", path);
        }

        private void AddWorksheets(XLWorkbook xlWorkbook)
        {
            using XLWorkbook currentConfigurationWorkbook = new XLWorkbook(LocalizationLoader.GetConfigurationPath());
            IXLWorksheet xlWorksheet = currentConfigurationWorkbook.Worksheet(SimpleLocalizationConfigurationProvider.Instance.SimpleLocalizationConfiguration.excelTableName);

            xlWorksheet.CopyTo(xlWorkbook, SimpleLocalizationConfigurationProvider.Instance.SimpleLocalizationConfiguration.excelTableName);
            IXLWorksheet differenceWorksheet = xlWorksheet.CopyTo(xlWorkbook, "Difference");

            for (int row = 3; row <= differenceWorksheet.LastRowUsed().RowNumber(); row++)
            {
                for (int column = 1; column <= SimpleLocalizationConfigurationProvider.Instance.SimpleLocalizationConfiguration.languageIds.Count + 1; column++)
                {
                    differenceWorksheet.Cell(row, column).Value = null;
                }
            }

            AddDifferenceEntries(differenceWorksheet);
        }

        private void AddDifferenceEntries(IXLWorksheet xlWorksheet)
        {
            List<LanguageId> sourceLanguageIds = SimpleLocalizationConfigurationProvider.Instance.SimpleLocalizationConfiguration.sourceLanguageIds;

            foreach (string currentKey in _currentKeys)
            {
                if (!_latestDifferenceKeys.Contains(currentKey))
                {
                    Log.Debug("Found new key {}.", currentKey);
                    AddToDiff(currentKey, xlWorksheet);
                    continue;
                }

                foreach (LanguageId sourceLanguageId in sourceLanguageIds)
                {
                    string newText = _currentLanguageCache.GetKey(sourceLanguageId, currentKey);
                    string oldText = _latestDifferenceLanguageCache.GetKey(sourceLanguageId, currentKey);

                    if (newText != oldText)
                    {
                        Log.Debug("Source text for key {} in language {} has changed.", currentKey, sourceLanguageId);
                        AddToDiff(currentKey, xlWorksheet);
                        break;
                    }
                }
            }
        }

        private void AddToDiff(string key, IXLWorksheet xlWorksheet)
        {
            _differenceContainsEntries = true;
            
            List<LanguageId> sourceLanguageIds = SimpleLocalizationConfigurationProvider.Instance.SimpleLocalizationConfiguration.sourceLanguageIds;
            List<LanguageId> languageIds       = SimpleLocalizationConfigurationProvider.Instance.SimpleLocalizationConfiguration.languageIds;

            int row = Mathf.Max(xlWorksheet.LastRowUsed().RowNumber() + 1, 3);

            xlWorksheet.Cell(row, 1).Value = key;

            foreach (LanguageId sourceLanguageId in sourceLanguageIds)
            {
                int column = languageIds.IndexOf(sourceLanguageId) + 2;
                xlWorksheet.Cell(row, column).Value = _currentLanguageCache.GetKey(sourceLanguageId, key);
            }
        }

        private bool ThereIsNoDifferenceFileYet()
        {
            return _latestDifferenceLanguageCache == null;
        }
    }
}