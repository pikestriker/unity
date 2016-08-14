using UnityEngine;
using System.Collections;
using System.IO;

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


	// Use this for initialization
	void Start () {
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

        while ((curLine = inputFile.ReadLine()) != null)
        {
            string [] bricks = curLine.Split(new char[]{','});
            float curX = (width / 2.0f) - (bricks.Length * width / 2.0f);

            foreach (var curBrick in bricks)
            {
                GameObject newGameObj;

                if (curBrick != "0")
                {
                    newGameObj = (GameObject)Instantiate(brickPrefab, new Vector3(curX, curY, 0.0f), Quaternion.identity);
                    SpriteRenderer sr = newGameObj.GetComponent<SpriteRenderer>();
                    BrickBehaviourScript bbs = newGameObj.GetComponent<BrickBehaviourScript>();
                    
                    switch(curBrick)
                    {
                        case "1":           //brown brick
                            sr.sprite = brownBrick;
                            bbs.brickType = BrickBehaviourScript.BrickType.BROWN_BRICK;
                            break;
                        case "2":           //red brick
                            sr.sprite = redBrick;
                            bbs.brickType = BrickBehaviourScript.BrickType.RED_BRICK;
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
	
	// Update is called once per frame
	void Update () {
	
	}

    public void FixedUpdate()
    {
        if (!startLife)
        {
            BallController ballScript = FindObjectOfType<BallController>();
            if (!ballScript.GetComponent<Renderer>().isVisible)
            {
                startLife = true;
                ballScript.setVelocity(0.0f, 0.0f);
                arrowObject.GetComponent<ArrowControlScript>().resetArrow();
                paddleObject.GetComponent<PaddleController>().resetPaddle();
                numLife--;
                if (numLife < 0)
                {
                    // this would be called to transition the scene to the game over screen and start at level 1 again?
                }
            }
        }
    }
}
