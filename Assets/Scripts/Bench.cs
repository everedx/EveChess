using Assets.Scripts;
using Assets.Scripts.Chess;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bench : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI pawnsText, knightsText, RookText, BishopText, QueenText;
    [SerializeField] ChessColors benchColor;

    public void UpdateBench(List<Piece> eatenPieces)
    {
        pawnsText.text = "x" + eatenPieces.FindAll(x => x.Type == Pieces.Pawn && x.Color == benchColor).Count;
        knightsText.text = "x" + eatenPieces.FindAll(x => x.Type == Pieces.Knight && x.Color == benchColor).Count;
        RookText.text = "x" + eatenPieces.FindAll(x => x.Type == Pieces.Rook && x.Color == benchColor).Count;
        BishopText.text = "x" + eatenPieces.FindAll(x => x.Type == Pieces.Bishop && x.Color == benchColor).Count;
        QueenText.text = "x" + eatenPieces.FindAll(x => x.Type == Pieces.Queen && x.Color == benchColor).Count;
    }
}
