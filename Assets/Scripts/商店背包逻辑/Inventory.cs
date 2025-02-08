using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 背包系统核心逻辑（单例模式）
/// 支持物品堆叠、数量限制和UI同步
/// </summary>
public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    [System.Serializable]
    public class InventoryUpdateEvent : UnityEvent<ItemData, int> { }
    public InventoryUpdateEvent OnInventoryUpdated = new InventoryUpdateEvent();

    // 使用字典存储物品实例和数量
    private Dictionary<ItemData, int> items = new Dictionary<ItemData, int>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 添加物品（自动堆叠）
    /// </summary>
    public void AddItem(ItemData item, int amount = 1)
    {
        Debug.Log($"尝试添加物品: {item.itemName} x{amount}");
        if (items.ContainsKey(item))
        {
            int currentAmount = items[item];
            int remainingSpace = item.maxStack - currentAmount;
            
            if (remainingSpace > 0)
            {
                int addAmount = Mathf.Min(remainingSpace, amount);
                items[item] += addAmount;
                amount -= addAmount;
                OnInventoryUpdated.Invoke(item, items[item]);
            }

            // 如果还有剩余数量需要添加
            if (amount > 0)
            {
                items.Add(item, amount);
                OnInventoryUpdated.Invoke(item, amount);
            }
        }
        else
        {
            items.Add(item, Mathf.Min(amount, item.maxStack));
            OnInventoryUpdated.Invoke(item, items[item]);
        }
    }

    /// <summary>
    /// 移除物品
    /// </summary>
    public bool RemoveItem(ItemData item, int amount = 1)
    {
        if (!items.ContainsKey(item) || items[item] < amount)
            return false;

        items[item] -= amount;
        if (items[item] <= 0)
        {
            items.Remove(item);
            OnInventoryUpdated.Invoke(item, 0);
        }
        else
        {
            OnInventoryUpdated.Invoke(item, items[item]);
        }
        return true;
    }

    /// <summary>
    /// 获取指定物品的数量
    /// </summary>
    public int GetItemCount(ItemData item)
    {
        return items.ContainsKey(item) ? items[item] : 0;
    }

    /// <summary>
    /// 获取所有物品的快照
    /// </summary>
    public Dictionary<ItemData, int> GetAllItems()
    {
        return new Dictionary<ItemData, int>(items);
    }
}


/*// Inventory.cs
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
}*/