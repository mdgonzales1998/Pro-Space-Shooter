using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour

{
    //Variables
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _speedMultiplyer = 2;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private GameObject _shieldVisual;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private int _lives = 3;
    [SerializeField] private int _score;
    [SerializeField] private GameObject _leftEngine;
    [SerializeField] private GameObject _rightEngine;
    [SerializeField] private AudioClip _laserAudioClip;
    private AudioSource _audioSource;
    private float _canFire = 0.1f;
    private SpawnManager _spawnManager;
    private bool _isTripleShotEnabled = false;
    private bool _isSpeedUpEnabled = false;
    private bool _isShieldsEnabled = false;
    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0.07f, -2.49f, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        _leftEngine.SetActive(false);
        _rightEngine.SetActive(false);

        if(_spawnManager == null)
        {
            Debug.LogError("PLAYER::Spawn Manager is NULL");
        }

        if (_uiManager == null)
        {
            Debug.LogError("PLAYER::UI Manager is NULL");
        }

        if (_audioSource == null)
        {
            Debug.LogError("PLAYER::Audio Source is NULL");
        } else
        {
            _audioSource.clip = _laserAudioClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Moves player
        calculateMovement();

        //Shoot laser
        shootLaser();

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Inverting isTripleShotEnabled");
            _isTripleShotEnabled = !_isTripleShotEnabled;
        }

    }

    public void Damage()
    {
        if (_isShieldsEnabled)
        {
            _isShieldsEnabled = false; 
            _shieldVisual.SetActive(false);
            return;
        }
        else
        {
            _lives--;
            _uiManager.UpdateLives(_lives);
            if (_lives == 2)
            {
                _rightEngine.SetActive(true);
            }
            else if (_lives == 1)
            {
                _leftEngine.SetActive(true);
            }
        }

        if(_lives <= 0)
        {
            _spawnManager.OnPlayerDeath();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public void SetTripleShot()
    {
        _isTripleShotEnabled = !_isTripleShotEnabled;
        StartCoroutine(DeactivateTripleShot());
    }

    public void SetSpeedUp()
    {
        _speed *= _speedMultiplyer;
        _isSpeedUpEnabled = true;
        StartCoroutine(DeactivateSpeedUp());
    }

    public void SetShields()
    {
        _isShieldsEnabled = true;
        _shieldVisual.SetActive(true);
    }

    IEnumerator DeactivateSpeedUp()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMultiplyer;
        _isSpeedUpEnabled = false;
    }

    IEnumerator DeactivateTripleShot()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotEnabled = false;
    }

    void shootLaser() { 

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            if (!_isTripleShotEnabled)
            {
                _canFire = Time.time + _fireRate;
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.015f, 0), Quaternion.identity);
            }
            else
            {
                _canFire = Time.time + _fireRate;
                Instantiate(_tripleShotPrefab, transform.position + new Vector3(0.64f, 0.062f, 0), Quaternion.identity);
            }
            _audioSource.Play();
        }
    }

    void calculateMovement()
    {
        //Variables to get Unity horizontal and vertical button inputs
        float horizInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");

        //Input variable
        Vector3 direction = new Vector3(horizInput, vertInput, 0);

        //Bounds
        Vector3 rightBound = new Vector3(-11, transform.position.y, 0);
        Vector3 leftBound = new Vector3(11, transform.position.y, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x >= 11)
        {
            transform.position = rightBound;
        }
        else if (transform.position.x <= -11)
        {
            transform.position = leftBound;
        }
    }

    public void updateScore(int newscore)
    {
        _score += newscore;
        _uiManager.UpdateScore(_score);
    }
}
