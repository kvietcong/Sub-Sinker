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
            transform.rotation = Quaternion.Slerp(Quaternion.Euler(0,startDir,0), Quaternion.Euler(0,targetDir,0), flipTimer / flipDuration);
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
        flipped = false;
        if (dir == "right")
        {
            if (transform.rotation.eulerAngles.y != 180)
            {
                targetDir = 180;
                startDir = 0;
                flipTimer = transform.rotation.eulerAngles.y / 180;
            }
        }
        else if (dir == "left")
        {
            if (transform.rotation.eulerAngles.y != 0)
            {
                targetDir = 0;
                startDir = 180;
                flipTimer = (180 - transform.rotation.eulerAngles.y) / 180;
            }
        }
        else
        {
            targetDir = Mathf.Abs(targetDir - 180); // flip from current direction
            if (targetDir == 0)
            {
                flipTimer = (180 - transform.rotation.eulerAngles.y) / 180;
            }
            else
            {
                flipTimer = transform.rotation.eulerAngles.y / 180;
            }
                
        }
    }
}
