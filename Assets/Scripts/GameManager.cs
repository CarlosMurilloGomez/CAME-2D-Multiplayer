using UnityEngine;
using Photon.Pun;
public class GameManager : MonoBehaviourPunCallbacks
{
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
}
