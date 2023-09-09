using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NumberController : MonoBehaviour
{
    public int value;
    
    [SerializeField] private ParticleSystem fireworks;

    private float minForce = 0;
    private float maxForce = 2;
    private float maxTorque = 5;
    private float xRange = 5;
    private float ySpawnPos = 8;
    private GameManager gameManager;
    private Rigidbody numRb;
    
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        numRb = GetComponent<Rigidbody>();
        numRb.AddForce(RandomForce(), ForceMode.Impulse);
        numRb.AddTorque(0, RandomTorque(), RandomTorque(), ForceMode.Impulse);
        transform.position = RandomSpawnPos();
    }

    void Update()
    {
        if(gameManager.gotTwoFactors || gameManager.isGameActive == false)
        {
            Destroy(gameObject);
        }
    }

    //helper functions
    Vector3 RandomForce()
    {
        return Vector3.down * Random.Range(minForce, maxForce);
    }

    float RandomTorque()
    {
        return Random.Range(0, maxTorque);
    }

    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ground")){
            Destroy(gameObject);
            gameManager.missedObjects += 1;
            if(gameManager.missedObjects >= 5)
            {
                Debug.Log("GameOver");
                gameManager.GameOver();
            }
        }
    }

    void OnMouseDown()
    {
        if (gameManager.multipliers[0] == -1)
        {
            gameManager.multipliers[0] = value;
        } else if (gameManager.multipliers[1] == -1){
            gameManager.multipliers[1] = value;
        }
        Destroy(gameObject);
        PlayFireworksEffect();
    }

    float RandomColorValue()
    {
        return Random.Range(0,0.8f);
    }

    void PlayFireworksEffect ()
    {
        var main = fireworks.main;
        float alpha = 1.0f;
        main.startColor = new Color(RandomColorValue(), RandomColorValue(), RandomColorValue(), alpha);
        Instantiate(fireworks, transform.position, fireworks.transform.rotation);

    }
    //destroy object when lower bound triggered

}
