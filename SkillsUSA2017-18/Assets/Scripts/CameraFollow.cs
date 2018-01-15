using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    GameObject player;

    private Vector3 offset;
    public float m_DampTime = 0.2f;                 // Approximate time for the camera to refocus.
    public float m_ScreenEdgeBuffer = 4f;           // Space between the top/bottom most target and the screen edge.
    private float m_ZoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
    private Vector3 m_MoveVelocity;                 // Reference velocity for the smooth damping of the position.
    private Vector3 m_DesiredPosition;              // The position the camera is moving towards.

    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (player == null)
        {
            return;
        }

        transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + offset, ref m_MoveVelocity, m_DampTime);
    }

    public void SetPlayer(GameObject thePlayer)
    {
        player = thePlayer;
        transform.position = new Vector3(player.transform.position.x,
            player.transform.position.y,
            transform.position.z);
        offset = transform.position - player.transform.position;
    }
}
