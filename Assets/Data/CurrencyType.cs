// CurrencyType.cs
using UnityEngine;

/// <summary>
/// 货币类型配置（ScriptableObject）
/// 定义游戏中所有货币类型的基础属性
/// </summary>
[CreateAssetMenu(menuName = "Inventory/Currency Type")]
public class CurrencyType : ScriptableObject
{
    [Header("货币属性")]
    public string currencyID;     // 货币唯一标识
    public string displayName;    // 显示名称
    public Sprite icon;           // 货币图标
    [Tooltip("玩家初始拥有的数量")]
    public int defaultAmount = 0; // 默认持有量
}