﻿using UnityEngine;

public class Test : MonoBehaviour
{
    private void Awake()
    {
        // IFoo foo = new Bar();
        //
        // Debug.Log(foo as IBar == null);
    }

    public interface IFoo
    {
        
    }

    public interface IBar : IFoo
    {
        
    }
    
    public class Bar : IBar
    {
        
    }
}