using System;
using System.Collections.Generic;
using UnityEngine;

public interface IEnumTypeToVisual
{
    public static readonly Dictionary<Type, Sprite> DictionaryOfImages = new Dictionary<Type, Sprite> 
    {
        { typeof(Milk), Resources.Load<Sprite>( "Images/Ingredients/Milk" ) },
        { typeof(Tomatoes), Resources.Load<Sprite>( "Images/Ingredients/Tomatoes" ) },
        { typeof(Ketchup), Resources.Load<Sprite>( "Images/Ingredients/Ketchup" ) },
    };
}
