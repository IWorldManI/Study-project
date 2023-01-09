using System;
using UnityEngine;

public class CustomerOrdersManager : IEnumTypeToVisual
{
    public Type GetOrder(ItemDistributor distributor)
    {
        var item = distributor.StandType;
        return item;
    }

    public Sprite GetImage(ItemDistributor distributor)
    {
        var item = distributor.StandType;
        return IEnumTypeToVisual.DictionaryOfImages[item];
    }
}
