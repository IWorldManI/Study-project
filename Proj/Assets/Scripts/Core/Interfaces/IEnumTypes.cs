using System;
using System.Collections.Generic;
using UnityEngine;

public interface IEnumTypes 
{
    public enum ItemTypes
    {
        Milk, Tomatoes, Ketchup,
    }
    
    public static readonly Dictionary<ItemTypes, Type> DictionaryOfStandTypes = new Dictionary<ItemTypes, Type> 
    {
        { ItemTypes.Milk, typeof(Milk) },
        { ItemTypes.Tomatoes, typeof(Tomatoes) },
        { ItemTypes.Ketchup, typeof(Ketchup) },
    };
}
