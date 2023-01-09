using UnityEngine;
using UnityEngine.UI;

public class TestItemsOnScreen : MonoBehaviour, IEnumTypeToVisual
{
    public Sprite a1, b1, c1;
    
    private Sprite a, b, c;

    private void Start()
    {
        a = IEnumTypeToVisual.DictionaryOfImages[typeof(Milk)];
        b = IEnumTypeToVisual.DictionaryOfImages[typeof(Tomatoes)];
        c = IEnumTypeToVisual.DictionaryOfImages[typeof(Ketchup)];
        a1 = a;
        b1 = b;
        c1 = c;
    }
}
