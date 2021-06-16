using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour

{
    [SerializeField] private float _speed = 4.0f;
    private Player _player;
    private Animator _animator;
    [SerializeField] AudioClip _explosionAudioClip;
    private AudioSource _audioSource;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        
        if (_player == null)
        {
            Debug.LogError("ENEMEY::Player is null");
        }

        if (_animator == null)
        {
            Debug.LogError("ENEMY::Animator is null");
        }

        if (_audioSource == null)
        {
            Debug.LogError("ENEMY::Audio Source is null");
        }
        else
        {
            _audioSource.clip = _explosionAudioClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //move down at 4 m/s
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        
        //if bottom of screen, respawn back at top with a new random x position
        if (transform.position.y >= 8)
        {
            float randomX = Random.Range(-9.5f, 9.5f);
            transform.position = new Vector3(randomX, -8, 0);
        }
        else if (transform.position.y <= -8)
        {
            float randomX = Random.Range(-9.5f, 9.5f);
            transform.position = new Vector3(randomX, 8, 0);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _speed = 0;
            Destroy(this.gameObject, 2.8f);
        }
        else if (other.tag == "Laser")
        {
            Destroy(other);
            if (_player != null)
            {
                _player.updateScore(10);
            }
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _speed = 0;
            Destroy(this.gameObject, 2.8f);
        }

    }
}
