using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private int _scoreValue = 10;

    private Player _player;
    private Animator _anim;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player is NULL.");
        }

        _anim = gameObject.GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.Log("The animator is NULL.");
        }
    }

    void Update()
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
            _speed = 0;
            _anim.SetTrigger("OnEnemyDeath");
            Destroy(gameObject, 2.8f);
        }
        else if (other.gameObject.CompareTag("Laser"))
        {
            gameObject.GetComponent<PolygonCollider2D>().isTrigger = false;
            if (_player != null)
            {
                _player.AddToScore(_scoreValue); ;
            }
            _speed = 0;
            _anim.SetTrigger("OnEnemyDeath");
            Destroy(other.gameObject);
            Destroy(gameObject, 2.8f);
        }

        
    }
}
