using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class ItemToolTip : MonoBehaviour
{
    [SerializeField] Text ItemNameText;
    [SerializeField] Text ItemTypeText;
    [SerializeField] Text ItemStatsText;
    [SerializeField] Text ItemLevelText;
    [SerializeField] EquipmentPanel equipped;
    private EquippableItem equippedItem;
    private float totalBonus;

    private StringBuilder sb = new StringBuilder();

    public void ShowTooltip( EquippableItem item)
    {
        for (int i = 0; i < equipped.equipmentSlots.Length; i++)
            {
                if (equipped.equipmentSlots[i].EquipmentType == item.EquipmentType)
                {
                    if (equipped.equipmentSlots[i].Item != null)
                    {
                        equippedItem = (EquippableItem)equipped.equipmentSlots[i].Item;
                        break;
                    }
                }
            }
        ItemNameText.text = item.ItemName;
        ItemTypeText.text = item.EquipmentType.ToString();
        ItemLevelText.text = "Item Level: " + item.Level.ToString();
        sb.Length = 0;
        if( equippedItem == null)
        {
            AddStat(item.StrBonus, "Strength");
            AddStat(item.DexBonus, "Dexterity");
            AddStat(item.ConBonus, "Constitution");
            AddStat(item.MinDamage, "Min Damage");
            AddStat(item.MaxDamage, "Max Damage");
        }
        else
        {
            AddStat(item.StrBonus, equippedItem.StrBonus, "Strength");
            AddStat(item.DexBonus, equippedItem.DexBonus, "Dexterity");
            AddStat(item.ConBonus, equippedItem.ConBonus, "Constitution");
            AddStat(item.MinDamage, equippedItem.MinDamage, "Min Damage");
            AddStat(item.MaxDamage, equippedItem.MaxDamage, "Max Damage");
        }
        
        ItemStatsText.text = sb.ToString();
        equippedItem = null;
        gameObject.SetActive(true); 
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false); 
    }

    private void AddStat(float value, string statName)
    {
        if(value !=0)
        {
            if (sb.Length > 0)
                sb.AppendLine();
            sb.Append(value);
            sb.Append(" ");
            sb.Append(statName);
            
        }
    }
    private void AddStat(float itemValue, float equippedValue, string statName)
    {
        if (itemValue != 0)
        {
            if (sb.Length > 0)
                sb.AppendLine();
            sb.Append(itemValue);
            sb.Append(" ");
            sb.Append(statName);
            sb.Append("   ");
            totalBonus = itemValue - equippedValue;
            if (totalBonus < 0)
                sb.Append("-");
            else if (totalBonus >0)
                sb.Append("+");
            else
                sb.Append("");
            sb.Append(totalBonus);
        }
        if(itemValue == 0 && equippedValue !=0)
        {
            if (sb.Length > 0)
                sb.AppendLine();
            sb.Append("0 "); 
            sb.Append(statName);
            sb.Append("   -");
            sb.Append(equippedValue);  
        }
    }

}
