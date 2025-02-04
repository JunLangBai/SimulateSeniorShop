using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image itemIcon;         // 商品图标显示
    [SerializeField] private Text itemNameText;      // 商品名称显示
    [SerializeField] private Transform priceParent;  // 价格标签的父节点
    [SerializeField] private GameObject pricePrefab; // 单个价格标签预制件
    [SerializeField] private Button purchaseButton; // 手动拖拽绑定


    [Header("状态指示")] 
    [SerializeField] private Image stockIndicator;   // 库存状态图标
    [SerializeField] private Color inStockColor = Color.green;
    [SerializeField] private Color outOfStockColor = Color.red;

    private ShopItem currentItem; // 当前显示的商品

    private List<Text> priceTexts = new List<Text>();
    private List<CurrencyCost> trackedCosts = new List<CurrencyCost>();
    
    void Awake()
    {
        // 自动获取本物体或子物体的按钮
        if (purchaseButton == null)
            purchaseButton = GetComponentInChildren<Button>();
    }
    
    /// <summary>
    /// 初始化商品UI显示
    /// </summary>
    public void Initialize(ShopItem shopItem)
    {
        currentItem = shopItem;
        
        // 清空旧价格标签
        ClearPriceTags();

        // 设置基础信息
        itemIcon.sprite = shopItem.item.icon;
        itemNameText.text = shopItem.item.itemName;

        // 生成价格标签
        CreatePriceTags(shopItem.costs);

        // 更新库存状态
        UpdateStockDisplay();
    }

    /// <summary>
    /// 购买按钮点击事件
    /// </summary>
    public void OnPurchaseClicked()
    {
        if (currentItem != null)
        {
            ShopSystem.Instance.TryPurchaseItem(currentItem);
            UpdateStockDisplay(); // 立即更新UI状态
        }
    }

    private void ClearPriceTags()
    {
        foreach (Transform child in priceParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void CreatePriceTags(CurrencyCost[] costs)
    {
        foreach (var cost in costs)
        {
            var priceTag = Instantiate(pricePrefab, priceParent);
            var priceText = priceTag.GetComponentInChildren<Text>();
            var currencyIcon = priceTag.GetComponentInChildren<Image>();

            // 设置文字和图标
            priceText.text = cost.amount.ToString();
            currencyIcon.sprite = cost.currencyType.icon;

            // 实时检测货币是否足够
            bool canAfford = CurrencyManager.Instance.HasEnoughCurrency(
                cost.currencyType, 
                cost.amount
            );
            priceText.color = canAfford ? Color.white : Color.red;
            
            priceTexts.Add(priceText);
            trackedCosts.Add(cost);
        }
        
        // 初始颜色设置
        UpdateAllPriceColors();
    }

    private void UpdateStockDisplay()
    {
        if (stockIndicator == null) return;

        stockIndicator.gameObject.SetActive(currentItem.stock != 0);
        stockIndicator.color = currentItem.stock > 0 ? 
            inStockColor : 
            outOfStockColor;
    }
    
    private void OnEnable()
    {
        CurrencyManager.Instance.OnCurrencyChanged.AddListener(OnCurrencyChanged);
    }

    private void OnDisable()
    {
        CurrencyManager.Instance.OnCurrencyChanged.RemoveListener(OnCurrencyChanged);
    }

    private void OnCurrencyChanged(CurrencyType changedType, int newAmount)
    {
        UpdateAllPriceColors();
    }

    private void UpdateAllPriceColors()
    {
        for (int i = 0; i < trackedCosts.Count; i++)
        {
            bool canAfford = CurrencyManager.Instance.HasEnoughCurrency(
                trackedCosts[i].currencyType, 
                trackedCosts[i].amount
            );
            priceTexts[i].color = canAfford ? Color.white : Color.red;
        }
    }
}