using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    protected Vector2 m_Velocity;
    protected Vector2 m_LastFeetPosition;

    [SerializeField] private float m_Speed = 5;
    [SerializeField] private Transform feet; // Reference to the feet GameObject
    [SerializeField] private Animator m_Animator; // Reference to the Animator
    private Vector2? m_Destination;
    private Vector2? Destination => m_Destination;

    public float CurrentSpeed => m_Velocity.magnitude;
    private bool IsMoving;

    void Start()
    {
        if (feet == null)
        {
            Debug.LogError("Feet GameObject is not assigned!");
            return;
        }

        m_LastFeetPosition = feet.position;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (feet == null) return;

        if (m_Destination.HasValue)
        {
            var dir = m_Destination.Value - (Vector2)feet.position;
            transform.position += (Vector3)(dir.normalized * Time.deltaTime * m_Speed);

            var distanceToDestination = Vector2.Distance(feet.position, m_Destination.Value);

            if (distanceToDestination < 0.1f)
            {
                m_Destination = null;
            }
        }

        m_Velocity = (new Vector2(
            feet.position.x - m_LastFeetPosition.x,
            feet.position.y - m_LastFeetPosition.y
        )) / Time.deltaTime;

        m_LastFeetPosition = feet.position;
        IsMoving = m_Velocity.magnitude > 0;

        // Update the animator with normalized velocity values
        if (m_Animator != null)
        {
            Vector2 normalizedVelocity = m_Velocity.normalized; // Ensure values are -1 to 1
            m_Animator.SetFloat("X", normalizedVelocity.x);
            m_Animator.SetFloat("Y", normalizedVelocity.y);

            // Optionally, set a boolean to indicate whether the player is moving
            m_Animator.SetBool("isMoving", IsMoving);
        }

        Debug.Log($"Feet Position: {feet.position}, Velocity: {m_Velocity}");
    }

    public void MoveTo(Vector2 destination)
    {
        SetDestination(destination);
    }

    public void SetDestination(Vector2 destination)
    {
        m_Destination = destination;
    }
}
