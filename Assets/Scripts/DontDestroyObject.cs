using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class DontDestroyObject : MonoBehaviour
{
    private static List<DontDestroyObject> list = new();

    private void Awake()
    {
        foreach (DontDestroyObject obj in list)
        {
            if(obj.name == this.name)
            {
                Destroy(gameObject);
                return;
            }
        }

        DontDestroyOnLoad(gameObject);
        list.Add(this);
    }
}
