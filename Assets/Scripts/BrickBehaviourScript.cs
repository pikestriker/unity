using UnityEngine;
using System.Collections;

public class BrickBehaviourScript : MonoBehaviour {

    public enum BrickType {RED_BRICK, BROWN_BRICK, GREY_BRICK };
    public BrickType brickType;
    public Sprite[] breakSprites;
    public int breakLevel = 0;
    public int destroyLevel = 3;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int brickHit()
    {
        //TODO:
        // - Need to destroy the bricks after their destroy level has been exceeded
        // - Need to "load" a level and create the game objects based on a template object
        int retScore = 0;
        int oldBreakLevel = breakLevel;
        switch(brickType)
        {
            case BrickType.RED_BRICK:
                breakLevel++;
                retScore = 200;
                break;
            case BrickType.BROWN_BRICK:
                breakLevel += 2;
                retScore = 100;
                break;
            default:
                print("Indestructable");
                break;
        }

        if (oldBreakLevel != breakLevel)
        {
            if (breakLevel > destroyLevel)
            {
                Destroy(this.gameObject);
                print("Destroy the brick");
            }
            else
            {
                Transform[] childTransforms = GetComponentsInChildren<Transform>(true);
                GameObject childGameObject;
                foreach (Transform t in childTransforms)
                {
                    childGameObject = t.gameObject;

                    if (t.name == "BreakArtwork")
                    {
                        t.GetComponent<SpriteRenderer>().sprite = breakSprites[breakLevel];
                        break;
                    }
                }
            }
        }

        return retScore;
    }
}
