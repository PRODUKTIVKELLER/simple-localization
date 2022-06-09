using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Components
{
    public interface ILocalized
    {
        GameObject gameObject { get; }

        void OnLanguageHasChanged();
    }
}