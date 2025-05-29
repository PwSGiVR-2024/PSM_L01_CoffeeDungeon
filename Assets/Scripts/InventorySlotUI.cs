using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class InventorySlotUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text amountText;

    public void Setup(InventorySlot slot)
    {
        icon.sprite = slot.item.iconPrefab;
        amountText.text = slot.amount.ToString();
    }
}
