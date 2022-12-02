using System;

public class CustomerOrdersManager
{
    public Type GetOrder(ItemDistributor distributor)
    {
        var item = distributor.StandType;
        return item;
    }
}
