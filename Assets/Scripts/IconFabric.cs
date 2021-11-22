using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconFabric : MonoBehaviour
{
    public List<Icon> iconPull;


    

    public Icon Create(IconSpawnIdClass idClass) 
    {
        if (iconPull.Count != 0)
        {
            return Instantiate(iconPull.Find(op => op.iconSpawnClass == idClass), gameObject.transform);
        }
        else Debug.LogWarning("No such object in IconFabric!");
        return null;
    }

}
