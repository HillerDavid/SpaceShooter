﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private int _scoreValue = 10;
    private float _fireRate = 3.0f;
    private float _fireCooldown = -1;
    private bool _canFire = true;

    [SerializeField]
    private GameObject _primaryWeapon;

    private Player _player;
    private Animator _deathAnim;

    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _deathSoundClip;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source of Enemy is NULL.");
        }

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player is NULL.");
        }

        _deathAnim = gameObject.GetComponent<Animator>();
        if (_deathAnim == null)
        {
            Debug.Log("The animator is NULL.");
        }
    }

    void Update()
    {
        CalculateMovement();

        if (Time.time > _fireCooldown && _canFire)
        {
            _fireRate = Random.Range(3.0f, 7.0f);
            _fireCooldown = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_primaryWeapon, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void CalculateMovement()
    {
        transform.Translate(new Vector3(0, -1, 0) * _speed * Time.deltaTime);

        if (transform.position.y < -5.5)
        {
            float randomX = Random.Range(-9.5f, 9.5f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            _canFire = false;
            _speed = 0;
            _deathAnim.SetTrigger("OnEnemyDeath");
            _audioSource.PlayOneShot(_deathSoundClip);
            Destroy(gameObject, 2.8f);
        }
        else if (other.gameObject.CompareTag("Laser"))
        {
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            if (_player != null)
            {
                _player.AddToScore(_scoreValue);
            }
            _canFire = false;
            _speed = 0;
            _deathAnim.SetTrigger("OnEnemyDeath");
            Destroy(other.gameObject);
            _audioSource.PlayOneShot(_deathSoundClip);
            Destroy(gameObject, 2.8f);
        }
    }
}
