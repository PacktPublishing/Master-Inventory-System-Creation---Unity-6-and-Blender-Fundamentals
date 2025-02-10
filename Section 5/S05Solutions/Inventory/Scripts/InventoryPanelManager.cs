using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


namespace Holistic3D.Inventory {

    public class InventoryPanelManager : MonoBehaviour {

        [SerializeField] private GameObject itemButtonPrefab;
        [SerializeField] private GameObject itemContainer;
        [SerializeField] private Button dropButton;
        [SerializeField] private Button dropAllButton;
        [SerializeField] private CanvasGroup itemDetailsPanel;
        [SerializeField] private TMPro.TMP_InputField numberToDrop;
        [SerializeField] private TMPro.TextMeshProUGUI itemNameText;
        [SerializeField] private TMPro.TextMeshProUGUI itemCountText;
        [SerializeField] private TMPro.TextMeshProUGUI itemDescriptionText;
        [SerializeField] private InventorySystem inventorySystem;
        [SerializeField] private List<ItemData> allItems;

        private Dictionary<GameObject, ItemType> inventoryItemMap = new Dictionary<GameObject, ItemType>();
        private Dictionary<int, GameObject> previewItemObjects;
        public static InventoryPanelManager Instance { get; private set; }

        private ItemType activeItemTypeTab;
        private GameObject itemToShow;

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

            itemDetailsPanel.alpha = 0;
        }

        public void SetPanelVisibilty(bool isVisible) {

            CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();
            canvasGroup.alpha = isVisible ? 1 : 0;
            canvasGroup.interactable = isVisible;
            canvasGroup.blocksRaycasts = isVisible;
        }

        public void TogglePanelVisibilty() {

            CanvasGroup canvasGroup = this.GetComponent<CanvasGroup>();

            if (canvasGroup.alpha == 1) {

                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
            else {

                canvasGroup.alpha = 1;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }

        public ItemButtonSettings CreateInventoryNuttom(ItemData itemData, InventorySlot slot) {

            GameObject itemButton = Instantiate(itemButtonPrefab, itemContainer.transform);
            ItemButtonSettings itemButtonSettings = itemButton.GetComponent<ItemButtonSettings>();
            itemButtonSettings.Init(itemData, 1);
            inventoryItemMap.Add(itemButton, itemData.itemType);
            itemButton.GetComponent<Button>().onClick.AddListener(() => ShowItem(itemData.itemID, slot));

            if (itemData.itemType != activeItemTypeTab) itemButton.gameObject.SetActive(false);

            return itemButtonSettings;
        }

        public void DestroyInventoryButton(GameObject invButton) {

            inventoryItemMap.Remove(invButton);
            itemDetailsPanel.alpha = 0;
            itemToShow.SetActive(false);
            Destroy(invButton);
        }

        public void FilterItemsByType(ItemType type) {

            activeItemTypeTab = type;

            foreach (var kvp in inventoryItemMap) {

                GameObject itemButton = kvp.Key;
                ItemType itemType = kvp.Value;

                itemButton.gameObject.SetActive(itemType == type);
            }
        }

        public void ShowItem(int itemID, InventorySlot slot) {

            dropButton.onClick.RemoveAllListeners();
            dropButton.onClick.AddListener(() => inventorySystem.RemoveItemFromSlot(slot, int.Parse(numberToDrop.text)));
            dropAllButton.onClick.RemoveAllListeners();
            dropAllButton.onClick.AddListener(() => inventorySystem.RemoveItemFromSlot(slot, slot.Quantity));
            itemDetailsPanel.alpha = 0;

            foreach (var obj in previewItemObjects.Values) obj.SetActive(false);

            if (previewItemObjects.TryGetValue(itemID, out itemToShow)) {

                itemToShow.SetActive(true);
                itemNameText.text = slot.itemData.itemName;
                itemCountText.text = slot.Quantity + "";
                itemDescriptionText.text = slot.itemData.description;
                itemDetailsPanel.alpha = 1;
            }
        }
    }
}