using UnityEngine;
using Photon.Pun;
using TMPro;
public class GameManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI scoreP1;
    public TextMeshProUGUI scoreP2;
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("player1", new Vector3(0, -1.3f, 0), Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("player2", new Vector3(-2, -1.3f, 0), Quaternion.identity);
        }
    }

    void Update()
    {
        
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
