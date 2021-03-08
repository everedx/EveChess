using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GUIPieceSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] Pieces pieceType;

    Image Image;
    bool mouseIsoverMe;
    // Start is called before the first frame update
    void Start()
    {
        Image = GetComponent<Image>();
        if (BoardController.instance.EatenPieces.Find(x => x.Color == NetworkHandler.instance.MyColor && x.Type == pieceType) == null)
            gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (mouseIsoverMe)
            Image.color = new Color(0, 1, 1);
        else
            Image.color = new Color(1, 1, 1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
            mouseIsoverMe = true;
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseIsoverMe = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Selected " + pieceType);
        BoardController.instance.ReplacePawn(pieceType);
        transform.parent.parent.gameObject.SetActive(false);
    }
}
