using UnityEngine;

/// <summary>
/// 商店系统核心逻辑（单例）
/// 处理购买验证和交易执行
/// </summary>
public class ShopSystem : MonoBehaviour
{
    // 单例实例
    public static ShopSystem Instance { get; private set; }
    
    //商店系统触发失败事件
    public PurchaseFailedEvent OnPurchaseFailed = new PurchaseFailedEvent();

    void Awake()
    {
        // 单例初始化逻辑
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 跨场景保留
        }
        else
        {
            Destroy(gameObject); // 防止重复创建
        }
    }

    /// <summary>
    /// 尝试购买商品
    /// </summary>
    /// <param name="shopItem">要购买的商品配置</param>
    public void TryPurchaseItem(ShopItem shopItem)
    {
        // 库存检查
        if (shopItem.stock == 0) return;

        // 货币扣除
        if (!CurrencyManager.Instance.TryDeductCurrencies(shopItem.costs))
            OnPurchaseFailed.Invoke(shopItem.costs);
        return;

        // 物品发放
        Inventory.Instance.AddItem(shopItem.item);

        // 库存更新（-1表示无限库存不减少）
        if (shopItem.stock > 0)
            shopItem.stock--;
    }
}