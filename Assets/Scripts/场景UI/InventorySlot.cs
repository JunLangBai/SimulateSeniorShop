using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 单个物品槽UI控件
/// </summary>
public class InventorySlot : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private Text amountText;
    [SerializeField] private Image background;

    public ItemData CurrentItem { get; private set; }
    public int CurrentAmount { get; private set; }

    public event Action<ItemData> OnClick;

    public void Initialize(ItemData item, int amount)
    {
        CurrentItem = item;
        CurrentAmount = amount;
        
        itemIcon.sprite = item.icon;
        UpdateAmountDisplay();
        
        GetComponent<Button>().onClick.AddListener(() => OnClick?.Invoke(item));
    }

    public void UpdateSlot(int newAmount)
    {
        CurrentAmount = newAmount;
        UpdateAmountDisplay();
    }

    private void UpdateAmountDisplay()
    {
        amountText.text = CurrentAmount > 1 ? CurrentAmount.ToString() : "";
        amountText.gameObject.SetActive(CurrentAmount > 1);
    }

    public void SetHighlight(bool isSelected)
    {
        background.color = isSelected ? 
            new Color(1, 1, 1, 0.1f) :
            new Color(1, 1, 1, 0.1f);
    }
}