using UnityEngine;

namespace Core.Factory
{
    public class GenericFactory<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private T _prefab;
        [SerializeField] private Transform _spawnPoint;
        private int _offset = 0;

        public T CreateNewInstance()
        {
            Vector3 position = new Vector3(_spawnPoint.transform.position.x -_offset, _spawnPoint.transform.position.y,
                _spawnPoint.transform.position.z);
            _offset++;
            
            return Instantiate(_prefab, position, Quaternion.identity);
        }
    }
}
