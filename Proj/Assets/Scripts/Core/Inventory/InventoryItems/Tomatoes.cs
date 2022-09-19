using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomatoes : Ingredient
{
    public void Move(Transform _target)
    {
        StartCoroutine(base.Move(_target));
    }
}
