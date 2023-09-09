using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InputController : MonoBehaviour
{
    [SerializeField] int value;
    private GameManager gameManager;
    private Button button;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        button = gameObject.GetComponent<Button>();

        //send value to game manager on click
        button.onClick.AddListener(() => gameManager.HandleInput(value));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
