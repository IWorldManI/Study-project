using System;
using System.Collections;
using System.Collections.Generic;
using Entity.NPC;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public Action<NPC> OnCollect;
    public Action<NPC> OnCompleteWay;
    
    public Action<ItemDistributor> OnCollectStand;
    
}
