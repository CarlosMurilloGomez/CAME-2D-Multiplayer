using Photon.Pun;
using UnityEngine;

public class getFruit : MonoBehaviourPunCallbacks
{
    public int points;
    public static int score = 0;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Player2"))
        {
            score += points;
            print("+" + points);
            animator.SetTrigger("getFruit");
        }
    }

    //private void DestroyFruit()
    //{
    //    GetComponent<PhotonView>().RPC("DestroyObject", RpcTarget.All, gameObject);
    //}

    [PunRPC]
    private void DestroyObject()
    {
        Destroy(gameObject);

    }
}
