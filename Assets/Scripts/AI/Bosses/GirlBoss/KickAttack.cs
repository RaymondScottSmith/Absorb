using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickAttack : MonoBehaviour
{

    public int attackDamage = 20;

    public Vector3 attackOffset;
    public float attackRange = 1.5f;

    public LayerMask attackMask;

    private Rigidbody2D rb;

    private PlayerController pc;
    private GirlBoss gb;

    [SerializeField] private AudioClip kickHitSound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = FindObjectOfType<PlayerController>();
        gb = GetComponent<GirlBoss>();
    }

    public void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        
        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null && colInfo.CompareTag("Player"))
        {
            pc.TakeDamage(attackDamage,kickHitSound);
            if (gb.isFlipped)
            {
                pc.ChangeDirection((Vector3.up + Vector3.right) * 10f);
                //StartCoroutine(LaunchBack());
                /*
                gb.transform.position = new Vector3(gb.transform.position.x - 5f, gb.transform.position.y,
                    gb.transform.position.z);
                    */
            }
            else
            {
                pc.ChangeDirection((Vector3.up + Vector3.left) * 10f);
            }
            
        }
        
        if (gb.isGrounded)
        {
            /*
            float velocity = 0.25f;
            float yPos = (rb.position.y + velocity) * Time.deltaTime;
            //float xPos = rb.position.x + velocity;
            //Vector2 newPos = rb.transform.position + (Vector3.left) * 5f;
            //rb.MovePosition(newPos);
            rb.MovePosition(new Vector3(rb.position.x, yPos));
            */
            gb.JumpBack();
        }

    }

    private IEnumerator LaunchBack()
    {
        //rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce((Vector3.up + Vector3.left), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.25f);
        rb.bodyType = RigidbodyType2D.Kinematic;
    }
}
