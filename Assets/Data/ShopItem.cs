// ShopItem.cs
using System;
using UnityEngine;

/// <summary>
/// 单个商品的价格组成
/// </summary>
[Serializable]
public class CurrencyCost
{
    public CurrencyType currencyType; // 需要的货币类型
    public int amount;                // 需要消耗的数量
}

/// <summary>
/// 商品配置（ScriptableObject）
/// 定义商店中单个商品的完整信息
/// </summary>
[CreateAssetMenu(menuName = "Shop/Shop Item")]
public class ShopItem : ScriptableObject
{
    [Header("商品属性")]
    public ItemData item;        // 对应的物品
    public CurrencyCost[] costs; // 购买需要的货币组合
    
    [Space]
    [Tooltip("-1表示无限库存，0表示售罄")]
    public int stock = -1;       // 商品库存量
}