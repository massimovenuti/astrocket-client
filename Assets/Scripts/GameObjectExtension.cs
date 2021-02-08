using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtension
{
    public static GameObject FindObjectByName(this GameObject parent, string name)
    {
        Transform parentTransform = parent.transform;
        GameObject found = parentTransform.Find(name) == null ? null : parentTransform.Find(name).gameObject;

        if (found != null)
        {
            return found;
        }
        else if(parentTransform.childCount > 0)
        {
            for (int i = 0; i < parentTransform.childCount; i++)
            {
                found = found ?? parentTransform.GetChild(i).gameObject.FindObjectByName(name);
            }
        }
        return found;
    }
}
