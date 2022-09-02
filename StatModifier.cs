using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModifier
{
    public readonly int Value;
    public readonly object Source;
    
    public StatModifier(int value, object source)
    {
        Value = value;
        Source = source;
    }
        public StatModifier(int value)
    {
        Value = value;
    }  
}

