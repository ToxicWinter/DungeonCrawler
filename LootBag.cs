using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    private SpriteDatabase spriteDB;
    public PlayerController player;
    private int randomNumber;
    public EquippableItem LootItem;
    [SerializeField] EquippableItem[] lootItems;
    public int[] lootTable =
    {
        10, //Amulet
        10, //Boots
        10, //Armor
        10, //Helm
        10, //Ring
        10, //Shields
        10 //Weapon
    };
    public int total;
    [SerializeField] int itemStats;

    public int ItemStats { get => itemStats; set => itemStats = value; }

    void Start()
    {
        spriteDB = GameObject.Find("SpriteDB").GetComponent<SpriteDatabase>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        total = 0;
        randomNumber = 0;
        GenerateLoot();
        Debug.Log(itemStats);
    }

    public void GenerateLoot()
    {
        foreach (var item in lootTable)
        {
            total += item;
        }
        randomNumber = Random.Range(0, total);
        for (int i = 0; i <= lootTable.Length; i++)
        {
            if (randomNumber > lootTable[i])
            {
                randomNumber -= lootTable[i];
            }
            else
            {
                LootItem = Instantiate(lootItems[i]);
                
                if (itemStats == 0)
                    itemStats++;
                for (int j = 0; j < itemStats; j++)
                {
                    if (i == 0 || i == 6) //amulet or weapon
                    {
                        randomNumber = (int)Mathf.Floor(Random.value*5);
                        switch(randomNumber)
                        {
                            case 0:
                                LootItem.StrBonus++;
                                break;
                            case 1:
                                LootItem.DexBonus++;
                                break;
                            case 2:
                                LootItem.ConBonus++;
                                break;
                            case 3:
                                LootItem.MinDamage++;
                                break;
                            case 4:
                                LootItem.MaxDamage++;
                                break;
                        }
                    }
                    else
                    {
                        randomNumber = (int)Mathf.Floor(Random.value*3);
                        switch (randomNumber)
                        {
                            case 0:
                                LootItem.StrBonus++;
                                break;
                            case 1:
                                LootItem.DexBonus++;
                                break;
                            case 2:
                                LootItem.ConBonus++;
                                break;
                        }
                    }
                }
                LootItem.Level = LootItem.StrBonus + LootItem.DexBonus + LootItem.ConBonus + LootItem.MinDamage + LootItem.MaxDamage;
                switch (i)
                {
                    case 0:
                        LootItem.Icon = spriteDB.Amulets[Random.Range(0, spriteDB.Amulets.Length)];
                        LootItem.ItemName = LootItem.Icon.name + " " + LootItem.EquipmentType.ToString();
                        break;
                    case 1:
                        LootItem.Icon = spriteDB.Boots[Random.Range(0, spriteDB.Boots.Length)];
                        LootItem.ItemName = LootItem.Icon.name + " " + LootItem.EquipmentType.ToString();
                        break;
                    case 2:
                        LootItem.Icon = spriteDB.Chests[Random.Range(0, spriteDB.Chests.Length)];
                        LootItem.ItemName = LootItem.Icon.name + " " + LootItem.EquipmentType.ToString();
                        break;
                    case 3:
                        LootItem.Icon = spriteDB.Helms[Random.Range(0, spriteDB.Helms.Length)];
                        LootItem.ItemName = LootItem.Icon.name + " " + LootItem.EquipmentType.ToString();
                        break;
                    case 4:
                        LootItem.Icon = spriteDB.Rings[Random.Range(0, spriteDB.Rings.Length)];
                        LootItem.ItemName = LootItem.Icon.name + " " + LootItem.EquipmentType.ToString();
                        break;
                    case 5:
                        LootItem.Icon = spriteDB.Shields[Random.Range(0, spriteDB.Shields.Length)];
                        LootItem.ItemName = LootItem.Icon.name + " " + LootItem.EquipmentType.ToString();
                        break;
                    case 6:
                        LootItem.Icon = spriteDB.Weapons[Random.Range(0, spriteDB.Weapons.Length)];
                        LootItem.ItemName = LootItem.Icon.name;
                        break;
                }
                if (LootItem.StrBonus > LootItem.DexBonus && LootItem.StrBonus > LootItem.ConBonus)
                    LootItem.ItemName = LootItem.ItemName + " of Strength";
                else if (LootItem.DexBonus > LootItem.StrBonus && LootItem.DexBonus > LootItem.ConBonus)
                    LootItem.ItemName = LootItem.ItemName + " of Speed";
                else if (LootItem.ConBonus > LootItem.StrBonus && LootItem.ConBonus > LootItem.DexBonus)
                    LootItem.ItemName = LootItem.ItemName + " of Vitality";
                else if (LootItem.ConBonus == LootItem.StrBonus && LootItem.ConBonus > LootItem.DexBonus)
                    LootItem.ItemName = LootItem.ItemName + " of the Ogre";
                else if (LootItem.ConBonus > LootItem.StrBonus && LootItem.ConBonus == LootItem.DexBonus)
                    LootItem.ItemName = LootItem.ItemName + " of the Serpent";
                else if (LootItem.DexBonus == LootItem.StrBonus && LootItem.DexBonus > LootItem.ConBonus)
                    LootItem.ItemName = LootItem.ItemName + " of the Tiger";
                else if (LootItem.DexBonus == LootItem.StrBonus && LootItem.DexBonus == LootItem.ConBonus && (LootItem.MinDamage != 0 || LootItem.MaxDamage != 0))
                    LootItem.ItemName = LootItem.ItemName + " of the Fang";
                else
                    LootItem.ItemName = LootItem.ItemName + " of Balance";
                break;
            }
        }
    }
}
