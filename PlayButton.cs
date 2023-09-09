using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayButton : MonoBehaviour
{
    private GameManager gameManager;
    private Button playButton;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playButton = GetComponent<Button>();
        playButton.onClick.AddListener(gameManager.StartGame);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
