using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private float _speed = 10f;
    [SerializeField]
    private float _primaryFireRate = 0.15f;
    private float _primaryNextFire = 0.0f;
    [SerializeField]
    private GameObject _primaryLaserPrefab;
    private Vector3 _primaryLaserOffset = new Vector3(0, 1.01f, 0);
    [SerializeField]
    private GameObject _tripleShotPrefab;
    private SpawnManager _spawnManager;
    private bool _isTripleShotActive = true;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }

        transform.position = new Vector3(0, 0, 0);
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _primaryNextFire)
        {
            FirePrimaryLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FirePrimaryLaser()
    {

        _primaryNextFire = Time.time + _primaryFireRate;
        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_primaryLaserPrefab, transform.position + _primaryLaserOffset, Quaternion.identity);
        }


    }

    public void Damage()
    {
        _lives--;

        if (_lives < 1)
        {
            _spawnManager.onPlayerDeath();
            Destroy(gameObject);
        }
    }
}