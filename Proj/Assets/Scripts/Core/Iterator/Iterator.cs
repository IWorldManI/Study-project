using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Iterator
{
    abstract class Iterator : IEnumerator
    {
        object IEnumerator.Current => Current();

        // Returns the key of the current element
        public abstract int Key();
        
        // Returns the current element
        public abstract object Current();
        
        // Move forward to next element
        public abstract bool MoveNext();
        
        // Rewinds the Iterator to the first element
        public abstract void Reset();
    }

    public abstract class IteratorAggregate : IEnumerable
    {
        // Returns an Iterator or another IteratorAggregate for the implementing
        // object.
        public abstract IEnumerator GetEnumerator();
    }

    // Concrete Iterators implement various traversal algorithms. These classes
    // store the current traversal position at all times.
    class ItemIterator : Iterator
    {
        private ItemCollection _collection;
        
        // Stores the current traversal position. An iterator may have a lot of
        // other fields for storing iteration state, especially when it is
        // supposed to work with a particular kind of collection.
        private int _position = -1;
        
        private bool _reverse = false;

        public ItemIterator(ItemCollection collection, bool reverse = false)
        {
            this._collection = collection;
            this._reverse = reverse;

            if (reverse)
            {
                this._position = collection.GetItems().Count;
            }
        }
        
        public override object Current()
        {
            return this._collection.GetItems()[_position];
        }
      
        public override int Key()
        {
            return this._position;
        }
        
        public override bool MoveNext()
        {
            int updatedPosition = this._position + (this._reverse ? -1 : 1);

            if (updatedPosition >= 0 && updatedPosition < this._collection.GetItems().Count)
            {
                this._position = updatedPosition;
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public override void Reset()
        {
            this._position = this._reverse ? this._collection.GetItems().Count - 1 : 0;
        }
    }

    // Concrete Collections provide one or several methods for retrieving fresh
    // iterator instances, compatible with the collection class.
    public class ItemCollection : IteratorAggregate
    {
        List<ItemDistributor> _collection = new List<ItemDistributor>();
        
        bool _direction = false;
        
        public void ReverseDirection()
        {
            _direction = !_direction;
        }
        
        public List<ItemDistributor> GetItems()
        {
            return _collection;
        }
        
        public void AddItem(ItemDistributor item)
        {
            this._collection.Add(item);
        }
        
        public override IEnumerator GetEnumerator()
        {
            return new ItemIterator(this, _direction);
        }
    }
}