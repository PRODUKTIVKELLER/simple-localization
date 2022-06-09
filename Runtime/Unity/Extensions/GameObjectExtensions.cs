using UnityEngine;

namespace Produktivkeller.SimpleLocalization.Unity.Extensions
{
    public static class GameObjectExtensions
    {
        public static string BuildFullName (this GameObject gameObject) {
            
            string name = gameObject.name;
            
            while (gameObject.transform.parent != null) {

                gameObject   = gameObject.transform.parent.gameObject;
                name = gameObject.name + "/" + name;
            }
            return name;
        }
    }
}