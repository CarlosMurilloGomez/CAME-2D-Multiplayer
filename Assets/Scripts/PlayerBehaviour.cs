using UnityEngine;
using Photon.Pun;

public class PlayerBehaviour : MonoBehaviourPunCallbacks
{
    public float speed, jumpForce;
    private Rigidbody2D rb2D;
    private Animator animator;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (photonView.IsMine)
        {
            
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.position = transform.position + Vector3.up + (transform.forward * -10);
        }

    }

    void Update()
    {
        if (photonView.IsMine)
        {
            //CORRER
            rb2D.linearVelocity = (transform.right * speed * Input.GetAxis("Horizontal")) + (transform.up * rb2D.linearVelocityY);

            if (rb2D.linearVelocityX > 0.1f)
            {
                photonView.RPC("RotateSprite", RpcTarget.All, false);
            }
            else if (rb2D.linearVelocityX < -0.1f)
            {
                photonView.RPC("RotateSprite", RpcTarget.All, true);
            }

            //SALTAR
            if (Input.GetButtonDown("Jump") && Mathf.Abs(rb2D.linearVelocityY) < 0.2)
            {
                rb2D.AddForce(transform.up * jumpForce);
            }
            //ANIMACIONES
            float vx = Mathf.Abs(rb2D.linearVelocityX);
            float vy = rb2D.linearVelocityY;
            


            animator.SetFloat("velocityX", vx);
            animator.SetFloat("velocityY", vy);

        }



    }


    [PunRPC]
    public void RotateSprite(bool rotate)
    {
        GetComponent<SpriteRenderer>().flipX = rotate;
    }

   
}
