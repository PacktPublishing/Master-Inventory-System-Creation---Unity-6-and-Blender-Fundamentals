using UnityEngine;

namespace Holistic3D.Inventory {

    [System.Serializable]
    public class InventorySlot {

        public ItemData itemData;
        private int quantity;
        private ItemButtonSettings itemButton;

        public int Quantity {

            get => quantity;

            set {

                quantity = value < 0 ? 0 : value;

                if (itemButton != null) {

                    itemButton.UpdateQuantityDisplay(quantity);
                }
            }
        }

        public InventorySlot(ItemData id, int q) {

            itemData = id;
            itemButton = InventoryPanelManager.Instance.CreateInventoryNuttom(itemData, this);
            Quantity = q;
        }

        public bool IsEmpty() {

            return itemData == null || quantity <= 0;
        }

        public void ClearSlot() {

            itemData = null;
            Quantity = 0;

            if (itemButton != null)
                InventoryPanelManager.Instance.DestroyInventoryButton(itemButton.gameObject);
        }

        public void SetItem(ItemData newItemData, int newQuantity) {

            itemData = newItemData;
            Quantity = newQuantity;
        }
    }
}