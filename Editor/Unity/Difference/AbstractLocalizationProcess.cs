using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Produktivkeller.SimpleLocalization.Excel;
using Produktivkeller.SimpleLocalization.Unity.Core;
using Produktivkeller.SimpleLogging;

namespace Produktivkeller.SimpleLocalization.Editor.Unity.Difference
{
    public abstract class AbstractLocalizationProcess
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        protected LanguageCache LoadLatestFileInDirectory(string path, string worksheet = null)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

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
                Log.Debug("No files found.");
                return null;
            }

            Log.Debug("Found file {}.", fileInfos[0].FullName);
            return LocalizationLoader.LoadConfigurationAndBuildLanguageCache(fileInfos[0].FullName, worksheet);
        }
    }
}