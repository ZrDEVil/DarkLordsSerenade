using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private bool isFacingRight;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float pauseTime;
    [SerializeField] private bool stopped = false;
    [SerializeField] private float[] pointPositions;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private int currentPointIndex = 0;
    private float currentMoveSpeed;
    private Vector2 startingPosition;
    private bool isFacingRightStart;
    private bool killed = false;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        startingPosition = new Vector2(transform.position.x,transform.position.y);
        isFacingRightStart = isFacingRight;

        InitEdgePositions();
        currentMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    { 
        EnemyMovement();
        CheckEdgePatrol();
        GetAnimations();
    }

    private void EnemyMovement() {
        if(isFacingRight) {
            rb.velocity = new Vector2(currentMoveSpeed, 0);
        } else {
            rb.velocity = new Vector2(-currentMoveSpeed, 0);
        }
    }

    private void CheckEdgePatrol() {
        if(!stopped) {
            if((!isFacingRight && transform.position.x <= pointPositions[currentPointIndex]) ||
                (isFacingRight && transform.position.x >= pointPositions[currentPointIndex])) {
                StopMovement();
            }
        }
    }
    private void FlipSprite() {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    private void FlipPointDirection() {
        if(currentPointIndex == 0) {
            currentPointIndex = 1;
            isFacingRight = true;
        } else {
            currentPointIndex = 0;
            isFacingRight = false;
        }
    }

    private IEnumerator StopMovementCoroutine() {
        stopped = true;

        currentMoveSpeed = 0;
        yield return new WaitForSeconds(pauseTime);
        FlipSprite();
        FlipPointDirection();

        currentMoveSpeed = moveSpeed;

        stopped = false;

    }

    private void StopMovement() {
        StartCoroutine(StopMovementCoroutine());
    }

    private void InitEdgePositions() {
        pointPositions = new float[2];
        Transform points = transform.Find("Points");

        pointPositions[0] = points.GetChild(0).position.x;
        pointPositions[1] = points.GetChild(1).position.x;

        points.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        points.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player") {
            DamagePlayer();
        }
    }

    private void DamagePlayer() {
        if(!killed) {
            ResetScene.InstanciaResetScene.Reset();
        }
    }

    public void Restart() {
        transform.position = startingPosition;
        isFacingRight = isFacingRightStart;
        stopped = false;
        killed = false;
    }

    public void Kill() {
        killed = true;
    }

    private void GetAnimations()
    {
        animator.SetFloat("Speed", currentMoveSpeed);
    }

}
