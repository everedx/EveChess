using DarkRift.Client;
using DarkRift.Client.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionText : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI textConnected;
    [SerializeField] GameObject progressIteml;
    [SerializeField] GameObject clientObject;
    [SerializeField] Button buttonReady;

    UnityClient client;
    // Start is called before the first frame update
    void Start()
    {

        client = clientObject.GetComponent<UnityClient>();

        InvokeRepeating("CheckConnectionServer", 0, 1);

    }


    private void CheckConnectionServer()
    {
        if (client.ConnectionState == DarkRift.ConnectionState.Connected)
        {
            textConnected.gameObject.SetActive(true);
            progressIteml.SetActive(false);
            buttonReady.gameObject.SetActive(true);
        }
        else
        {
            textConnected.gameObject.SetActive(false);
            progressIteml.SetActive(true);
            buttonReady.gameObject.SetActive(false);
        }
            
    }


}
