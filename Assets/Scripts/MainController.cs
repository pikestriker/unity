using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class MainController : MonoBehaviour {

    public GameObject brickPrefab;
    public GameObject arrowObject;
    public GameObject ballObject;
    public GameObject paddleObject;
    public Sprite redBrick;
    public Sprite brownBrick;
    public Sprite greyBrick;
    public int level = 5;
    public bool startLife = true;
    public int numLife = 3;
    public int score = 0;
    public bool doingSetup = true;
    public int numDestroyableBricks;
    public bool paused = false;
    public static int MAX_NUM_LEVELS = 5;

    private GameObject levelImage;
    private Text levelText;
    private float levelStartDelay = 2f;
    private float pauseButtonDelay = 0.5f;
    private bool pauseDelay = false;
    private string pauseButton = "p";

    private Text scoreText;
    private Text lifeText;

    private List<GameObject> brickList = new List<GameObject>();

	// Use this for initialization
	void Start () {
        loadLevel();
	}

    private void loadLevel()
    {
        doingSetup = true;

        if (levelImage == null)
            levelImage = GameObject.Find("LevelImage");

        levelImage.SetActive(true);

        if (levelText == null)
            levelText = GameObject.Find("LevelText").GetComponent<Text>();

        if (scoreText == null)
            scoreText = GameObject.Find("ScoreText").GetComponent<Text>();

        if (lifeText == null)
            lifeText = GameObject.Find("LifeText").GetComponent<Text>();

        numDestroyableBricks = 0;

        levelText.text = "Level " + level;

        Invoke("deactivateLevelImage", levelStartDelay);

        SpriteRenderer tempRender = brickPrefab.GetComponent<SpriteRenderer>();
        float width = tempRender.bounds.size.x;
        float height = tempRender.bounds.size.y;
        print(width);
        print(height);

        float screenHeight = Camera.main.orthographicSize * 2.0f;
        float screenWidth = screenHeight * Screen.width / Screen.height;
        float curY = screenHeight / 2.0f - height / 2.0f;
        string levelString = string.Format("{0}/Levels/level{1}.txt", Application.dataPath, this.level);
        StreamReader inputFile = new StreamReader(levelString);

        string curLine = "";

        foreach (var curBrick in brickList)
        {
            if (curBrick != null)
                Destroy(curBrick);
        }

        brickList.Clear();

        while ((curLine = inputFile.ReadLine()) != null)
        {
            string[] bricks = curLine.Split(new char[] { ',' });
            float curX = (width / 2.0f) - (bricks.Length * width / 2.0f);

            foreach (var curBrick in bricks)
            {
                GameObject newGameObj;

                if (curBrick != "0")
                {
                    newGameObj = (GameObject)Instantiate(brickPrefab, new Vector3(curX, curY, 0.0f), Quaternion.identity);
                    SpriteRenderer sr = newGameObj.GetComponent<SpriteRenderer>();
                    BrickBehaviourScript bbs = newGameObj.GetComponent<BrickBehaviourScript>();
                    bbs.breakLevel = 0;
                    brickList.Add(newGameObj);

                    switch (curBrick)
                    {
                        case "1":           //broken brown brick
                            sr.sprite = brownBrick;
                            bbs.brickType = BrickBehaviourScript.BrickType.BROWN_BRICK;
                            bbs.breakLevel = 2;
                            numDestroyableBricks++;
                            break;
                        case "2":           //full brown brick
                            sr.sprite = brownBrick;
                            bbs.brickType = BrickBehaviourScript.BrickType.BROWN_BRICK;
                            bbs.breakLevel = 0;
                            numDestroyableBricks++;
                            break;
                        case "3":           //broken red brick
                            sr.sprite = redBrick;
                            bbs.breakLevel = 2;
                            bbs.brickType = BrickBehaviourScript.BrickType.RED_BRICK;
                            numDestroyableBricks++;
                            break;
                        case "4":           //less broken red brick
                            sr.sprite = redBrick;
                            bbs.breakLevel = 1;
                            bbs.brickType = BrickBehaviourScript.BrickType.RED_BRICK;
                            numDestroyableBricks++;
                            break;
                        case "5":           //full broken red brick
                            sr.sprite = redBrick;
                            bbs.breakLevel = 0;
                            bbs.brickType = BrickBehaviourScript.BrickType.RED_BRICK;
                            numDestroyableBricks++;
                            break;
                        case "6":           //grey brick
                            sr.sprite = greyBrick;
                            bbs.breakLevel = 0;
                            bbs.brickType = BrickBehaviourScript.BrickType.GREY_BRICK;
                            break;
                    }
                    bbs.setBreakLevel();
                }
                curX += width;
            }
            curY -= height;
        }

        inputFile.Close();
    }

    private void deactivateLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void FixedUpdate()
    {
        if (Input.GetKey(pauseButton.ToLower()) && !pauseDelay)
        {
            paused = !paused;
            pauseDelay = true;
            Invoke("setPauseDelayToFalse", pauseButtonDelay);
            // set the paused text on the screen
        }

        //if (!startLife)
        //    print("startLife is false");
        //else
        //    print("startLife is true");

        //if (!doingSetup)
        //    print("doingSetup is false");
        //else
        //    print("doingSetup is true");

        //if (!paused)
        //    print("paused is false");
        //else
        //    print("paused is true");
        if (!startLife && !doingSetup && !paused)
        {
            scoreText.text = "Score:  " + score;
            BallController ballScript = FindObjectOfType<BallController>();

            if (numDestroyableBricks <= 0)
            {
                resetLife();
                level++;
                if (level > MAX_NUM_LEVELS)
                    resetGame("You reached Max Levels " + MAX_NUM_LEVELS + " resetting to level 1");
                else
                    loadLevel();
            }

            if (!ballScript.GetComponent<Renderer>().isVisible)
            {
                resetLife();
                numLife--;
                if (numLife < 0)
                {
                    numLife = 3;
                    score = 0;
                    resetGame("You suck!  Game Over");
                    // this would be called to transition the scene to the game over screen and start at level 1 again?
                }
                lifeText.text = "Lives:  " + numLife;
            }
        }
    }

    private void setPauseDelayToFalse()
    {
        pauseDelay = false;
    }

    private void resetGame(string textToDisplay)
    {
        doingSetup = true;
        levelImage.SetActive(true);
        level = 1;
        levelText.text = textToDisplay;

        Invoke("loadLevel", levelStartDelay);
    }

    private void resetLife()
    {
        startLife = true;
        BallController ballScript = FindObjectOfType<BallController>();
        ballScript.setVelocity(0f, 0f);
        arrowObject.GetComponent<ArrowControlScript>().resetArrow();
        paddleObject.GetComponent<PaddleController>().resetPaddle();
    }
}
