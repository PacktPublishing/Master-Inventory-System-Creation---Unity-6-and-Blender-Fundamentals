using System.Collections.Generic;
using UnityEngine;

namespace Holistic3D.Inventory {

    public class InventorySystem : MonoBehaviour {

        public List<InventorySlot> slots = new List<InventorySlot>();
        public int maxSlots = 20;

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

        public void RemoveItem(ItemData itemData, int quantity) {

            InventorySlot slot = slots.Find(s => s.itemData == itemData);

            if (slot != null) {

                if (slot.quantity >= quantity) {

                    slot.quantity -= quantity;

                    if (slot.quantity <= 0) {

                        slot.ClearSlot();
                        slots.Remove(slot);
                    }
                }
            }
        }

        public bool IsFull() {

            return slots.Count >= maxSlots;
        }

        public void ClearInventory() {

            slots.Clear();
        }
    }
}