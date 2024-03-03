using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    private IDictionary<ItemClassEnum, int> leftItems;
    private IDictionary<ItemClassEnum, int> collectedItems;

    void Awake()
    {
        // TODO: IMPROVE for initialization
        collectedItems = new Dictionary<ItemClassEnum, int> { { ItemClassEnum.ShineCoin, 0 }, { ItemClassEnum.ShineHeart, 0 }, { ItemClassEnum.SpinCoin, 0 }, };
        leftItems = new Dictionary<ItemClassEnum, int> { { ItemClassEnum.ShineCoin, 0 }, { ItemClassEnum.ShineHeart, 0 }, { ItemClassEnum.SpinCoin, 0 }, };
    }

    void Update()
    {
        leftItems = new Dictionary<ItemClassEnum, int> { { ItemClassEnum.ShineCoin, 0 }, { ItemClassEnum.ShineHeart, 0 }, { ItemClassEnum.SpinCoin, 0 }, };
        // TODO: IMPROVE for Performance
        ItemController[] itemsEnNivel = FindObjectsOfType<ItemController>();
        for (int i = 0; i < itemsEnNivel.Length; i++)
        {
            leftItems[itemsEnNivel[i].ItemClass] = leftItems[itemsEnNivel[i].ItemClass] + 1;
        }
    }

    public void AddCollectedItem(ItemClassEnum itemClass)
    {
        collectedItems[itemClass] = collectedItems[itemClass] + 1;
    }

    public bool HasNItems(ItemClassEnum itemClass, int quantity)
    {
        return collectedItems[itemClass] >= quantity;
    }

    public string GetLeftItemsOnLevelString()
    {
        int SpinCoinLeft = leftItems[ItemClassEnum.SpinCoin];
        int ShineCoinLeft = leftItems[ItemClassEnum.ShineCoin];
        int ShineHeartLeft = leftItems[ItemClassEnum.ShineHeart];
        return string.Format("Shining Coin: {0} \nSpinning Coin: {1} \n Shining Heart: {2}",
                                SpinCoinLeft,
                                ShineCoinLeft,
                                ShineHeartLeft);
    }
}
