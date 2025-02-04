// PurchaseFailedPanel.cs

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseFailedPanel : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private CanvasGroup panel;
    [SerializeField] private Text messageText;

    void OnEnable()
    {
        StartCoroutine(DelayedRegistration());
    }

    IEnumerator DelayedRegistration()
    {
        // 等待单例准备就绪
        yield return new WaitUntil(() => ShopSystem.Instance != null);
        
        // 安全注册监听器
        ShopSystem.Instance.OnPurchaseFailed.AddListener(ShowPopup);
    }

    void OnDisable()
    {
        if (ShopSystem.Instance != null)
        {
            ShopSystem.Instance.OnPurchaseFailed.RemoveListener(ShowPopup);
        }
    }


    public void ShowPopup(CurrencyCost[] missingCurrencies)
    {
        // 构建提示信息
        string baseMessage = "货币不足";
        messageText.text = baseMessage;
        
        panel.alpha = 1;
        panel.blocksRaycasts = true;
    }

    public void ClosePopup()
    {
        panel.alpha = 0;
        panel.blocksRaycasts = false;
    }
    

    // 可选：跳转到充值界面
    public void NavigateToCurrencyShop()
    {
        Debug.Log("打开充值界面");
        // 实现具体跳转逻辑
    }
}