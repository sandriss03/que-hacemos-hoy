// PlayerController.cs
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;

    Vector2 movement;

    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}

// BallController.cs
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float kickForce = 10f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            GetComponent<Rigidbody2D>().AddForce(direction * kickForce, ForceMode2D.Impulse);
        }
    }
}

// EnemyAI.cs
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform ball;
    public float speed = 4f;

    void Update()
    {
        Vector2 direction = (ball.position - transform.position).normalized;
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }
}

// Goal.cs
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    public Text scoreText;
    private int playerScore = 0;
    private int enemyScore = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Ball"))
        {
            if(this.CompareTag("PlayerGoal")) 
                enemyScore++;
            else 
                playerScore++;

            scoreText.text = playerScore + " - " + enemyScore;
            other.transform.position = Vector2.zero;
            other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}

// KickButton.cs
using UnityEngine;

public class KickButton : MonoBehaviour
{
    public BallController ball;
    public float kickStrength = 15f;

    public void Kick()
    {
        Vector2 direction = (ball.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)).normalized;
        ball.GetComponent<Rigidbody2D>().AddForce(direction * kickStrength, ForceMode2D.Impulse);
    }
}

// InputTouch.cs
using UnityEngine;

public class InputTouch : MonoBehaviour
{
    public PlayerController player;

    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            Vector2 direction = (touchPos - player.transform.position).normalized;
            player.rb.MovePosition(player.rb.position + direction * player.speed * Time.deltaTime);
        }
    }
}
