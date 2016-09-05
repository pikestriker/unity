using UnityEngine;
using System.Collections;

public class ArrowControlScript : MonoBehaviour {

    public float rotationSpeed = 1.0f;
    private float zRot = 0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        MainController mainController = FindObjectOfType<MainController>();
        bool startLife = FindObjectOfType<MainController>().startLife;
        if (startLife && !mainController.doingSetup)
        {
            BallController ballScript = FindObjectOfType<BallController>();

            bool right = false;
            bool transform = false;
            float tempRotVel = rotationSpeed;
            if (Input.GetKey("right") && !Input.GetKey("left") && !Input.GetKey("space"))
            {
                tempRotVel = -tempRotVel;
                right = true;
                transform = true;
            }
            else if (Input.GetKey("left") && !Input.GetKey("right") && !Input.GetKey("space"))
            {
                transform = true;
            }
            else if (Input.GetKey("space") && !Input.GetKey("left") && !Input.GetKey("right"))
            {
                FindObjectOfType<MainController>().startLife = false;
                ballScript.setInitialVelocity();
                this.gameObject.SetActive(false);
            }

            if (transform)
            {
                float rotAmount = tempRotVel * Time.deltaTime;
                Transform objTrans = this.gameObject.transform;

                // if the arrow is within the 0 - 80 or the 290 to 360 range or
                // if the arrow is out of that range and the opposite direction is pressed it
                // can still rotate
                if ((zRot >= 0f && zRot <= 80f) ||
                    (zRot >= 290f && zRot <= 360f) ||
                    (zRot <= 290f && zRot >= 270f && !right) ||
                    (zRot >= 80f && zRot <= 100f && right))
                {
                    objTrans.Rotate(new Vector3(0.0f, 0.0f, 1.0f), rotAmount);
                    zRot += rotAmount;
                    if (zRot > 360f)
                        zRot = zRot - 360f;
                    else if (zRot < 0f)
                        zRot = 360f - zRot;
                }
            }

            ballScript.setPosToEndOfArror(this.gameObject);
        }
	}

    public void resetArrow()
    {
        zRot = 0f;
        this.gameObject.SetActive(true);
        this.gameObject.transform.rotation = Quaternion.identity;
        BallController ballScript = FindObjectOfType<BallController>();
        ballScript.setPosToEndOfArror(this.gameObject);
    }
    
}
