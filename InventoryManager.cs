using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;
    [SerializeField] UIController userInterface;
    [SerializeField] PlayerController player;
    
    public void Awake()
    {
        inventory.OnItemRightClickedEvent += EquipFromInventory;
        equipmentPanel.OnRightClickedEvent += UnequipFromEquipPanel;
    }

    public void EquipFromInventory(Item item)
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            DeleteItem(item);
        }
        else 
        {
            if(item is EquippableItem)
            {
                Equip((EquippableItem)item);
            }
        }
        
    }
    private void UnequipFromEquipPanel(Item item)
    {
        if(item is EquippableItem)
        {
            Unequip((EquippableItem)item);
        }
    }

    public void Equip(EquippableItem item)
    {
        if(inventory.RemoveItem(item))
        {
            EquippableItem previousItem;
            if(equipmentPanel.AddItem(item, out previousItem))
            {
                if ( previousItem != null)
                {
                    inventory.AddItem(previousItem);
                    previousItem.Unequip(player);
                    player.isDirtyHP = true;
                    userInterface.UpdateStats();
                }
                item.Equip(player);
                player.isDirtyHP = true;
                userInterface.UpdateStats();
            }
            else
            {
                inventory.AddItem(item);
            }
        }
    }
    public void Unequip(EquippableItem item)
    {
        if (!inventory.IsFull() && equipmentPanel.RemoveItem(item))
        {
            item.Unequip(player);
            inventory.AddItem(item);
            player.isDirtyHP = true;
            userInterface.UpdateStats();
        }
    }
    public void DeleteItem(Item item)
    {
        inventory.RemoveItem(item);
    }
    
}
