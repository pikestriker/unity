using UnityEngine;
using System.Collections;

public class ArrowControlScript : MonoBehaviour {

    public float rotationSpeed = 1.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        bool startLife = FindObjectOfType<MainController>().startLife;
        if (startLife)
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
                if ((objTrans.eulerAngles.z >= 0 && objTrans.eulerAngles.z <= 80) ||
                    (objTrans.eulerAngles.z >= 290 && objTrans.eulerAngles.z <= 360) ||
                    (objTrans.eulerAngles.z <= 290 && objTrans.eulerAngles.z >= 270 && !right) ||
                    (objTrans.eulerAngles.z >= 80 && objTrans.eulerAngles.z <= 100 && right))
                {
                    objTrans.Rotate(new Vector3(0.0f, 0.0f, 1.0f), rotAmount);
                }
            }

            ballScript.setPosToEndOfArror(this.gameObject);
        }
	}

    public void resetArrow()
    {
        this.gameObject.SetActive(true);
        this.gameObject.transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), 0.0f);
    }
    
}
