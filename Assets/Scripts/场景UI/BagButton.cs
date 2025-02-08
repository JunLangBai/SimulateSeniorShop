using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagButton : MonoBehaviour
{
    public CanvasGroup bags;

    public void Bag()
    {
        if (bags.alpha == 0)
        {
            bags.alpha = 1;
            bags.blocksRaycasts = true;
        }
        else
        {
            bags.alpha = 0;
            bags.blocksRaycasts = false;
        }
    }
}
