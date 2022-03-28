using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Straight shot
public class BulletBasic : AbstractBullet
{
    private Rigidbody2D rb;

    private void Awake() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        damage = 5;
    }

    private void Update() {
        Move();
    }

    // Does not work? Rotation instead set by barrel
    // Function not commented out because of abstract parent class
    public override void setDirection(Vector3 dir) {
        transform.LookAt(dir);
        Debug.DrawRay(transform.position, dir, Color.yellow, 2);
    }

    protected override void Move() {
        // Move along transform.forward axis
        float theta = transform.localEulerAngles.x;
        float dirX = Mathf.Cos(theta * Mathf.Deg2Rad);
        float dirY = -Mathf.Sin(theta * Mathf.Deg2Rad);
        if (transform.forward.x < 0f)
            dirX *= -1;
        direction = new Vector2(dirX, dirY);
        rb.velocity = direction * moveSpeed;

        // Vector3 ray = direction;
        // ray.z = 0;
        // Debug.DrawRay(transform.position, ray, Color.red);
        // Debug.DrawRay(transform.position, transform.forward, Color.blue);

        // Debug.Log("x"+ transform.localEulerAngles.x);
        // Debug.Log("y"+ transform.localEulerAngles.y);
        // Debug.Log("z"+ transform.localEulerAngles.z);
    }

    private void OnDisable() {
        rb.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Ground")) {
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Player")) {
            gameObject.SetActive(false);
            // Deal Damage
            Health playerHp = other.GetComponent<Health>();
            playerHp.LoseHealth(damage);
        }
    }
}
