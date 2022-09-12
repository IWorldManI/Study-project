using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float moveTime = 10f;
    
    //[EditorButton]
    private void StartFollow()
    {
        StartCoroutine(Move());
    }
 
    private IEnumerator Move()
    {
        var time = 0f;
        while (time <= moveTime)
        {
            var step = (player.position - transform.position) * Mathf.InverseLerp(0, moveTime, time);
            transform.position += step;
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
            if (Vector3.Distance(transform.position, player.position) <= Vector3.kEpsilon)
            {
                yield break;
            }
        }
    }
}
