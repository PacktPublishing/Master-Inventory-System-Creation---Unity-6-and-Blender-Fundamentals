using UnityEngine;
using UnityEngine.InputSystem;

namespace Holistic3D.Inventory {

    public class PlayerInventorySystem : MonoBehaviour {

        public InventorySystem inventorySystem;
        public InputAction dropAction;
        public ItemData itemToDrop;

        private void Start() {

            dropAction = InputSystem.actions.FindAction("Drop");
        }

        private void Update() {

            if (dropAction.WasPressedThisFrame()) {

                DropItem(itemToDrop, 6);
            }
        }

        public int PickUpItem(ItemData itemData, int quantity) {

            if (!inventorySystem.IsFull() || itemData.isStackable) {

                return inventorySystem.AddItem(itemData, quantity);
            }

            return quantity;
        }

        public void DropItemsSlot(int slotNumber) {

            inventorySystem.RemoveItemsFromSlots(slotNumber);
        }

        public void DropItem(ItemData itemData, int quantity) {

            int couldntBeDropped = inventorySystem.RemoveItem(itemData, quantity);
            int numberDropped = quantity - couldntBeDropped;

            if (numberDropped > 0) {

                if (itemData.groupedPrefab) {

                    Instantiate(itemData.prefab, GetDropPosition(), Quaternion.identity).GetComponent<ItemPickUp>().quantity = numberDropped;
                }
                else {

                    Vector3 location = GetDropPosition();
                    for (int i = 0; i < numberDropped; ++i) {

                        Instantiate(itemData.prefab, GetDropPosition(), Quaternion.identity);
                    }
                }
            }
        }

        private Vector3 GetDropPosition() {

            Vector3 playerPosition = transform.position;
            Vector3 forwardDirection = transform.forward;
            Vector3 upDirection = transform.up;
            float dropDistance = 2.0f;

            return playerPosition + forwardDirection * dropDistance + upDirection * dropDistance;
        }
    }
}