using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
 
[Serializable]
public class CharacterStat 
{
    public int BaseValue;
    private readonly List<StatModifier> statModifiers;
    private bool isDirty = true;
    private int lastValue;
    private int lastBaseValue = 0;
    [SerializeField]
    public int Value {
        get 
        {
            if (isDirty || BaseValue != lastBaseValue)
            {
                lastBaseValue = BaseValue;
                lastValue = CalculateFinalValue();
                isDirty = false;
            }
            return lastValue;
        }
    }
    
    public CharacterStat()
    {
        statModifiers = new List<StatModifier>();
    } 

    public CharacterStat(int baseValue) : this()
    {
        BaseValue = baseValue;
        statModifiers = new List<StatModifier>();
    } 
    public void AddModifier(StatModifier mod)
    {
        isDirty = true;
        statModifiers.Add(mod);
    }
    
    public void RemoveModifier(StatModifier mod)
    {
        isDirty = true;
        statModifiers.Remove(mod);
    }
    
    private int CalculateFinalValue()
    {
        int finalValue = BaseValue;
        for (int i = 0; i < statModifiers.Count; i++)
        {
            finalValue += statModifiers[i].Value;
        }
        return finalValue;
    } 

    public bool RemoveAllModifiersFromSource(object source)
    {
        bool didRemove = false;
        for(int i = statModifiers.Count - 1; i>= 0 ; i--)
        {
            if(statModifiers[i].Source == source)
            {
                isDirty = true;
                didRemove = true;
                statModifiers.RemoveAt(i);
            }
        }
        return didRemove;
    }
}
