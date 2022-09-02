using UnityEngine;

public enum EquipmentType
{
    Helmet,
    Armor,
    Boots,
    Amulet,
    Weapon,
    Shield,
    Ring,
}


[CreateAssetMenu]
public class EquippableItem : Item
{
    public int StrBonus;
    public int DexBonus;
    public int ConBonus;
    public int MinDamage;
    public int MaxDamage;
    public EquipmentType EquipmentType;
    
    public void Equip(PlayerController c)
    {
        if (StrBonus != 0 )
        {
            c.Strength.AddModifier(new StatModifier(StrBonus, this));
        }
        if (DexBonus != 0 )
        {
            c.Dexterity.AddModifier(new StatModifier(DexBonus, this));
        }
        if (ConBonus != 0 )
        {
            c.Constitution.AddModifier(new StatModifier(ConBonus, this));
        }
        if ( MinDamage != 0)
        {
            c.weaponDamageMin = c.weaponDamageMin + MinDamage;
        }
        if ( MaxDamage != 0)
        {
            c.weaponDamageMax = c.weaponDamageMax + MaxDamage; 
        }
    }

    public void Unequip(PlayerController c)
    {
        c.Strength.RemoveAllModifiersFromSource(this);
        c.Dexterity.RemoveAllModifiersFromSource(this);
        c.Constitution.RemoveAllModifiersFromSource(this);
        c.weaponDamageMin = c.weaponDamageMin - MinDamage; 
        c.weaponDamageMax = c.weaponDamageMax - MaxDamage;
    }
}

