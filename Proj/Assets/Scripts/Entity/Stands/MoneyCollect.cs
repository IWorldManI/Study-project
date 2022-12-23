using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class MoneyCollect : ItemDistributor
{
    private List<PooledShape> _moneyList = new List<PooledShape>();
    private MoneyPool _moneyPool;
    
    private Sequence _moneyAnimation;

    private void Start()
    {
        _moneyPool = FindObjectOfType<MoneyPool>();
        foreach (Transform child in transform)
        {
            _moneyList.Add(child.GetComponent<PooledShape>());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CharacterMoveAndRotate>(out var player))
        {
            foreach (var money in _moneyList)
            {
                money.transform.parent = player.transform;
                _moneyAnimation = money.transform.DOLocalJump(Vector3.zero, 1f, 1, _itemDistributeDuration).OnComplete(() => MoveComplete(money));
            }
        }
    }

    private void MoveComplete(PooledShape item)
    {
        _moneyPool.KillShape(item);
    }
}
