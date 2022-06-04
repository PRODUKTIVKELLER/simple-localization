using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity
{
    public interface ILocalized
    {
        GameObject gameObject { get; }

        void OnLanguageHasChanged();
    }
}