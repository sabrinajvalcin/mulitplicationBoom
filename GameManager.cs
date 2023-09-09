using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;




public class GameManager : MonoBehaviour
{
    public bool isGameActive;
    public int[] multipliers = {-1,-1};
    public bool gotTwoFactors = false;
    public int missedObjects = 0;

    [SerializeField] private List<GameObject> factors;
    [SerializeField] private string answer;
    [SerializeField] private int score;
    [SerializeField] private Button restartButton;

    private TextMeshProUGUI questionText, answerText, scoreText, correctCardText;
    private GameObject titleScreen, gameOverScreen, questionScreen, numberPad, correctCard;
    private float spawnRate = 1.5f;
    

    void Start()
    {
        titleScreen = GameObject.Find("TitleScreen");
        numberPad = GameObject.Find("NumberPad");
        gameOverScreen = GameObject.Find("GameOverScreen");
        questionScreen = GameObject.Find("QuestionScreen");
        correctCard = GameObject.Find("CorrectCard");
        scoreText =  GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        questionText = GameObject.FindWithTag("qText").GetComponent<TextMeshProUGUI>();
        answerText = GameObject.Find("answerField").GetComponent<TextMeshProUGUI>();
        correctCardText = correctCard.GetComponent<TextMeshProUGUI>();
        gameOverScreen.SetActive(false);
        questionScreen.SetActive(false);
        correctCard.SetActive(false);
    }
    void Update()
    {
        gotTwoFactors = multipliers[1] != -1;
        if(gotTwoFactors)
        {
            isGameActive = false;

            DisplayQuestionPanel();
        }
    }

    IEnumerator SpawnNumber()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);

            //here we put the logic for making an inference (i.e) choosing the factors
            //if a student ansers a question wrong, this is the state we will pass to our greedy 
            //algorithm to see what other questions are usually wrong when they answer that question wrong
            int num = Random.Range(0,13);
            Instantiate(factors[num]);
        }

    }

    public void StartGame()
    {
        isGameActive = true;
        titleScreen.SetActive(false);
        StartCoroutine(SpawnNumber());
    }

    public void GameOver()
    {
        isGameActive = false;
        gameOverScreen.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void DisplayQuestionPanel()
    {
        questionText.text = $"{multipliers[0]} × {multipliers[1]} = ?";
        questionScreen.SetActive(true);
    }



    void UpdateAnswerDisplay(int num)
    {
        if (answerText.text == "___"){
            answerText.text = $"{num}";
            answer = $"{num}";
        } else
        {
            
            answerText.text += $"{num}";
            answer += $"{num}";
        }
    }

    void DeactivateQuestionScreen ()
    {
        questionScreen.SetActive(false);
        //reset factors
        multipliers[0] = -1;
        multipliers[1] = -1;

        //reset answer
        answer = "";
        answerText.text = "___";

        correctCard.SetActive(false);
        numberPad.SetActive(true);

        //resume game
        StartGame();
    }

    void CheckAnswer()
    {
        //check for empot string
        if (answer != "")
        {
            int userInput = int.Parse(answer);
            int correctAns;
            correctAns = multipliers[0] * multipliers[1];
            

            if(userInput == correctAns)
            {
                score += 5;
                scoreText.text = $"Score: {score}";
                DeactivateQuestionScreen();
                
            }

            if(userInput != correctAns)
            {
                
                //deactivate numberPad
                numberPad.SetActive(false);
                
                //display correct answer
                correctCard.SetActive(true);
                correctCardText.text = $"{correctAns}";

                score -=2;
                scoreText.text = $"Score: {score}";
                answerText.text = "";
                
                Invoke(nameof(DeactivateQuestionScreen), 5.0f);
                
            }

        }
        
    }

    public void HandleInput(int buttonValue)
    {
        switch (buttonValue)
        {
            case 100:
                //check answer 
                CheckAnswer();
                break;
            case -100:
                //backspace
                answerText.text = "";
                answer = "";
                break;
            default:
                UpdateAnswerDisplay(buttonValue);
                break;

        }  
    }
   
}
