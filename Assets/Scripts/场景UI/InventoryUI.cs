using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 动态背包界面控制器
/// </summary>
public class InventoryUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Transform itemContainer; // 物品槽容器
    [SerializeField] private GameObject itemSlotPrefab; // 单个物品槽预制件

    [Header("视觉设置")]
    [SerializeField] private Color selectedColor = Color.yellow;
    
    private Dictionary<ItemData, InventorySlot> slots = new Dictionary<ItemData, InventorySlot>();

    void Start()
    {
        Inventory.Instance.OnInventoryUpdated.AddListener(UpdateInventoryDisplay);
        InitializeUI();
    }

    private void InitializeUI()
    {
        // 初始加载已有物品
        foreach (var item in Inventory.Instance.GetAllItems())
        {
            CreateOrUpdateSlot(item.Key, item.Value);
        }
    }

    private void UpdateInventoryDisplay(ItemData item, int newAmount)
    {
        if (newAmount == 0)
        {
            RemoveSlot(item);
        }
        else
        {
            CreateOrUpdateSlot(item, newAmount);
        }
    }

    private void CreateOrUpdateSlot(ItemData item, int amount)
    {
        if (slots.ContainsKey(item))
        {
            // 更新现有槽位
            slots[item].UpdateSlot(amount);
        }
        else
        {
            // 创建新槽位
            GameObject slotObj = Instantiate(itemSlotPrefab, itemContainer);
            InventorySlot slot = slotObj.GetComponent<InventorySlot>();
            slot.Initialize(item, amount);
            slots.Add(item, slot);
            
            // 添加点击事件
            slot.OnClick += HandleItemSelected;
        }
    }

    private void RemoveSlot(ItemData item)
    {
        if (slots.ContainsKey(item))
        {
            Destroy(slots[item].gameObject);
            slots.Remove(item);
        }
    }

    private void HandleItemSelected(ItemData item)
    {
        // 高亮选中效果
        foreach (var slot in slots.Values)
        {
            slot.SetHighlight(slot.CurrentItem == item);
        }
        Debug.Log($"选中物品: {item.itemName}");
    }
}