using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace Holistic3D.Inventory {

    public class ItemToggleMonitor : MonoBehaviour {

        public ToggleGroup toggleGroup;

        private Toggle lastSelectedToggle;

        void Start() {

            foreach (var toggle in toggleGroup.GetComponentsInChildren<Toggle>()) {

                toggle.onValueChanged.AddListener(isOn => {

                    if (isOn) {

                        HandleToggleChange(toggle);
                    }
                });
                if (toggle.isOn) InventoryPanelManager.Instance.FilterItemsByType(toggle.GetComponent<ToggleItemType>().toggleItemType);
            }
        }

        private void HandleToggleChange(Toggle selectToggle) {

            if (selectToggle != lastSelectedToggle) {

                lastSelectedToggle = selectToggle;
                ItemType selectedType = selectToggle.GetComponent<ToggleItemType>().toggleItemType;
                InventoryPanelManager.Instance.FilterItemsByType(selectedType);
            }
        }
    }
}