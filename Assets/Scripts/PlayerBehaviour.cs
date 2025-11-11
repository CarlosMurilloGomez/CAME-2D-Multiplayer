using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public float speed, jumpForce;
    private Rigidbody2D rb2D;
    private Animator animator;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //CORRER
        rb2D.linearVelocity = (transform.right * speed * Input.GetAxis("Horizontal")) + (transform.up * rb2D.linearVelocityY);

        if (rb2D.linearVelocityX > 0.1f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (rb2D.linearVelocityX < -0.1f)
        {
            GetComponent <SpriteRenderer>().flipX = true;
        }

        //SALTAR
        if (Input.GetButtonDown("Jump") && animator.GetFloat("VelocityY") == 0)
        {
            rb2D.AddForce(transform.up * jumpForce);
        }

        //ANIMACIONES
        animator.SetFloat("velocityX", Mathf.Abs(rb2D.linearVelocityX));
        animator.SetFloat("velocityY", rb2D.linearVelocityY);
    }
}
