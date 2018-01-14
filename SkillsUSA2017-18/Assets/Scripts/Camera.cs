using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject player;

    private Vector3 offset;
    public float m_DampTime = 0.2f;                 // Approximate time for the camera to refocus.
    public float m_ScreenEdgeBuffer = 4f;           // Space between the top/bottom most target and the screen edge.
    private float m_ZoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
    private Vector3 m_MoveVelocity;                 // Reference velocity for the smooth damping of the position.
    private Vector3 m_DesiredPosition;              // The position the camera is moving towards.

    // Use this for initialization
    void Start ()
    {
        offset = transform.position - player.transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + offset, ref m_MoveVelocity, m_DampTime);
    }
}
