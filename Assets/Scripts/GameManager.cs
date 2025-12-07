using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class GameManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI scoreP1;
    public TextMeshProUGUI scoreP2;

    public TextMeshProUGUI nombreP1;
    public TextMeshProUGUI nombreP2;

    public GameObject panelSalir;

    public GameObject items;
    public static bool gameOver;
    public TextMeshProUGUI ganador;
    public GameObject panelGameOver;


    void Start()
    {
        gameOver = false;
        nombreP1.text = PhotonNetwork.PlayerList[0].NickName + ": ";
        nombreP2.text = PhotonNetwork.PlayerList[1].NickName + ": ";
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("player1", new Vector3(0, -1.3f, 0), Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("player2", new Vector3(-2, -1.3f, 0), Quaternion.identity);

        }
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(0);
        Destroy(this);

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LeaveRoom();
        }
    }
    void Update()
    {
        if (items.transform.childCount == 0)
        {
            gameOver = true;
            if (int.Parse(scoreP1.text) > int.Parse(scoreP2.text))
            {
                ganador.text = PhotonNetwork.PlayerList[0].NickName;
            }
            else if (int.Parse(scoreP1.text) < int.Parse(scoreP2.text))
            {
                ganador.text = PhotonNetwork.PlayerList[1].NickName;

            }
            else
            {
                ganador.text = "EMPATE";
            }
        }

        if (!gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (panelSalir.activeSelf)
                {
                    panelSalir.SetActive(false);
                }
                else
                {
                    panelSalir.SetActive(true);
                }
            }
        }
        else
        {
            
            panelGameOver.SetActive(true);

        }

    }

    public void salir()
    {
        PhotonNetwork.LeaveRoom();

    }

    public void noSalir()
    {
        panelSalir.SetActive(false);

    }


    public void addPoints(int points, int player)
    {
        PhotonView pv = gameObject.GetComponent<PhotonView>();
        pv.RPC("addPointsRPC", RpcTarget.All, player, points);

    }

    [PunRPC]
    public void addPointsRPC(int player, int points)
    {
        if (player == 1)
        {
            scoreP1.text = (int.Parse(scoreP1.text) + points).ToString();
        }else
        {
            scoreP2.text = (int.Parse(scoreP2.text) + points).ToString();
        }
    }
}
