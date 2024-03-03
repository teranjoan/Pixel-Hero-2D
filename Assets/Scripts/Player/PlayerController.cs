using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player Sprites
    private GameObject standingPlayer;
    private GameObject ballPlayer;

    [Header("Player Movement")] // O tiene que ser publico o serializado
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask selectedLayeredMask;
    private Rigidbody2D rb;
    private Transform checkGroundPoint;
    private bool isGrounded, isFlippedInX;
    // Player Animator
    private Animator standingPlayerAnimator;
    private Animator ballPlayerAnimator;
    private int idSpeed, idIsGrounded, idCanDoubleJump, idShootArrowTrigger;

    [Header("Player Shoot")]
    [SerializeField]
    private ArrowController arrowController;
    private Transform transformArrowPoint, playerTransform;
    private Transform transformBombPoint;
    [SerializeField] private GameObject prefabBomb;

    [Header("Player Dust")]
    [SerializeField]
    private GameObject dustJump;
    private bool isIdle, canDoubleJump;
    private Transform transformDustPoint;

    [Header("Player Dash")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float waitForDash;
    private float dashCounter;
    private float afterDashCounter;

    [Header("Player Dash After Image")]
    [SerializeField] private SpriteRenderer playerSR;
    [SerializeField] private SpriteRenderer afterImageSR;
    [SerializeField] private float afterImageLifeTime;
    [SerializeField] private Color afterImageColor;
    [SerializeField] private float afterImageTimeBetween;
    private float afterImageCounter;

    // BallMode
    private float ballModeCounter;
    [SerializeField] private float waitForBallMode;

    // Player extras
    private PlayerExtraTrackers playerExtraTrackers;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
        playerExtraTrackers = GetComponent<PlayerExtraTrackers>();
    }

    private void Start()
    {
        standingPlayer = GameObject.Find("StandingPlayer");
        ballPlayer = GameObject.Find("BallPlayer");
        ballPlayer.SetActive(false);
        transformDustPoint = GameObject.Find("DustPoint").GetComponent<Transform>();
        transformArrowPoint = GameObject.Find("ArrowPoint").GetComponent<Transform>();
        checkGroundPoint = GameObject.Find("CheckGroundPoint").GetComponent<Transform>();
        transformBombPoint = GameObject.Find("BombPoint").GetComponent<Transform>();
        standingPlayerAnimator = standingPlayer.GetComponent<Animator>();
        ballPlayerAnimator = ballPlayer.GetComponent<Animator>();
        idSpeed = Animator.StringToHash("Speed");
        idIsGrounded = Animator.StringToHash("isGrounded");
        idShootArrowTrigger = Animator.StringToHash("shootArrow");
        idCanDoubleJump = Animator.StringToHash("canDoubleJump");
    }

    void Update()
    {
        DashControl();
        JumpControl();
        CheckAndSetDirection();
        Shoot();
        PlayDust();
        BallMode();
    }

    private void ShowAfterImage()
    {
        SpriteRenderer ai = Instantiate(afterImageSR, playerTransform.position, playerTransform.rotation);
        ai.sprite = playerSR.sprite;
        ai.transform.localScale = playerTransform.localScale;
        ai.color = afterImageColor;
        Destroy(ai.gameObject, afterImageLifeTime);
        afterImageCounter = afterImageTimeBetween;
    }
    private void DashControl()
    {
        if (afterDashCounter > 0)
        {
            afterDashCounter -= Time.deltaTime;
        }
        else
        {
            if ((Input.GetButtonDown("Fire2") && standingPlayer.activeSelf) && playerExtraTrackers.HasHability(HabilityEnum.Dash))
            {
                dashCounter = dashTime;
                ShowAfterImage();
            }
        }
        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            rb.velocity = new Vector2(dashSpeed * playerTransform.localScale.x, rb.velocity.y);
            afterImageCounter -= Time.deltaTime;
            if (afterImageCounter <= 0)
            {
                ShowAfterImage();
            }
            afterDashCounter = waitForDash;
        }
        else
        {
            MoveControl();
        }
    }

    private void Shoot()
    {
        if (Input.GetButtonDown("Fire1") && standingPlayer.activeSelf)
        {
            ArrowController arrowTemp = Instantiate(arrowController, transformArrowPoint.position, transformArrowPoint.rotation);
            if (isFlippedInX)
            {
                arrowTemp.ArrowDirection = new Vector2(-1, 0f);
                arrowTemp.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                arrowTemp.ArrowDirection = new Vector2(1, 0f);
            }
            standingPlayerAnimator.SetTrigger(idShootArrowTrigger);
        }

        if ((Input.GetButtonDown("Fire1") && ballPlayer.activeSelf) && playerExtraTrackers.HasHability(HabilityEnum.DropBomb))
        {
            Instantiate(prefabBomb, transformBombPoint.position, Quaternion.identity);
        }
    }

    void MoveControl()
    {
        float inputX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        rb.velocity = new Vector2(inputX, rb.velocity.y);
        if (standingPlayer.activeSelf)
        {
            standingPlayerAnimator.SetFloat(idSpeed, Mathf.Abs(rb.velocity.x));
        }
        else
        {
            ballPlayerAnimator.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        }
    }

    void JumpControl()
    {
        isGrounded = Physics2D.OverlapCircle(checkGroundPoint.position, 0.2f, selectedLayeredMask);
        if (Input.GetButtonDown("Jump") && (isGrounded || (canDoubleJump && playerExtraTrackers.HasHability(HabilityEnum.DoubleJump)) ))
        {
            if (isGrounded)
            {
                canDoubleJump = true;
                Instantiate(dustJump, transformDustPoint.position, Quaternion.identity);
            }
            else
            {
                canDoubleJump = false;
                standingPlayerAnimator.SetTrigger(idCanDoubleJump);
            }
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        standingPlayerAnimator.SetBool(idIsGrounded, isGrounded);
    }

    private void CheckAndSetDirection()
    {
        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            isFlippedInX = true;
        }
        else if (rb.velocity.x > 0)
        {
            transform.localScale = Vector3.one;
            isFlippedInX = false;
        }
    }

    private void PlayDust()
    {
        if (rb.velocity.x != 0 && isIdle)
        {
            isIdle = false;
            if (isGrounded)
            {
                Instantiate(dustJump, transformDustPoint.position, Quaternion.identity);
            }
        }
        if (rb.velocity.x == 0)
        {
            isIdle = true;
        }
    }

    private void BallMode()
    {
        float inputVertical = Input.GetAxisRaw("Vertical");
        if ((inputVertical < -.9f && !ballPlayer.activeSelf || inputVertical > .9f && ballPlayer.activeSelf) && playerExtraTrackers.HasHability(HabilityEnum.BallMode))
        {
            ballModeCounter -= Time.deltaTime;
            if (ballModeCounter < 0)
            {
                standingPlayer.SetActive(!standingPlayer.activeSelf);
                ballPlayer.SetActive(!ballPlayer.activeSelf);
            }
        }
        else
        {
            ballModeCounter = waitForBallMode;
        }
    }
}
