using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController2 : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rb;
    private Vector2 moveVelocity;
    private Vector2 newPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal2"), Input.GetAxisRaw("Vertical2"));
        moveVelocity = moveInput.normalized * speed;
        
    }

    private void FixedUpdate()
    {

        if (rb.position.y + moveVelocity.y * Time.fixedDeltaTime < 6.31 && rb.position.y + moveVelocity.y * Time.fixedDeltaTime > -6.85 && rb.position.x + moveVelocity.x * Time.fixedDeltaTime < 16.45 && rb.position.x + moveVelocity.x * Time.fixedDeltaTime > -16.35) //Keep paddle in-bounds
        {
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition(rb.position);
        }
            
    }
}
