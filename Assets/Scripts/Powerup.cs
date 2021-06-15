using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField] /* 0 = Triple Shot | 1 = Speed | 2 = Shield */
    private int _powerupID;

    [SerializeField]
    private AudioClip _clip;
    private float _clipVolume = 10f;

    void Update()
    {
        if (transform.position.y < -5.75f)
        {
            Destroy(gameObject);
        }
        Vector3 direction = new Vector3(0, -1, 0);
        transform.Translate(direction * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(_clip, transform.position, _clipVolume);
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.ActivateTripleShotPowerup();
                        break;
                    case 1:
                        player.ActivateSpeedPowerup();
                        break;
                    case 2:
                        player.ActivateShieldPowerup();
                        break;
                    default:
                        break;
                }
            }

            Destroy(gameObject);
        }
    }
}
