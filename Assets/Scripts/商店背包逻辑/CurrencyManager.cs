// CurrencyManager.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 货币管理系统（单例）
/// 负责所有货币的存储、增减和状态通知
/// </summary>
public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    /// <summary>
    /// 货币变更事件（货币类型，新数量）
    /// </summary>
    [System.Serializable]
    public class CurrencyEvent : UnityEvent<CurrencyType, int> { }
    public CurrencyEvent OnCurrencyChanged = new CurrencyEvent();

    // 当前持有的所有货币数据
    private Dictionary<CurrencyType, int> currencies = new Dictionary<CurrencyType, int>();

    void Awake()
    {
        // 单例初始化
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// 初始化货币系统
    /// </summary>
    /// <param name="currencyTypes">需要管理的所有货币类型</param>
    public void InitializeCurrencies(CurrencyType[] currencyTypes)
    {
        foreach (var currency in currencyTypes)
        {
            SetCurrency(currency, currency.defaultAmount);
        }
    }

    /// <summary>
    /// 获取指定货币的数量
    /// </summary>
    public int GetCurrency(CurrencyType type)
    {
        return currencies.ContainsKey(type) ? currencies[type] : 0;
    }

    /// <summary>
    /// 直接设置货币数量（慎用）
    /// </summary>
    public void SetCurrency(CurrencyType type, int amount)
    {
        currencies[type] = amount;
        OnCurrencyChanged.Invoke(type, amount);
    }

    /// <summary>
    /// 增加货币数量（支持负数扣除）
    /// </summary>
    public void AddCurrency(CurrencyType type, int amount)
    {
        SetCurrency(type, GetCurrency(type) + amount);
    }

    /// <summary>
    /// 检查是否拥有足够数量的货币
    /// </summary>
    public bool HasEnoughCurrency(CurrencyType type, int amount)
    {
        return GetCurrency(type) >= amount;
    }

    /// <summary>
    /// 尝试扣除多种货币（原子操作）
    /// 全部扣除成功返回true，否则不扣除任何货币
    /// </summary>
    /// <param name="costs">需要扣除的货币组合</param>
    public bool TryDeductCurrencies(CurrencyCost[] costs)
    {
        // 第一阶段：验证所有货币是否足够
        foreach (var cost in costs)
        {
            if (!HasEnoughCurrency(cost.currencyType, cost.amount))
                return false;
        }

        // 第二阶段：实际扣除
        foreach (var cost in costs)
        {
            AddCurrency(cost.currencyType, -cost.amount);
        }

        return true;
    }
}