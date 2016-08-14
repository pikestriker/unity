using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

    public float minXVel = 0.1f;
    public float minYVel = 0.1f;
    public float xVel = 4.0f;
    public float yVel = 4.0f;
    public float maxVel = 5.656854f;
    public float minVel = 3.0f;
    private float sinValue;
    private float cosValue;

	// Use this for initialization blah blah blah
	void Start () {
        //print("Start BallController");
        //setVelocity(xVel, yVel);
    }

    //void OnTriggerEnter2D(Collider2D col)
    //{
    //    print("OnTriggerEnter2D BallController");
    //    if (col.gameObject.name == "Top" || col.gameObject.name == "Bottom")
    //        yVel = -yVel;
    //    else if (col.gameObject.name == "Left" || col.gameObject.name == "Right")
    //        xVel = -xVel;

    //    setVelocity(xVel, yVel);
    //}
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Brick")
        {
            print("Hit a brick");
            int retScore = col.gameObject.GetComponent<BrickBehaviourScript>().brickHit();
            FindObjectOfType<MainController>().score += retScore;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        bool startLife = FindObjectOfType<MainController>().startLife;
        if (!startLife)
        {
            Rigidbody2D spriteRigidbody = GetComponent<Rigidbody2D>();
            float curVel = spriteRigidbody.velocity.magnitude;

            if (curVel > maxVel || curVel < minVel)
            {
                //float xRatio = Mathf.Acos(spriteRigidbody.velocity.x / curVel);
                //float yRatio = Mathf.Asin(spriteRigidbody.velocity.y / curVel);
                float xRatio = spriteRigidbody.velocity.x / curVel;
                float yRatio = spriteRigidbody.velocity.y / curVel;

                if (curVel > maxVel)
                {
                    xVel = (maxVel - 0.01f) * xRatio;
                    yVel = (maxVel - 0.01f) * yRatio;
                }
                else if (curVel < minVel)
                {
                    xVel = (minVel + 0.01f) * xRatio;
                    yVel = (maxVel + 0.01f) * yRatio;
                }

                if (xVel >= -0.01f || xVel <= 0.01f)
                {
                    if (xVel < 0.0f)
                        xVel -= minXVel;
                    else
                        xVel += minXVel;
                }

                if (yVel >= -0.01f || yVel <= 0.01f)
                {
                    if (yVel < 0.0f)
                        yVel -= minYVel;
                    else
                        yVel += minYVel;
                }

                setVelocity(xVel, yVel);
            }
        }
    }

    public void setVelocity(float xVel, float yVel)
    {
        Rigidbody2D spriteRigidbody = GetComponent<Rigidbody2D>();

        spriteRigidbody.velocity = new Vector2(xVel, yVel);
    }

    bool isBallOut()
    {
        bool retVal = !this.GetComponent<Renderer>().isVisible;
        if (retVal)
        {
            setVelocity(0.0f, 0.0f);
            FindObjectOfType<MainController>().startLife = true;
        }
        return retVal;
    }

    public void setPosToEndOfArror(GameObject arrow)
    {
        Transform arrowTrans = arrow.GetComponent<Transform>();
        Transform ballTrans = this.gameObject.GetComponent<Transform>();
        ballTrans.position = arrowTrans.position;

        //float cosValue = Mathf.Cos(Mathf.Deg2Rad * arrowTrans.rotation.z);
        //float sinValue = Mathf.Sin(Mathf.Deg2Rad * arrowTrans.rotation.z);
        float add90 = 90 * Mathf.Deg2Rad;
        cosValue = Mathf.Cos(arrowTrans.eulerAngles.z * Mathf.Deg2Rad + add90);
        sinValue = Mathf.Sin(arrowTrans.eulerAngles.z * Mathf.Deg2Rad + add90);

        float xVal = ballTrans.position.x + 1.0f * cosValue;
        float yVal = ballTrans.position.y + 1.0f * sinValue;

        ballTrans.position = new Vector2(xVal, yVal);
    }
    
    public void setInitialVelocity()
    {
        float overallVel = maxVel;      //Mathf.Sqrt(Mathf.Pow(xVel, 2.0f) + Mathf.Pow(yVel, 2.0f));
        xVel = overallVel * cosValue;
        yVel = overallVel * sinValue;

        setVelocity(xVel, yVel);
    }
}
