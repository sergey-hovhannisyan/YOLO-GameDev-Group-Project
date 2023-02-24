using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public int pointValue = 1;
    public float enemySpeed = 500f;
    public int lives = 1;
    Rigidbody2D _rigidbody2D;

    private GameObject player;
    public GameObject selfExplosion;
    public GameObject bulletSpikes;
    public AudioClip selfExplosionClip;

    GameManager _gameManager;
    
    void Awake()
    {
        // Waits before starting attack
        StartCoroutine(WaitSeconds(0.2f));
    }

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerDirection = (player.transform.position - transform.position).normalized;
        _rigidbody2D.AddForce(playerDirection * enemySpeed * Time.deltaTime);
        transform.rotation = Quaternion.FromToRotation(Vector2.up, playerDirection);
    }

    void OnTriggerEnter2D(Collider2D other)
    { 
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            Instantiate(bulletSpikes, transform.position, Quaternion.identity);
            if (lives == 0)
            {
                AudioSource.PlayClipAtPoint(selfExplosionClip, transform.position, 1f);
                Instantiate(selfExplosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
                _gameManager.AddScore(pointValue);
            }
            else
            {
                lives--;
            }
        }
    }
    
    // Wait couroutine
    private IEnumerator WaitSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
