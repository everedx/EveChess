using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour
{
    GameObject trackingObject;
    bool needToStartTracking;
    [SerializeField] Camera cameraWhites;
    [SerializeField] Camera cameraBlacks;
    [SerializeField] BoardController board;

    [SerializeField] Material selectionMatWhite;
    [SerializeField] Material selectionMatBlack;

    private Camera cameraUsed;

    private GameObject highightedPiece;
    private GameObject selectedPiece;
    public GameObject HighlightedPiece { get => highightedPiece;  }
    public GameObject SelectedPiece { get => selectedPiece;  }

    private void Start()
    {
        if (NetworkHandler.instance.MyColor == ChessColors.Black)
            cameraUsed = cameraBlacks;
        else
            cameraUsed = cameraWhites;
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = cameraUsed.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            if (objectHit != null && objectHit.GetComponent<Outline>() != null && selectedPiece != objectHit.gameObject)
                highightedPiece = objectHit.gameObject;
            else
                highightedPiece = null;

        }
        else
            highightedPiece = null;


        if (Input.GetMouseButtonDown(0) && highightedPiece != null )
        {
            selectedPiece = highightedPiece;
            board.SelectPiece(selectedPiece);
        }

        if (Input.GetMouseButtonDown(0) && highightedPiece == null)
        {
            if (hit.transform!=null && hit.transform.gameObject != selectedPiece)
            {
                selectedPiece = null;
                board.SelectPiece(null);
            }
            
        }


        if (Input.GetMouseButton(0) && selectedPiece != null)
        {

            if (((BoardController.instance.GetBoxFromCoordinates(selectedPiece.transform.position).GetPiece().Color == NetworkHandler.instance.MyColor && BoardController.instance.IsMyTurn) || BoardController.instance.byPassTurns) && !BoardController.instance.GameOver)
            {
                if (trackingObject == null)
                    needToStartTracking = true;
            }
            

        }

        if (Input.GetMouseButtonUp(0))
        {
            needToStartTracking = false;
            if (trackingObject != null)
            {
                ///Move piece!!
                BoardController.instance.MovePiece(selectedPiece, trackingObject.transform.position);

                Destroy(trackingObject);
                Debug.Log("EndTracking");
                selectedPiece = null;
                board.SelectPiece(null);
            }
            
            
        }
            


        TrackPiece();

    }



    private void TrackPiece()
    {
        if (needToStartTracking)
        {
            Debug.Log("StartTracking");
            trackingObject = Instantiate(selectedPiece);
            MeshRenderer rend = trackingObject.GetComponent<MeshRenderer>();
            if (BoardController.instance.GetBoxFromCoordinates(selectedPiece.transform.position).GetPiece().Color == ChessColors.White)
                rend.material = selectionMatWhite;
            else
                rend.material = selectionMatBlack;
            needToStartTracking = false;
        }

        if (trackingObject != null)
        {
            RaycastHit hit;
            Ray ray = cameraUsed.ScreenPointToRay(Input.mousePosition);
     
            if (Physics.Raycast(ray, out hit ,float.MaxValue, LayerMask.GetMask("Board")))
            {
                if(hit.collider != null)
                    trackingObject.transform.position = BoardController.instance.GetCoordinatesFromBox(BoardController.instance.GetBoxFromCoordinates(hit.point));

            }
            
        }
    }
}
