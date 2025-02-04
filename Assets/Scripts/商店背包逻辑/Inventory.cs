// Inventory.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 背包管理系统（单例）
/// 负责物品的存储、堆叠和状态通知
/// </summary>
public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    /// <summary>
    /// 背包变更事件
    /// </summary>
    [System.Serializable]
    public class InventoryEvent : UnityEvent { }
    public InventoryEvent OnInventoryChanged = new InventoryEvent();

    // 当前持有的物品数据（物品-数量）
    private Dictionary<ItemData, int> items = new Dictionary<ItemData, int>();

    void Awake()
    {
        // 单例初始化
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// 添加物品到背包（自动堆叠）
    /// </summary>
    /// <param name="item">物品数据</param>
    /// <param name="amount">添加数量</param>
    public void AddItem(ItemData item, int amount = 1)
    {
        if (items.ContainsKey(item))
        {
            items[item] = Mathf.Min(items[item] + amount, item.maxStack);
        }
        else
        {
            items.Add(item, amount);
        }
        OnInventoryChanged.Invoke();
    }

    /// <summary>
    /// 从背包移除物品
    /// </summary>
    /// <returns>是否成功移除</returns>
    public bool RemoveItem(ItemData item, int amount = 1)
    {
        if (!items.ContainsKey(item) || items[item] < amount)
            return false;

        items[item] -= amount;
        if (items[item] <= 0)
            items.Remove(item);

        OnInventoryChanged.Invoke();
        return true;
    }

    /// <summary>
    /// 获取指定物品的当前数量
    /// </summary>
    public int GetItemCount(ItemData item)
    {
        return items.ContainsKey(item) ? items[item] : 0;
    }
}