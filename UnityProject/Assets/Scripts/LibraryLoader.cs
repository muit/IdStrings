// Copyright 2015-2019 Miguel Fernandez Arce - All rights reserved
using IdStrings;
using UnityEngine;


public class LibraryLoader : MonoBehaviour
{
    public IdString myName;


    void Awake ()
    {
        IdString a = new IdString("ss");

        Debug.Log((string)a);
    }

    void OnDestroy() {
    }
}
