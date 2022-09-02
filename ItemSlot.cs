using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler,IPointerExitHandler, IPointerEnterHandler
{

    public event Action<Item> OnRightClickEvent;
    [SerializeField] Image image;
    [SerializeField] ItemToolTip tooltip;

    
    private Item item;
    public Item Item {
        get { return item;}
        set {
            item = value;
            if(item == null)
            {
                image.enabled = false;
            } else {
                image.sprite = item.Icon;
                image.enabled = true;
            }
        }
    }

    protected virtual void OnValidate()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }
        if (tooltip == null)
        {
            tooltip = FindObjectOfType<ItemToolTip>();
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData != null && eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log(OnRightClickEvent);
            if ( Item != null && OnRightClickEvent != null)
            {
                 OnRightClickEvent(Item);
            } 
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Item is EquippableItem)
        {
            tooltip.ShowTooltip((EquippableItem)Item);
        } 
    }

        public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip();
    }
}
