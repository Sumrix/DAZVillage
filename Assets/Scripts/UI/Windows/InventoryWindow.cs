using UnityEngine;

namespace GameUI
{
    public class InventoryWindow :
        MonoBehaviour
    {
        private void OnEnable()
        {
            Managers.UI.Inventory.gameObject.SetActive(true);
        }
        private void OnDisable()
        {
            Managers.UI.Inventory.gameObject.SetActive(false);
        }
    }
}