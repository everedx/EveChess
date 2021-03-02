using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConnectedClientPrefab : MonoBehaviour
{


    public void SetID(string ID)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = ID;
    }
}
