using UnityEngine;


namespace Holistic3D.Inventory {

    [CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/ItemData")]
    public class ItemData : ScriptableObject {

        public string itemName;
        public int itemID;
        public Sprite icon;
        public ItemType itemType;
        public bool isStackable;
        public int maxStackSize;
        public bool groupedPrefab;
        public GameObject prefab;
        public string description;
    }

}