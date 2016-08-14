using UnityEngine;
using System.Collections;

public class PaddleController : MonoBehaviour {

    public float velocity = 2.0f;
    public GameObject rightWall;
    public GameObject leftWall;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        bool startLife = FindObjectOfType<MainController>().startLife; ;
        if (!startLife)
        {
            if (Input.GetKey("right") && !Input.GetKey("left"))
                setVelocity(true);
            else if (Input.GetKey("left") && !Input.GetKey("right"))
                setVelocity(false);
            else
            {
                Rigidbody2D paddleRigidBody = GetComponent<Rigidbody2D>();
                if (paddleRigidBody.velocity.x != 0.0f)
                    paddleRigidBody.velocity = new Vector2(0.0f, 0.0f);
            }
        }
    }

    void setVelocity(bool right)
    {
        Rigidbody2D paddleRigidBody = GetComponent<Rigidbody2D>();

        if (!paddleAtEnd(right))
        {
            
            if (right)
            {
                if (paddleRigidBody.velocity.x <= 0)        //going in the other direction or stopped
                    paddleRigidBody.velocity = new Vector2(velocity, 0.0f);
            }
            else
            {
                if (paddleRigidBody.velocity.x >= 0)
                    paddleRigidBody.velocity = new Vector2(-velocity, 0.0f);

            }
        }
        else
        {
            if (paddleRigidBody.velocity.x != 0.0f)
            {
                paddleRigidBody.velocity = new Vector2(0.0f, 0.0f);
            }
        }
    }

    bool paddleAtEnd(bool right)
    {
        CircleCollider2D [] colliders = GetComponentsInChildren<CircleCollider2D>(true);

        CircleCollider2D endPaddleCollider = null;

        //find the correct collider
        foreach (CircleCollider2D curCollider in colliders)
        {
            if (right && curCollider.name == "RightBall")
            {
                endPaddleCollider = curCollider;
                break;
            }
            else if (!right && curCollider.name == "LeftBall")
            {
                endPaddleCollider = curCollider;
                break;
            }
        }

        if (endPaddleCollider == null)
            return true;

        if (right)
        {
            BoxCollider2D rightCollider = rightWall.GetComponent<BoxCollider2D>();
            if (endPaddleCollider.bounds.max.x > rightCollider.bounds.min.x - 0.10f)
                return true;
        }
        else
        {
            BoxCollider2D leftCollider = leftWall.GetComponent<BoxCollider2D>();
            if (endPaddleCollider.bounds.min.x < leftCollider.bounds.max.x + 0.10f)
                return true;
        }

        return false;
    }

    public void resetPaddle()
    {
        this.GetComponent<Transform>().position = new Vector2(0.0f, this.GetComponent<Transform>().position.y);
    }
}
