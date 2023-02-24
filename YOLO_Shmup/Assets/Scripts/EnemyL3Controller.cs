using UnityEngine;
using System.Collections;

public class EnemyL3Controller : MonoBehaviour
{
    public int pointValue = 10;
    public float enemySpeed = 500f;
    Rigidbody2D _rigidbody2D;
    public int lives = 20;

    private GameObject player;
    float bulletSpeed = 500f;
    float targetDistance = 8f;
    float period = 1.5f;
    float nextShoot = 0f;

    public GameObject bulletPrefab;
    public AudioClip gun_shot_clip;
    public Transform spawn;
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
        FireGun();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 targetPosition = player.transform.position - (player.transform.position - transform.position).normalized * targetDistance;
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        _rigidbody2D.AddForce(direction * enemySpeed * Time.deltaTime);
        transform.rotation = Quaternion.FromToRotation(Vector2.up, player.transform.position - transform.position);
        if (Time.deltaTime > nextShoot)
        {
            nextShoot += period;
            StartCoroutine(FireGun());
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Hit with bullet
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            Instantiate(bulletSpikes, transform.position, Quaternion.identity);
            if (lives == 0)
            {
                AudioSource.PlayClipAtPoint(selfExplosionClip, transform.position, 1f);
                Destroy(gameObject);
                _gameManager.AddScore(pointValue);
            }
            else
            {
                lives--;
            }   
        }
    }

    private IEnumerator FireGun()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 1.5f)); // wait for a random amount of time
            GameObject newBullet = Instantiate(bulletPrefab, spawn.position, Quaternion.identity);
            newBullet.transform.rotation = transform.rotation;
            newBullet.GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed);
            AudioSource.PlayClipAtPoint(gun_shot_clip, spawn.position, 1f);
        }
    }

    // Wait couroutine
    private IEnumerator WaitSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
