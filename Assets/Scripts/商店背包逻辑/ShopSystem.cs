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
        if (shopItem == null) return;

        Debug.Log($"尝试购买 {shopItem.item.itemName}");

        // 检查库存
        if (shopItem.stock == 0)
        {
            Debug.Log("商品已售罄");
            OnPurchaseFailed.Invoke(shopItem.costs);
            return;
        }

        // 检查并扣除货币（原子操作）
        if (!CurrencyManager.Instance.TryDeductCurrencies(shopItem.costs))
        {
            Debug.Log("货币不足");
            OnPurchaseFailed.Invoke(shopItem.costs);
            return;
        }

        // 添加物品到背包
        Inventory.Instance.AddItem(shopItem.item);

        // 更新库存
        if (shopItem.stock > 0)
            shopItem.stock--;

        Debug.Log("购买成功");
    }
}