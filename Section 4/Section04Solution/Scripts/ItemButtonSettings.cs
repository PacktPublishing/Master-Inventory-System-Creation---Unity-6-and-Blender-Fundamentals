using UnityEngine;
using UnityEngine.UI;


namespace Holistic3D.Inventory {

    public class ItemButtonSettings : MonoBehaviour {

        [SerializeField] private Image spriteImage;
        [SerializeField] private TMPro.TextMeshProUGUI numberInSlot;
        [SerializeField] private ItemType itemType;

        public void Init(ItemData data, int itemCount) {

            spriteImage.sprite = data.icon;
            numberInSlot.text = itemCount + "";
            itemType = data.itemType;
        }

    }
}