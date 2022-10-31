using System;

public class CustomerOrdersManager : ItemDistributor
{
    public Type GetOrder(ItemDistributor distributor)
    {
        var item = distributor.StandType;
        return item;
    }
}
