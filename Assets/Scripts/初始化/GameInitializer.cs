using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] 
    private CurrencyType[] allCurrencyTypes; // 拖入所有CurrencyType资产

    void Start()
    {
        // 初始化货币系统（设置初始金额）
        CurrencyManager.Instance.InitializeCurrencies(allCurrencyTypes);
        
        // 其他系统初始化...
    }
}