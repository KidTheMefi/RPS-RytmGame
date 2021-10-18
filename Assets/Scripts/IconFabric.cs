using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconFabric : MonoBehaviour
{
    public List<Icon> itemsPull;

    public Icon Create(int id)
    {
      
            return Instantiate(itemsPull.Find(op => op.id == id), gameObject.transform);
       // else Debug.LogWarning("No objects in IconFabric!");
       // return null;
    }

}
