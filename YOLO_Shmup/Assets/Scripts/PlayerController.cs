using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float speed = 10f;
    float angleSpeed = 50f;
    float bulletSpeed = 1000f;
    float timeInterval = 0.07f;
    bool hitTimeFlag = true;

    Rigidbody2D _rigidbody2D;
    public GameObject bulletPrefab;
    public GameObject respawn_PS;
    public Transform spawn;
    public AudioClip gun_shot_clip;
    public EnemySpawner _enemySpawner;

    private bool isShooting = false;

    GameManager _gameManager;

    // Initialization
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();

    }

    // Updates for every frame 
    void Update()
    {
        float xSpeed = Input.GetAxis("Horizontal") * speed;
        float ySpeed = Input.GetAxis("Vertical") * speed;
        _rigidbody2D.velocity = new Vector2(xSpeed, ySpeed);

        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, angleSpeed * Time.deltaTime);

        if (Input.GetMouseButtonDown(0))
        {
            isShooting = true;
            StartCoroutine(FireGun());
        }
        if (Input.GetMouseButtonUp(0))
        {
            isShooting = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && hitTimeFlag)
        {
            hitTimeFlag = false;
            _gameManager.UpdateLives(-1);
            StartCoroutine(WaitSeconds(2f));
            hitTimeFlag = true;
        }
    }

    // Player respawn: 
    public void Respawn()
    {
        StartCoroutine(WaitSeconds(2));
        Vector2 previousPosition = transform.position;
        Vector2 spawnPos = previousPosition;

        while (Vector2.Distance(previousPosition, spawnPos) < 2.0f)
        {
            spawnPos = new Vector2(Random.Range(-8.25f, 8.25f), Random.Range(-4.5f, 4.5f));
        }
        Instantiate(respawn_PS, spawnPos, Quaternion.identity);
        transform.position = spawnPos;
        _enemySpawner.enabled = false;
        StartCoroutine(WaitSeconds(3));
        _enemySpawner.enabled = true;

    }
    private IEnumerator FireGun()
    {
        while (isShooting)
        {
            GameObject newBullet = Instantiate(bulletPrefab, spawn.position, Quaternion.identity);
            newBullet.transform.rotation = transform.rotation;
            newBullet.GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed);
            AudioSource.PlayClipAtPoint(gun_shot_clip, spawn.position, 1f);
            yield return new WaitForSeconds(timeInterval);
        }
    }

    // Wait coroutine
    private IEnumerator WaitSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}