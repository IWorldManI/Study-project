using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMultiplier : MonoBehaviour
{
    private Slider _slider;
    private bool _filled = false;
    
    //Limits of win range
    [SerializeField] private float min, max;
    
    private void Start()
    {
        _slider = GetComponent<Slider>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_slider.value >= min && _slider.value <= max)
            {
                Debug.Log("Win");
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        if (!_filled)
        {
            _slider.value += 0.01f;
            if (_slider.value >= 1)
                _filled = true;
        }
        else if(_filled)
        {
            _slider.value -= 0.01f;
            if (_slider.value <= 0)
                _filled = false;
        }
    }
}
