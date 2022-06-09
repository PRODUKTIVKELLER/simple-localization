using System;

namespace Produktivkeller.SimpleLocalization.Code_Patterns
{
    public abstract class Singleton<T> where T : Singleton<T>
    {
        public string Value { get; set; }

        private static readonly Lazy<T> PrivateInstance =
            new Lazy<T>(() =>
            {
                T t = Activator.CreateInstance(typeof(T), true) as T;
                t?.Initialize();
                return t;
            });

        public static T Instance
        {
            get => PrivateInstance.Value;
        }

        protected abstract void Initialize();
    }
}