using System;

namespace Produktivkeller.SimpleLocalization
{
    public abstract class Singleton<T> where T : class
    {
        public string Value { get; set; }

        private static readonly Lazy<T> PrivateInstance =
            new Lazy<T>(() => Activator.CreateInstance(typeof(T), true) as T);

        public static T Instance
        {
            get => PrivateInstance.Value;
        }

        protected abstract void Initialize();
    }
}