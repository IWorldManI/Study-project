using DG.Tweening;
using UnityEngine;


public class DoorOpening : MonoBehaviour
{
    [SerializeField] private Transform door;
    [SerializeField] private float targetAngle; //200 refrigerator
    
    private int _entityCounter;
    private bool _isOpened = false;

    private void Start()
    {
        door = GetComponentInChildren<DoorComponent>().GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EntityBase>(out var entity))
        {
            _entityCounter++;
            TryOpenTheDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<EntityBase>(out var entity))
        {
            _entityCounter--;
            TryCloseTheDoor();
        }
    }

    private void TryOpenTheDoor()
    {
        if (!_isOpened)
        {
            OpenDoor();
        }
    }

    private void TryCloseTheDoor()
    {
        if (_entityCounter <= 0 && _isOpened)
        {
            CloseDoor();
        }
    }

    private void OpenDoor()
    {
        door.DOLocalRotate(new Vector3(0,targetAngle,0), 0.4f, RotateMode.FastBeyond360);
        _isOpened = true;
    }

    private void CloseDoor()
    {
        door.DOLocalRotate(Vector3.zero, 0.4f, RotateMode.FastBeyond360);
        _isOpened = false;
    }
}
