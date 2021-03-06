using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Game1Manager : MonoBehaviour
{
    public bool isGameStarted = false;
    public bool win = false;
    public Transform outSidePos;
    public DoorScript doorGame1;
    public List<Target> Targets = new List<Target>();
    public List<int> questions = new List<int>();
    public List<int> currentQuestion = new List<int>();
    PlayerController player;


    public GameObject game1Canvas;
    public TextMeshProUGUI XText;
    // public TMP_InputField inputField;
    // public float answer;
    // public int _question;

    public GameObject tutorialUI;
    public GameObject winCanvas;
    public GameObject loseCanvas;

    [Header("Question")]
    public int x = 0;
    public int y = 0;
    public int a = 0;
    public string question;
    public TextMeshProUGUI questionText;

    // [Header("Time")]
    // public float time;
    // float timer;
    // public Image timeImageBar;
   
    [Header("Progress")]
    public int numToAnswer;
    public int remaining = 0;
    public TextMeshProUGUI remainingText;

    [Header("Incorrect")]
    public int maxInCorrectAnswer;
    public int inCorrectAnswer = 0;
    public TextMeshProUGUI inCorrectText;
    // Target[] _targets;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
        // _targets = FindObjectsOfType<Target>();
        // AssignQuestionToAllTarget();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateBar();
        ConvertQuestionToString();
        UpdateQuestionText();
        // TimeCount();
    }

    public void AssignQuestionToTarget(Target _target){
        if(currentQuestion.Count < 1){
            _target.SetQuestion(-1);
            return;
        }

        int random = Random.Range(0,currentQuestion.Count);
        int selectQuestion = currentQuestion[random];
        currentQuestion.RemoveAt(random);
        _target.SetQuestion(selectQuestion);
    }

    //calculate when hit sub target
    public void CalculatorQuestion(float question,float answer){
        float _x = question;
        float _y = ((_x * x) + a) / y; //change this if you want to change question

        if(answer == _y){
            //correct
            // Debug.Log("Correct");
            remaining -= 1;
            // HideCanvas();
        }else{
            //not correct
            // Debug.Log("InCorrect");
            inCorrectAnswer += 1;
            // HideCanvas();
        }

        Progress();
        InCorrect();
    }

    public float FindAnswer(float question){
        float _y = ((question * x) + a) / y;
        return _y;
    }

    void UpdateBar(){
        remainingText.text = $"Remaining : {remaining} / {numToAnswer}";
        inCorrectText.text = $"Wrong : {inCorrectAnswer} / {maxInCorrectAnswer}";
    }

    void Progress(){
        if(remaining == 0){
            Debug.Log("Win");
            win = true;
            //show Text win
            winCanvas.SetActive(true);

            doorGame1.GetComponent<Animator>().enabled = false;
        }
    }

    void InCorrect(){
        if(inCorrectAnswer == maxInCorrectAnswer){
            Debug.Log("GameOver");
            isGameStarted = false;
            loseCanvas.SetActive(true);
        }
    }

    public void StartGame(){
        // timer = time;
        inCorrectAnswer = 0;
        remaining = numToAnswer;

        if(currentQuestion != null){
            currentQuestion.RemoveRange(0,currentQuestion.Count);
        }

        foreach(int n in questions){
            currentQuestion.Add(n);
        }

        game1Canvas.SetActive(true);
        tutorialUI.SetActive(true);
    }

    public void ActiveTargetAndRandomQuestion(){
        //use ActiveTargetAndRandomQuestion() in button in tutorialUI
        foreach(Target n in Targets){
            n.gameObject.SetActive(true);
            n.ShowTarget();
        }

        //And !!!!! Don't forget to random question
    }

    public void EndGame(){
        //warp to outside
        game1Canvas.SetActive(false);
        player.CurrentStage = GameStage.LOBBY;
        player.transform.position = outSidePos.position;

        //Hide all target
        foreach(Target n in Targets){
            n.GetComponent<Target>().HideTargetWhenEndGame();
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && !isGameStarted){
            isGameStarted = true;
            player.CurrentStage = GameStage.GAME1;
            player.ChangeCamera(1);
            StartGame();
        }
    }

    void ConvertQuestionToString(){
        string _x = "",_y = "",_a = "";

        if(x > 0){
            _x = $"{x}x";
        }else if(x < 0){
            _x = $"{x}x";
        }else if(x == 0){
            _x = null;
        }

        if(y > 0){
            _y = $"{y}y";
        }else if(y < 0){
            _y = $"{y}y";
        }else if(y == 0){
            _y = null;
        }

        if(a > 0){
            _a = $"+{a}";
        }else if(a < 0){
            _a = $"{a}";
        }else if(a == 0){
            _a = null;
        }

        question = $"{_y} = {_x}{_a}";
    }

    void UpdateQuestionText(){
        questionText.text = question;
    }
}
