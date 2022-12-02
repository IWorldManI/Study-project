
using UnityEngine;

public interface IObserver
{
    Transform _transform { get;}
    void UpdateObserver(ISubject subject);
}
