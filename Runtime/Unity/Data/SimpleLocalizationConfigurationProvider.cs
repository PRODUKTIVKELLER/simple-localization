using System.Reflection;
using Produktivkeller.SimpleLocalization.Code_Patterns;
using Produktivkeller.SimpleLogging;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Data
{
    public class SimpleLocalizationConfigurationProvider : Singleton<SimpleLocalizationConfigurationProvider>
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);

        public SimpleLocalizationConfiguration SimpleLocalizationConfiguration { get; private set; }

        protected override void Initialize()
        {
            SimpleLocalizationConfiguration[] simpleLocalizationConfigurations = Resources.LoadAll<SimpleLocalizationConfiguration>("");

            if (simpleLocalizationConfigurations.Length == 0)
            {
                Log.Error("No configuration for 'Simple Localization' found in the 'Resources' folder.");
                return;
            }

            if (simpleLocalizationConfigurations.Length > 1)
            {
                Log.Error("More than one configuration found for 'Simple Localization' in the 'Resources' folder.");
                return;
            }

            SimpleLocalizationConfiguration = simpleLocalizationConfigurations[0];
        }
    }
}