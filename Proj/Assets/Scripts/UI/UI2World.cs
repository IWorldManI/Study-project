using UnityEngine;
using UnityEngine.UI;

public class UI2World : MonoBehaviour
{
    private Camera _cam;
    private Canvas _canvas;

    private RectTransform _canvasRect;
    
    public PooledShape _target;
    
    private RawImage _spriteHolder;
 
    //this is the ui element
    private RectTransform _uiElement;

    private void Start()
    {
        _cam = FindObjectOfType<Camera>();
        _canvas = FindObjectOfType<CanvasForFollowingItems>().GetComponent<Canvas>();
        
        //first you need the RectTransform component of your canvas
        _canvasRect = _canvas.GetComponent<RectTransform>();
        _uiElement = GetComponent<RectTransform>();
        _spriteHolder = GetComponent<RawImage>();
        transform.parent = _canvas.transform;
    }

    public void UpdateOrderImage(Sprite image)
    {
        _spriteHolder.texture = image.texture;
    }

    private void LateUpdate()
    {
        Vector2 viewportPosition=_cam.WorldToViewportPoint(_target.transform.position + new Vector3(0,4,0));
        var sizeDelta = _canvasRect.sizeDelta;
        
        Vector2 targetScreenPosition=new Vector2(
            ((viewportPosition.x*sizeDelta.x)-(sizeDelta.x*0.5f)),
            ((viewportPosition.y*sizeDelta.y)-(sizeDelta.y*0.5f)));
 
        //now you can set the position of the ui element
        _uiElement.anchoredPosition=targetScreenPosition;
    }
}
