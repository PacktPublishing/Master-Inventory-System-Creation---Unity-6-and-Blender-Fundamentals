using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


namespace Holistic3D.Inventory {

    public class InventoryPanelManager : MonoBehaviour {

        [SerializeField] private GameObject itemButtonPrefab;
        [SerializeField] private GameObject itemContainer;
        [SerializeField] private List<ItemData> allItems;

        private Dictionary<GameObject, ItemType> inventoryItemMap = new Dictionary<GameObject, ItemType>();
        private Dictionary<int, GameObject> previewItemObjects;
        public static InventoryPanelManager Instance { get; private set; }

        private void Awake() {

            if (Instance != null && Instance != this) {

                Destroy(gameObject);
            }
            else {

                Instance = this;
            }

            previewItemObjects = new Dictionary<int, GameObject>();

            foreach (var item in allItems) {

                GameObject obj = GameObject.Find("Preview_" + item.itemID);

                if (obj != null) {

                    previewItemObjects[item.itemID] = obj;
                    obj.SetActive(false);
                }
            }
        }


        void Start() {

            for (int i = 0; i < 20; ++i) {

                GameObject itemButton = Instantiate(itemButtonPrefab, itemContainer.transform);
                ItemData thisItemData = Random.Range(0, 2) == 1 ? allItems[0] : allItems[1];
                itemButton.GetComponent<ItemButtonSettings>().Init(thisItemData, Random.Range(1, 11));
                inventoryItemMap.Add(itemButton, thisItemData.itemType);

                itemButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(thisItemData.itemID));
            }

            for (int i = 0; i < 20; ++i) {

                GameObject itemButton = Instantiate(itemButtonPrefab, itemContainer.transform);
                ItemData thisItemData = allItems[2];
                itemButton.GetComponent<ItemButtonSettings>().Init(thisItemData, Random.Range(1, 11));
                inventoryItemMap.Add(itemButton, thisItemData.itemType);

                itemButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(thisItemData.itemID));
            }
        }

        public void FilterItemsByType(ItemType type) {

            foreach (var kvp in inventoryItemMap) {

                GameObject itemButton = kvp.Key;
                ItemType itemType = kvp.Value;

                itemButton.gameObject.SetActive(itemType == type);
            }
        }

        public void ShowItem(int itemID) {

            foreach (var obj in previewItemObjects.Values) obj.SetActive(false);

            if (previewItemObjects.TryGetValue(itemID, out GameObject itemToShow)) itemToShow.SetActive(true);
        }
    }
}