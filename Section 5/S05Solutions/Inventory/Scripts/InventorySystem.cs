using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Holistic3D.Inventory {

    public class InventorySystem : MonoBehaviour {

        public List<InventorySlot> slots = new List<InventorySlot>();
        public int maxSlots = 5;
        public PlayerInventorySystem playerInventorySystem;

        public int AddItem(ItemData itemData, int quantity) {

            if (quantity <= 0) return 0;

            int remainingItems = quantity;

            if (itemData.isStackable) {

                foreach (InventorySlot slot in slots) {

                    if (slot.itemData == itemData) {

                        int spaceInStack = itemData.maxStackSize - slot.Quantity;

                        if (spaceInStack > 0) {

                            int itemsToAdd = Mathf.Min(remainingItems, spaceInStack);
                            slot.Quantity += itemsToAdd;
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
            List<InventorySlot> slotsWithItem = slots.Where(s => s.itemData == itemData).ToList();

            int totalAvailableItems = slotsWithItem.Sum(s => s.Quantity);
            if (remainingItems > totalAvailableItems && !removePartial) return quantity;

            foreach (var slot in slotsWithItem) {

                if (remainingItems <= 0) break;

                if (slot.Quantity < remainingItems) {

                    remainingItems -= slot.Quantity;
                    slot.ClearSlot();
                    slots.Remove(slot);
                }
                else {

                    slot.Quantity -= remainingItems;
                    remainingItems = 0;
                }
            }

            return remainingItems;
        }

        public int RemoveItemFromSlot(InventorySlot slot, int quantity) {

            if (slot.Quantity >= quantity) {

                slot.Quantity -= quantity;
                DropItem(slot.itemData, quantity);

                if (slot.Quantity == 0) {

                    slot.ClearSlot();
                    slots.Remove(slot);
                }


                return 0;
            }
            else {

                int remainingQuantity = quantity - slot.Quantity;
                DropItem(slot.itemData, slot.Quantity);
                slot.ClearSlot();
                slots.Remove(slot);

                return remainingQuantity;
            }
        }

        public void DropItem(ItemData itemData, int numberDropped) {

            if (numberDropped > 0) {

                if (itemData.groupedPrefab) {

                    Instantiate(itemData.prefab, playerInventorySystem.GetDropPosition(), Quaternion.identity).GetComponent<ItemPickUp>().quantity = numberDropped;
                }
                else {

                    Vector3 location = playerInventorySystem.GetDropPosition();
                    for (int i = 0; i < numberDropped; ++i) {

                        Instantiate(itemData.prefab, playerInventorySystem.GetDropPosition(), Quaternion.identity);
                    }
                }
            }
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