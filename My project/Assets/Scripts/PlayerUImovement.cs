using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerUImovement : MonoBehaviour
{
   [Header("Movement Settings")]
    public float moveSpeed = 5f;
    private Vector2 targetPosition;
    private bool isMoving = false;

    [Header("Sprites")]
    public Sprite idleSprite;  // Standing still
    public Sprite leftSprite;  // Facing/Moving Left
    public Sprite rightSprite; // Facing/Moving Right

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 1. UI CHECK
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return; 
        }

        // 2. INPUT
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Mathf.Abs(Camera.main.transform.position.z);
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
            
            targetPosition = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
            isMoving = true;
        }

        // 3. SPRITE SWAP LOGIC
        UpdateCharacterSprite();
    }

    void UpdateCharacterSprite()
    {
        if (isMoving)
        {
            // Compare target x to current x to decide which sprite to show
            if (targetPosition.x < transform.position.x)
            {
                spriteRenderer.sprite = leftSprite;
            }
            else if (targetPosition.x > transform.position.x)
            {
                spriteRenderer.sprite = rightSprite;
            }
        }
        else
        {
            // If not moving, show the idle sprite
            spriteRenderer.sprite = idleSprite;
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            Vector2 currentPos = rb.position;
            Vector2 nextStep = Vector2.MoveTowards(currentPos, targetPosition, moveSpeed * Time.fixedDeltaTime);
            
            rb.MovePosition(nextStep);

            // Arrival check
            if (Vector2.Distance(currentPos, targetPosition) < 0.1f)
            {
                isMoving = false;
            }
        }
    }

}

