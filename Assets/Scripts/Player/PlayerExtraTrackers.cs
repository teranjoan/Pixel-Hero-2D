using System.Collections.Generic;
using UnityEngine;

public class PlayerExtraTrackers : MonoBehaviour
{
    [SerializeField] private IDictionary<HabilityEnum, bool> enabledHabilities;

    void Awake()
    {
        enabledHabilities = new Dictionary<HabilityEnum, bool>();
    }

    public bool HasHability(HabilityEnum hability)
    {
        return enabledHabilities.ContainsKey(hability) && enabledHabilities[hability];
    }

    public void AddHability(HabilityEnum hability)
    {
        enabledHabilities.TryAdd(hability, true);
    }

    public void AddHabilities(HabilityEnum[] habilities)
    {
        foreach (var hab in habilities)
        {
            AddHability(hab);
        }
    }

    public void NotifyTracker(ItemsManager ItemsManager)
    {
        if (ItemsManager.HasNItems(ItemClassEnum.ShineHeart, 5))
        {
            AddHability(HabilityEnum.DoubleJump);
        }
        if (ItemsManager.HasNItems(ItemClassEnum.SpinCoin, 6))
        {
            AddHability(HabilityEnum.Dash);
        }
        if (ItemsManager.HasNItems(ItemClassEnum.ShineCoin, 10))
        {
            AddHabilities(new HabilityEnum[] { HabilityEnum.BallMode, HabilityEnum.DropBomb });
        }
    }
}
