using System.Collections;
using Produktivkeller.SimpleLocalization.Unity.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Produktivkeller.SimpleLocalization.Unity.Utils
{
    public class RebuildLayoutOnLanguageChange : MonoBehaviour, ILocalized
    {
        public void OnLanguageHasChanged()
        {
            StartCoroutine(RebuildLayout());
        }

        private IEnumerator RebuildLayout()
        {
            yield return null;
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }
    }
}