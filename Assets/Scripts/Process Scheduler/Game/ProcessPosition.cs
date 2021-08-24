using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProcessPosition
{
    public bool isAvailable;
    public Vector3 position;

    public ProcessPosition(Vector3 position, bool isAvailable)
    {
        this.position = position;
        this.isAvailable = isAvailable;
    }
}
