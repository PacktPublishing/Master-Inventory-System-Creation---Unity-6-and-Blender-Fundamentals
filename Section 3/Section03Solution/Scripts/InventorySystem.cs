using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Holistic3D.Inventory {

    public class InventorySystem : MonoBehaviour {

        public List<InventorySlot> slots = new List<InventorySlot>();
        public int maxSlots = 5;

        public int AddItem(ItemData itemData, int quantity) {

            if (quantity <= 0) return 0;

            int remainingItems = quantity;

            if (itemData.isStackable) {

                foreach (InventorySlot slot in slots) {

                    if (slot.itemData == itemData) {

                        int spaceInStack = itemData.maxStackSize - slot.quantity;

                        if (spaceInStack > 0) {

                            int itemsToAdd = Mathf.Min(remainingItems, spaceInStack);
                            slot.quantity += itemsToAdd;
                            remainingItems -= itemsToAdd;

                            if (remainingItems <= 0) return 0;
                        }
                    }
                }
            }

            while (remainingItems > 0 && slots.Count < maxSlots) {

                int itemsToAdd = Mathf.Min(remainingItems, itemData.isStackable ? itemData.maxStackSize : 1);
                slots.Add(new InventorySlot(itemData, itemsToAdd));
                remainingItems -= itemsToAdd;
            }

            return remainingItems;
        }

        public int RemoveItem(ItemData itemData, int quantity, bool removePartial = true) {

            int remainingItems = quantity;
            List<InventorySlot> slotsWithItem = slots.Where(slots => slots.itemData == itemData).ToList();

            int totalAvailableItems = slotsWithItem.Sum(s => s.quantity);
            if (remainingItems > totalAvailableItems && !removePartial) return quantity;

            foreach (var slot in slotsWithItem) {

                if (remainingItems <= 0) break;

                if (slot.quantity < remainingItems) {

                    remainingItems -= slot.quantity;
                    slot.ClearSlot();
                    slots.Remove(slot);
                }
                else {

                    slot.quantity -= remainingItems;
                    remainingItems = 0;
                }
            }

            return remainingItems;
        }

        public void RemoveItemsFromSlots(int slotNumber) {

            slots.RemoveAt(slotNumber);
        }

        public bool IsFull() {

            return slots.Count >= maxSlots;
        }

        public void ClearInventory() {

            slots.Clear();
        }
    }
}