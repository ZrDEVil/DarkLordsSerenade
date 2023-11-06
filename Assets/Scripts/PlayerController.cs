using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: MonoBehaviour
{
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float horizontalMove = 0f;
    [SerializeField] private float verticalMove = 0f;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool isRunning = false;
    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Checkpoint checkPoint;
    [SerializeField] private int coins = 0;

    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private bool gamePaused;

    [SerializeField] public static GameObject instanciaPlayerController;
    private static PlayerController _instanciaPlayerController;
    public static PlayerController InstanciaPlayerController {
        get {
            if(_instanciaPlayerController == null) {
                _instanciaPlayerController = instanciaPlayerController.GetComponent<PlayerController>();
            }
            return _instanciaPlayerController;
        }
    }

    private void Awake() {
        instanciaPlayerController = FindObjectOfType<PlayerController>().gameObject;
    }

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gamePaused = false;
    }   

    // Update is called once per frame
    void Update() {
        GetGamePaused();
        
        if (!gamePaused)
        {
            IsGroundedCheck();
            GetHorizontalVerticalMove();
            CharacterMoveHorizontal();
            CharacterMoveVertical();
            GetSprint();
            FlipSprite();
            GetAnimations();
        }
    }

    void GetHorizontalVerticalMove() {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
    }

    void FlipSprite() {
        if((isFacingRight && horizontalMove < 0.0f) || (!isFacingRight && horizontalMove > 0.0f)) {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    void CharacterMoveHorizontal() {
        rb.velocity = new Vector2(horizontalMove * runSpeed, rb.velocity.y);
        if (isRunning)
        {
            rb.velocity = new Vector2(rb.velocity.x * 1.6f, rb.velocity.y);
        }
    }

    void CharacterMoveVertical() {
        //Pulo segurando o botão
        if(Input.GetButtonDown("Jump") && isGrounded) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        //O player já está pulando mas soltou o botão
        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0.0f) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

    }

    private bool IsGroundedCheck() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        return isGrounded;
    }

    private void GetSprint() {
        if(Input.GetButton("Run")) {
            isRunning = true;
        } else {
            isRunning = false;
        }
    }

    public void AddCoin() {
        coins++;
    }

    public void RestartCheckpoint() {
        transform.position = checkPoint.Position();

        //Resetar a gravidade do player.
        rb.velocity = new Vector2(0, 0);
    }

    public void RestartCoins() {
        coins = 0;
    }

    public void SetCheckpoint(Checkpoint newCheckpoint) {
        checkPoint = newCheckpoint;
    }

    private void GetAnimations()
    {
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        animator.SetBool("OnGround", isGrounded);
        animator.SetBool("Running", isRunning);
        animator.SetBool("Paused", gamePaused);
    }

    private void GetGamePaused()
    {
        if (Time.timeScale > 0)
        {
            gamePaused = false;
        }
        else
        {
            gamePaused = true;
        }
    }
}

