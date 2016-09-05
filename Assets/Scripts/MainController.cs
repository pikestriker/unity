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
    public int level = 1;
    public bool startLife = true;
    public int numLife = 3;
    public int score = 0;
    public bool doingSetup = true;
    public int numDestroyableBricks;
    public static int MAX_NUM_LEVELS = 1;

    private GameObject levelImage;
    private Text levelText;
    private float levelStartDelay = 2f;

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
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        lifeText = GameObject.Find("LifeText").GetComponent<Text>();
        numDestroyableBricks = 0;

        levelText.text = "Level " + level;

        levelImage.SetActive(true);

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
                        case "1":           //brown brick
                            sr.sprite = brownBrick;
                            bbs.brickType = BrickBehaviourScript.BrickType.BROWN_BRICK;
                            numDestroyableBricks++;
                            break;
                        case "2":           //red brick
                            sr.sprite = redBrick;
                            bbs.brickType = BrickBehaviourScript.BrickType.RED_BRICK;
                            numDestroyableBricks++;
                            break;
                        case "3":           //grey brick
                            sr.sprite = greyBrick;
                            bbs.brickType = BrickBehaviourScript.BrickType.GREY_BRICK;
                            break;
                    }
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
        if (!startLife && !doingSetup)
        {
            scoreText.text = "Score:  " + score;
            BallController ballScript = FindObjectOfType<BallController>();

            if (numDestroyableBricks <= 0)
            {
                resetLife();
                level++;
                if (level >= MAX_NUM_LEVELS)
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
                    resetGame("You suck!  Game Over");
                    // this would be called to transition the scene to the game over screen and start at level 1 again?
                }
                lifeText.text = "Lives:  " + numLife;
            }
        }
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
