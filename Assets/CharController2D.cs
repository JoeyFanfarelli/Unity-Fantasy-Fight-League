using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController2D : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rb;
    private Vector2 moveVelocity;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed;
        
    }

    private void FixedUpdate()
    {
        if (rb.position.y + moveVelocity.y * Time.fixedDeltaTime < 4.5 && rb.position.y + moveVelocity.y * Time.fixedDeltaTime > -4.5 && rb.position.x + moveVelocity.x * Time.fixedDeltaTime < 7.3 && rb.position.x + moveVelocity.x * Time.fixedDeltaTime > -7.4) //Keep paddle in-bounds
        {
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition(rb.position);
        }
    }
}
