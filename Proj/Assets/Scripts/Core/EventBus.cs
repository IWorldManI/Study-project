using System;
using Entity.NPC;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public Action<NPC> OnCollect;
    public Action<NPC> OnCompleteWay;
    
    public Action<ItemDistributor> OnCollectStand;
    
}
