using Assets.Scripts;
using Core.Utils;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkHandler : PersistentSingleton<NetworkHandler>
{
    UnityClient client;
    [SerializeField] GameObject prefabConnectedClient;


    ChessColors myColor = ChessColors.White;

    public ChessColors MyColor { get => myColor;  }


    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1024,576,false);

        client = GetComponent<UnityClient>();
        client.MessageReceived += ManageMessages;
    }


    private void ManageMessages(object sender, MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage())
        using (DarkRiftReader reader = message.GetReader())
        {

            switch (message.Tag)
            {
                case 0:
                    GameObject parent = GameObject.Find("Clients");
                    if (parent != null)
                    {

                        if (reader.Length % 2 != 0)
                        {
                            Debug.LogWarning("Received malformed connection packet.");
                            return;
                        }

                        for (int i = parent.transform.childCount - 1; i >= 0; i--)
                            Destroy(parent.transform.GetChild(i).gameObject);

                        while (reader.Position < reader.Length)
                        {
                            ushort id = reader.ReadUInt16();

                            GameObject go = Instantiate(prefabConnectedClient, parent.transform);
                            go.GetComponent<ConnectedClientPrefab>().SetID(id.ToString());

                        }
                    }
                    break;

                case 2:

                    if (reader.ReadBoolean())
                    {
                        myColor = ChessColors.White;
                    }
                    else
                    {
                        myColor = ChessColors.Black;
                    }

                    SceneManager.LoadScene("Game");

                    break;

                case 3:
                    char originColumn = reader.ReadChar();
                    char originRow = reader.ReadChar();
                    char targetColumn = reader.ReadChar();
                    char targetRow = reader.ReadChar();

                    BoardController.instance.MovePieceFromNetwork(originColumn, originRow, targetColumn, targetRow);

                    break;
                case 4:
                    char column = reader.ReadChar();
                    char row = reader.ReadChar();
                    Debug.Log("Eaten: " + column + row);
                    BoardController.instance.EatPieceFromNetwork(column, row);
                    break;
            }
           
               
                
            
        }
    }


    public void SendReadyMessage()
    {
        using (DarkRiftWriter messageWriter = DarkRiftWriter.Create())
        {

            messageWriter.Write(client.ID);
            using (Message playerReadyMessage = Message.Create(1, messageWriter))
            {
                client.SendMessage(playerReadyMessage, SendMode.Reliable);
            }
        }
    }


    public void SendMovement(char originColumn,char originRow, char targetColumn, char targetRow)
    {
        using (DarkRiftWriter messageWriter = DarkRiftWriter.Create())
        {
            messageWriter.Write(originColumn);
            messageWriter.Write(originRow);
            messageWriter.Write(targetColumn);
            messageWriter.Write(targetRow);
            using (Message movementMessage = Message.Create(3, messageWriter))
            {
                client.SendMessage(movementMessage, SendMode.Reliable);
            }
        }
    }

    public void SendEatenPiece(char column, char row)
    {
        using (DarkRiftWriter messageWriter = DarkRiftWriter.Create())
        {
            messageWriter.Write(column);
            messageWriter.Write(row);
            using (Message eatenMessage = Message.Create(4, messageWriter))
            {
                client.SendMessage(eatenMessage, SendMode.Reliable);
            }
        }
    }



}
