using System;
using System.Collections;
using System.Collections.Generic;
using Entity.NPC;
using UnityEngine;

public abstract class AbstractStand : ItemDistributor
{
    protected readonly Dictionary<string, Coroutine> _receiveItemDictionary = new Dictionary<string, Coroutine>();
    protected readonly Dictionary<string, Coroutine> _giveItemDictionary = new Dictionary<string, Coroutine>(); 

    protected abstract IEnumerator GiveDelay(InventoryManager inventoryManager, NPC npc, string entityName);
    protected abstract IEnumerator ReceiveDelay(InventoryManager inventoryManager, NPC npc, string entityName);
}
