using UnityEngine;
using UnityEngine.UI;

public class CurrencyDisplay : MonoBehaviour
{
    [SerializeField] CurrencyType currencyType;
    [SerializeField] Image iconImage;
    [SerializeField] Text amountText;

    void Start()
    {
        // 初始化显示
        iconImage.sprite = currencyType.icon;
        UpdateDisplay(currencyType, 
            CurrencyManager.Instance.GetCurrency(currencyType));

        // 注册变更事件
        CurrencyManager.Instance.OnCurrencyChanged.AddListener(UpdateDisplay);
    }

    void UpdateDisplay(CurrencyType type, int amount)
    {
        if (type == currencyType)
            amountText.text = amount.ToString();
    }
}