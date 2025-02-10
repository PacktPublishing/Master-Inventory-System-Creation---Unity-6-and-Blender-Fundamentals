using UnityEngine;

namespace Holistic3D.Inventory {

    public class InventoryManager : MonoBehaviour {

        public InventorySystem inventorySystem;
        public ItemData item;
        public ItemData item1;

        private void Update() {

            if (Input.GetKeyDown(KeyCode.A)) inventorySystem.AddItem(item, 1);
            if (Input.GetKeyDown(KeyCode.S)) inventorySystem.AddItem(item1, 1);
            if (Input.GetKeyDown(KeyCode.R)) inventorySystem.RemoveItem(item, 1);
        }
    }
}