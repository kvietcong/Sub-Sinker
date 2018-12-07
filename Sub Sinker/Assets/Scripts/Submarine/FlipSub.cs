using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSub : MonoBehaviour {
    bool isFlipping;
    float flipTimer;
    public float flipDuration = 0.6f;

    float startDir;
    float targetDir;

    bool flipped;

	// Use this for initialization
	void Start () {
        targetDir = 0;
        flipped = true;
	}

    // Update is called once per frame
    void Update()
    {
        if (transform.rotation.eulerAngles.y != targetDir)
        {
            float lerpTime = flipTimer / flipDuration;
            if (lerpTime >= 0.5f) {
                transform.rotation = Quaternion.Slerp(Quaternion.Euler(0, 270f, 0), Quaternion.Euler(0, targetDir, 0), (lerpTime - 0.5f) * 2);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(Quaternion.Euler(0, startDir, 0), Quaternion.Euler(0, 270f, 0), lerpTime * 2);
            }
            print("FLIPPING.. progress is " + lerpTime);
        }

        flipTimer += Time.deltaTime;

        if (flipTimer >= flipDuration && flipped == false)
        {
            SendMessageUpwards("FlipCollider", targetDir == 180 ? "right" : "left");
            flipped = true;
        }
    }

    void Flip (string dir)
    {
        if (dir == "right")
        {
            if (transform.rotation.eulerAngles.y != 180)
            {
                float rot = Mathf.Round(transform.rotation.eulerAngles.y);
                if (rot == 0)
                {
                    rot = 360;
                }
                flipped = false;
                targetDir = 180;
                startDir = 0; // 0  is 360
                flipTimer = flipDuration * ((360 - rot) / 180);
                print("timer: " + flipTimer);
                print("from rot: " + rot);
            }
        }
        else if (dir == "left")
        {
            if (transform.rotation.eulerAngles.y != 0)
            {
                float rot = Mathf.Round(transform.rotation.eulerAngles.y);
                flipped = false;
                targetDir = 0; // 0 is 360
                startDir = 180;
                flipTimer = flipDuration * ((rot - 180)  / 180);
                print("timer: " + flipTimer);
                print("from rot: " + rot);
            }
        }
        else
        {
            print("y u do dis");
        }
    }
}
