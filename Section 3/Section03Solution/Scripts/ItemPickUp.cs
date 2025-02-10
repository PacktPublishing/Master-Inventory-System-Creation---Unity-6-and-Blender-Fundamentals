using UnityEngine;


namespace Holistic3D.Inventory {
    public class ItemPickUp : MonoBehaviour {

        public ItemData itemData;
        public int quantity = 1;

        private void Start() {

            gameObject.GetComponent<Collider>().isTrigger = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }

        private void OnTriggerEnter(Collider other) {

            if (other.CompareTag("Player")) {

                PlayerInventorySystem playerInventory = other.GetComponent<PlayerInventorySystem>();

                if (playerInventory != null) {

                    quantity = playerInventory.PickUpItem(itemData, quantity);
                    if (quantity <= 0) Destroy(gameObject);
                }
            }
        }

        private void OnCollisionEnter(Collision collision) {

            ItemPickUp other = collision.gameObject.GetComponent<ItemPickUp>();

            if (other != null && itemData.itemID == collision.gameObject.GetComponent<ItemPickUp>().itemData.itemID) return;

            gameObject.GetComponent<Collider>().isTrigger = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}