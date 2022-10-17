using Produktivkeller.SimpleLocalization.Editor.Unity.Difference;
using Produktivkeller.SimpleLocalization.Unity.Core;
using UnityEditor;
using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Editor.Unity.Core
{
    [CustomEditor(typeof(LocalizationService))]
    public class LocalizationServiceEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Generate localization difference"))
            {
                new LocalizationDifferenceGenerator().Generate();
            }

            if (GUILayout.Button("Import latest finished localization"))
            {
                new FinishedLocalizationImporter().ImportLatest();
            }
        }
    }
}