// PurchaseEvents.cs
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PurchaseFailedEvent : UnityEvent<CurrencyCost[]> { }