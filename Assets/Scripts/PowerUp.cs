using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int powerupID;
    [SerializeField] private AudioClip _powerUpAudio;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_powerUpAudio, transform.position);
            Player player = collision.transform.GetComponent<Player>();
            if (player != null)
            {
                //0 = Triple Shot, 1 = Speed, 2 = Shields
                switch (powerupID)
                {
                    case 0:
                        player.SetTripleShot();
                        break;
                    case 1:
                        player.SetSpeedUp();
                        break;
                    case 2:
                        player.SetShields();
                        break;
                    default:
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
