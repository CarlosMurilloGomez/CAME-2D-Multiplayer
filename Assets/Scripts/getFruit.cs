using Photon.Pun;
using UnityEngine;

public class getFruit : MonoBehaviourPunCallbacks
{
    public int points;
    private GameObject gameManager;
    private Animator animator;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PhotonView pv = collision.gameObject.GetComponent<PhotonView>();
        if (pv != null)
        {
            Debug.Log("PhotonView encontrado. ViewID: " + pv.ViewID);
            Debug.Log("PhotonView encontrado. Owner: " + pv.Owner.ActorNumber);
            Debug.Log("PhotonView encontrado. Mine: " + pv.IsMine);
        }
        
        if (pv.IsMine)
        {
            Debug.Log("Es Mio");
            Debug.Log("El gameController: " + gameManager.name);
            gameManager.GetComponent<GameManager>().addPoints(points, pv.Owner.ActorNumber);
            photonView.RPC("DestroyAnimation", RpcTarget.All);

        }

    }
   
    [PunRPC]
    public void DestroyAnimation()
    {
        animator.SetTrigger("getFruit");

    }

    public void DestroyFruit()
    {
        photonView.RPC("DestroyObject", RpcTarget.All);
    }

    [PunRPC]
    private void DestroyObject()
    {
        Destroy(gameObject);

    }
}
