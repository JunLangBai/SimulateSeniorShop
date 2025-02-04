// ItemData.cs
using UnityEngine;

/// <summary>
/// 物品基础数据配置（ScriptableObject）
/// 用于创建所有可交易/可存储物品的配置数据
/// </summary>
[CreateAssetMenu(menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("基础属性")]
    public string itemID;         // 物品唯一标识符
    public string itemName;       // 物品显示名称
    public Sprite icon;           // 物品图标
    [Tooltip("最大堆叠数量，1表示不可堆叠")] 
    public int maxStack = 1;      // 堆叠上限
    [TextArea] public string description; // 物品详细描述
}