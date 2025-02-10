using UnityEngine;

namespace Holistic3D.Inventory {

    [System.Serializable]
    public class InventorySlot {

        public ItemData itemData;
        public int quantity;

        public InventorySlot(ItemData id, int q) {

            itemData = id;
            quantity = q;
        }

        public bool IsEmpty() {

            return itemData == null || quantity <= 0;
        }

        public void ClearSlot() {

            itemData = null;
            quantity = 0;
        }

        public void SetItem(ItemData newItemData, int newQuantity) {

            itemData = newItemData;
            quantity = newQuantity;
        }
    }
}