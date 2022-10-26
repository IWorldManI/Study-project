using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;
using Random = UnityEngine.Random;

public class CustomerOrdersManager : ItemDistributor
{
    public Type GetOrder(ItemDistributor distributor)
    {
        var item = distributor.StandType;
        return item;
    }
}
