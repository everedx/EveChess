using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrackingHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] textMeshes;
    [SerializeField] TMP_FontAsset fontWhites;
    [SerializeField] TMP_FontAsset fontBlacks;

    public void SetTrackingText(string text, ChessColors color)
    {
        for (int i = textMeshes.Length - 2; i >= 0; i--)
        {
            textMeshes[i + 1].text = textMeshes[i].text;
            textMeshes[i + 1].font = textMeshes[i].font;
        }

        textMeshes[0].text = text;
        if(color == ChessColors.White)
            textMeshes[0].font = fontWhites;
        else
            textMeshes[0].font = fontBlacks;

    }
}
