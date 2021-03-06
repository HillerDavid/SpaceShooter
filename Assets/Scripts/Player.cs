﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private float _speed = 10f;
    private float _speedMultiplier = 1.5f;
    [SerializeField]
    private float _primaryFireRate = 0.15f;
    private float _primaryNextFire = 0.0f;
    [SerializeField]
    private GameObject _primaryLaserPrefab;
    private Vector3 _primaryLaserOffset = new Vector3(0, 1.01f, 0);
    [SerializeField]
    private GameObject _tripleShotPrefab;
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject[] _damageVisualizer;

    private bool _isTripleShotActive = false;
    private bool _isSpeedActive = false;
    private bool _isShieldActive = false;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioClip _deathSoundClip;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is NULL");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on Player is NULL.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
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

        if (!_isSpeedActive)
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * (_speed * _speedMultiplier) * Time.deltaTime);
        }


        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x >= 14f)
        {
            transform.position = new Vector3(-14f, transform.position.y, 0);
        }
        else if (transform.position.x <= -14f)
        {
            transform.position = new Vector3(14f, transform.position.y, 0);
        }
    }

    void FirePrimaryLaser()
    {
        _primaryNextFire = Time.time + _primaryFireRate;
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_primaryLaserPrefab, transform.position + _primaryLaserOffset, Quaternion.identity);
        }

        _audioSource.Play();
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;
        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _audioSource.PlayOneShot(_deathSoundClip);
            _spawnManager.OnPlayerDeath();            
            Destroy(gameObject);
        }

        ShowNewDamage();
    }

    public void ActivateTripleShotPowerup()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }


    private IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }
    public void ActivateSpeedPowerup()
    {
        _isSpeedActive = true;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    private IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedActive = false;
    }

    public void ActivateShieldPowerup()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void AddToScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    private void ShowNewDamage()
    {
        if (_lives == 2)
        {
            _damageVisualizer[0].SetActive(true);
        }
        else if (_lives == 1)
        {
            _damageVisualizer[1].SetActive(true);
        }
    }
}