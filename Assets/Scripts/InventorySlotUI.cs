using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IDropHandler
{
    public int slotIndex;
    public Inventory inventoryBackend;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        DraggableItem draggableItem = droppedObject.GetComponent<DraggableItem>();

        if (draggableItem != null)
        {
            InventorySlotUI oldSlot = draggableItem.parentAfterDrag.GetComponent<InventorySlotUI>();
            draggableItem.parentAfterDrag = transform;

            if (oldSlot != null)
                inventoryBackend.SwapItems(oldSlot.slotIndex, this.slotIndex);
        }
    }
}
