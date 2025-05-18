using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWalker : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float waitTime = 2f;

    public Vector2 minBounds;
    public Vector2 maxBounds;

    public Transform spriteChild; // Assign your child GameObject here (with Animator & Sprite)

    private Vector2 targetPosition;
    private float waitTimer;
    private bool isWaiting;

    private float stuckTimer = 0f;
    private float stuckCheckInterval = 1f;
    private float stuckThreshold = 0.05f;
    private Vector2 lastPosition;

    private Animator animator;

    void Start()
    {
        if (spriteChild != null)
        {
            animator = spriteChild.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("Sprite Child is not assigned!");
        }

        ChooseNewTarget();
        lastPosition = transform.position;
        StartCoroutine(CheckIfStuck());
    }

    void Update()
    {
        bool isMoving = false;

        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                ChooseNewTarget();
            }
        }
        else
        {
            Vector2 prevPos = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            Vector2 movement = (Vector2)transform.position - prevPos;
            if (movement.magnitude > 0.01f)
            {
                isMoving = true;

                // Flip the sprite based on direction (x-axis)
                if (spriteChild != null)
                {
                    Vector3 scale = spriteChild.localScale;
                    scale.x = movement.x != 0 ? -Mathf.Sign(movement.x) * Mathf.Abs(scale.x) : scale.x;
                    animator.speed = isMoving ? 0.5f : 1f;  // 0.5f = half speed, 1f = normal
                    spriteChild.localScale = scale;
                }
            }

            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                isWaiting = true;
                waitTimer = waitTime;
            }
        }

        // Set Animator Walk Bool
        if (animator != null)
        {
            animator.SetBool("1_Move", isMoving);
        }
    }

    void ChooseNewTarget()
    {
        float x = Random.Range(minBounds.x, maxBounds.x);
        float y = Random.Range(minBounds.y, maxBounds.y);
        targetPosition = new Vector2(x, y);
        stuckTimer = 0f;
    }

    IEnumerator CheckIfStuck()
    {
        while (true)
        {
            yield return new WaitForSeconds(stuckCheckInterval);

            float movedDistance = Vector2.Distance(transform.position, lastPosition);
            if (!isWaiting && movedDistance < stuckThreshold)
            {
                stuckTimer += stuckCheckInterval;
                if (stuckTimer >= 2f)
                {
                    ChooseNewTarget();
                }
            }
            else
            {
                stuckTimer = 0f;
            }

            lastPosition = transform.position;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector2 center = (minBounds + maxBounds) / 2;
        Vector2 size = maxBounds - minBounds;
        Gizmos.DrawWireCube(center, size);
    }
}
