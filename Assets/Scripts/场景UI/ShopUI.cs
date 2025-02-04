using UnityEngine;

/// <summary>
/// 商店界面控制器
/// 负责动态生成商品列表并管理UI交互
/// </summary>
public class ShopUI : MonoBehaviour
{
    [Header("UI元素绑定")]
    [Tooltip("商品列表的父容器，用于排列生成的商品项")]
    [SerializeField] 
    private Transform shopContent; // 在Unity编辑器中拖入ScrollView的Content对象

    [Tooltip("商品UI元素的预制件模板")]
    [SerializeField] 
    private ShopItemUI itemUIPrefab; // 拖入预先制作好的商品UI预制件

    [Header("商品数据配置")]
    [Tooltip("需要在商店中显示的所有商品配置")]
    [SerializeField] 
    private ShopItem[] shopItems; // 在Inspector面板中配置需要显示的商品

    /// <summary>
    /// 初始化商店界面
    /// </summary>
    void Start()
    {
        // 确保数据有效性
        if (shopContent == null)
        {
            Debug.LogError("未指定商品列表容器！");
            return;
        }

        if (itemUIPrefab == null)
        {
            Debug.LogError("未指定商品UI预制件！");
            return;
        }

        // 清空现有商品项（防止重复生成）
        ClearExistingItems();

        // 动态生成所有商品UI元素
        GenerateShopItems();
    }

    /// <summary>
    /// 生成所有商品项的UI
    /// </summary>
    private void GenerateShopItems()
    {
        foreach (var item in shopItems)
        {
            // 实例化预制件并设置父对象
            var uiElement = Instantiate(itemUIPrefab, shopContent);
            
            // 初始化商品数据
            if (uiElement != null)
            {
                uiElement.Initialize(item);
            }
            else
            {
                Debug.LogError("商品UI实例化失败");
            }
        }
    }

    /// <summary>
    /// 清空现有商品项（用于刷新界面）
    /// </summary>
    private void ClearExistingItems()
    {
        foreach (Transform child in shopContent)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// 动态更新商店商品（外部调用接口）
    /// </summary>
    /// <param name="newItems">新的商品配置数组</param>
    public void RefreshShopItems(ShopItem[] newItems)
    {
        shopItems = newItems;
        ClearExistingItems();
        GenerateShopItems();
    }
}