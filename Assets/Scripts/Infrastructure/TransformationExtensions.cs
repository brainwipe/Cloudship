using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    public static List<GameObject> FindObjectsWithTag(this Transform parent, string tag)
    {
        List<GameObject> taggedGameObjects = new List<GameObject>();

        var childCount = parent.childCount;
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == tag)
            {
                taggedGameObjects.Add(child.gameObject);
            }
            if (child.childCount > 0)
            {
                taggedGameObjects.AddRange(FindObjectsWithTag(child, tag));
            }
        }
        return taggedGameObjects;
    }

    public static List<T> FindAllObjectsOfType<T>(this Transform parent)
    {
        List<T> result = new List<T>();

        var childCount = parent.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = parent.GetChild(i);
            var component = child.gameObject.GetComponent<T>();
            if (component != null)
            {
                result.Add(component);
            }
            if (child.childCount > 0)
            {
                result.AddRange(FindAllObjectsOfType<T>(child));
            }
        }

        return result;
    }
}
