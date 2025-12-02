using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class Connection : MonoBehaviourPunCallbacks
{
    public static int IDjugador;
    private bool cone;
    public Button[] botonesInicio;
    public Button volverAInicio;
    public GameObject panelCrearSala;
    public GameObject panelUnirse;
    public GameObject panelSalaMaster;
    public GameObject panelSalaClient;

    public TextMeshProUGUI NombreSala;
    public Button botonCrearSala;
    void Start()
    {
        botonesInicio[0].interactable = false;
        botonesInicio[1].interactable = false;
        cone = false;
        PhotonNetwork.ConnectUsingSettings();
    }

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    override public void OnConnectedToMaster()
    {
        botonesInicio[0].interactable = true;
        botonesInicio[1].interactable = true;
        Debug.Log("Conectado al servidor");
    }

    public void CrearSala()
    {
        if ( PhotonNetwork.CountOfRooms <= 3)
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 2;

            PhotonNetwork.JoinOrCreateRoom(NombreSala.text, options, TypedLobby.Default);
            botonCrearSala.interactable = false;
            panelCrearSala.SetActive(false);
            panelSalaMaster.SetActive(true);
        }

    }

    override public void OnJoinedRoom()
    {
        Debug.Log("Conectado a la sala " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("hay " + PhotonNetwork.CurrentRoom.PlayerCount + " jugadores conectados");
        IDjugador = PhotonNetwork.LocalPlayer.ActorNumber;
        Debug.Log("ID del jugador: " + IDjugador);

    }

    public void IrACrearSala()
    {
        panelCrearSala.SetActive(true);
    }

    public void VolverAlInicio()
    {
        panelCrearSala.SetActive(false);
        panelUnirse.SetActive(false);

    }

    public void VolverAlInicioDesdeSala()
    {
        panelSalaMaster.SetActive(false);
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer == newMasterClient)
        {
            photonView.RPC("RPC_SalirSala", RpcTarget.All);
        }
    }

    [PunRPC]
    public void RPC_SalirSala()
    {
        PhotonNetwork.LeaveRoom();
        panelSalaClient.SetActive(false);
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
            else if (!PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount > 1)
            {
                PhotonNetwork.LoadLevel(1);
                Destroy(this);
                
            }
        }
    }


}
