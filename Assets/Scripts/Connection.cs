using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

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
    public Button btnJugar;

    public TextMeshProUGUI NombreSala;
    public Button botonCrearSala;

    public TextMeshProUGUI nombreJugador1;
    public TextMeshProUGUI nombreJugador2;

    public GameObject roomItemPrefab;
    public Transform roomListContent;

    private Dictionary<string, RoomInfo> cachedRooms = new Dictionary<string, RoomInfo>();
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
        PhotonNetwork.JoinLobby();

    }

    public void CrearSala()
    {
        if ( PhotonNetwork.CountOfRooms <= 3)
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 2;


            PhotonNetwork.CreateRoom(NombreSala.text, options, TypedLobby.Default);
            
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
        GameObject.Find("NombreSalaText").GetComponent<TextMeshProUGUI>().text = "Sala: " + PhotonNetwork.CurrentRoom.Name;
        cone = true;
    }

    public void IrACrearSala()
    {
        panelCrearSala.SetActive(true);
    }

    public void IrAUnirse()
    {
        panelUnirse.SetActive(true);
    }

    public void VolverAlInicio()
    {
        panelCrearSala.SetActive(false);
        panelUnirse.SetActive(false);

    }

    public void VolverAlInicioDesdeSala()
    {
        panelSalaMaster.SetActive(false);
        panelSalaClient.SetActive(false);
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            cone = false;
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

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Ha entrado un jugador: " + newPlayer.NickName);
        ActualizarBotonJugar();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Ha salido un jugador: " + otherPlayer.NickName);
        ActualizarBotonJugar();
    }

    private void ActualizarBotonJugar()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            btnJugar.interactable = true;
        }
        else
        {
            btnJugar.interactable = false;
        }
    }

    private void Update()
    {

        if (NombreSala.text.Trim().Length == 1)
        {
            botonCrearSala.interactable = false;
        }
        else
        {
            botonCrearSala.interactable = true;
        }

        if (cone)
        {


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Tipo de componente: " + nombreJugador1.GetType().Name);
                Debug.Log("Texto raw: '" + nombreJugador1.text + "' (len=" + nombreJugador1.text.Length + ")");
                if (nombreJugador1.text.Trim().Length == 1)
                {
                    PhotonNetwork.LocalPlayer.NickName = "Player1";
                }
                else
                {
                    PhotonNetwork.LocalPlayer.NickName = nombreJugador1.text;

                }

            }
            else
            {
                if (nombreJugador2.text.Trim().Length == 1)
                {
                    PhotonNetwork.LocalPlayer.NickName = "Player2";

                }
                else
                {
                    PhotonNetwork.LocalPlayer.NickName = nombreJugador2.text;

                }
            }
            //Debug.Log("Nuevo NickName: " + PhotonNetwork.LocalPlayer.NickName);

        }
    }

    public void jugar()
    {
        PhotonNetwork.LoadLevel(1);
        Destroy(this);

    }




    // Cuando Photon recibe cambios en la lista de salas
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // Actualizar caché interna
        foreach (RoomInfo room in roomList)
        {
            Debug.Log("Sala encontrada: " + room.Name + " jugadores: " + room.PlayerCount);

            if (room.RemovedFromList)   // Si ya no existe
            {
                if (cachedRooms.ContainsKey(room.Name))
                    cachedRooms.Remove(room.Name);
            }
            else
            {
                // Solo salas que NO están llenas
                if (room.PlayerCount < room.MaxPlayers)
                    cachedRooms[room.Name] = room;
            }
        }

        UpdateRoomListUI();
    }

    void UpdateRoomListUI()
    {
        // Limpiar lista actual
        foreach (Transform child in roomListContent)
        {
            Destroy(child.gameObject);
        }

        // Crear un item por sala
        foreach (RoomInfo room in cachedRooms.Values)
        {
            GameObject newRoomItem = Instantiate(roomItemPrefab, roomListContent);

            newRoomItem.transform.Find("SalaNombre").GetComponent<TextMeshProUGUI>().text = room.Name;

            Button joinButton = newRoomItem.transform.Find("btnUnirseSala").GetComponent<Button>();
            joinButton.onClick.AddListener(() =>
            {
                PhotonNetwork.JoinRoom(room.Name);
                panelUnirse.SetActive(false);
                panelSalaClient.SetActive(true);
            });

        }
    }







}
