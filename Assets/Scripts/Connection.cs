using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Connection : MonoBehaviourPunCallbacks
{
    public static int IDjugador;
    private bool cone;
    public Button boton;
    void Start()
    {
        cone = false;
        PhotonNetwork.ConnectUsingSettings();
    }

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    override public void OnConnectedToMaster()
    {
        Debug.Log("Conectado al servidor");
    }

    public void ButtonConnect()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;

        PhotonNetwork.JoinOrCreateRoom("room1", options, TypedLobby.Default);
        cone = true;
        boton.interactable = false;
    }

    override public void OnJoinedRoom()
    {
        Debug.Log("Conectado a la sala " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("hay " + PhotonNetwork.CurrentRoom.PlayerCount + " jugadores conectados");
        IDjugador = PhotonNetwork.LocalPlayer.ActorNumber;
        Debug.Log("ID del jugador: " + IDjugador);

       

        
    }

    private void Update()
    {
        if (cone)
        {
            if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount > 1)
            {
                PhotonNetwork.LoadLevel(1);
                Destroy(this);
            }
            else
            {
                if (!PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount > 1)
                {
                    PhotonNetwork.LoadLevel(1);
                    Destroy(this);
                }
            }
        }
    }


}
