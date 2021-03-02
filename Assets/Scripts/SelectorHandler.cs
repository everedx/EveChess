using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorHandler : MonoBehaviour
{

    [SerializeField] PointerController pointer;
    Outline outline;

    [SerializeField] Gradient colorOverTime;
    [SerializeField] float colorTime;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        HighLight();
        Select();
    }


    private void HighLight()
    {
        if (pointer.HighlightedPiece == gameObject)
        {
            timer += Time.deltaTime;
            if (timer >= colorTime)
                timer = 0;
            outline.OutlineColor = colorOverTime.Evaluate(timer / colorTime);
            outline.OutlineWidth = 3;
        }
        else
        {
            timer = 0;
            outline.OutlineWidth = 0;
        }
    }

    private void Select()
    {
        if (pointer.SelectedPiece == gameObject)
        {
            outline.OutlineWidth = 5;
            outline.OutlineColor = colorOverTime.Evaluate(0.5f);
        }
        else if ( pointer.HighlightedPiece != gameObject )
            outline.OutlineWidth = 0;
    }
}
