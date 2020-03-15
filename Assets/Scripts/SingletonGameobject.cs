using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonGameobject<T> : MonoBehaviour where T: Component
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }

            var obj = new GameObject(typeof(T).Name);
            instance = obj.AddComponent<T>();
            return instance;
        }

        protected set
        {
            instance = value;
        }
    }
}
