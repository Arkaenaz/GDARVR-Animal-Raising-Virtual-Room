using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class CatBehaviour : StateMachine<CatBehaviour>
{
    [Header("Properties")]
    [SerializeField] private EntityStatField _statField;

    public float movementSpeed = 3;
    public float jumpForce = 300;
    public float timeBeforeNextJump = 1.2f;
    private float canJump = 0f;

    Animator anim;
    Rigidbody rb;

    public Bar _hungerBar;
    public Bar _thirstBar;
    public Bar _moodBar;

    private IdleState _idleState;
    private PatrolState _patrolState;
    private DrinkState _drinkState;
    private PlayState _playState;
    private HungryState _hungryState;
    private EatState _eatState;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        _idleState = new(this);
        _patrolState = new(this);
        _drinkState = new(this);
        _playState = new(this);
        _hungryState = new(this);
        _eatState = new(this);

        SwitchState(_idleState);
    }

    protected override void Update()
    {
        //ControllPlayer();
        Debug.Log("hi");
        CurrentState?.Update();
        _hungerBar.UpdateBar(_statField.maxHunger, _statField.currentHunger);
        _thirstBar.UpdateBar(_statField.maxThirst, _statField.currentThirst);
        _moodBar.UpdateBar(_statField.maxMood, _statField.currentMood);
    }

    void ControllPlayer()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
            anim.SetInteger("Walk", 1);
        }
        else {
            anim.SetInteger("Walk", 0);
        }

        transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);

        if (Input.GetButtonDown("Jump") && Time.time > canJump)
        {
                rb.AddForce(0, jumpForce, 0);
                canJump = Time.time + timeBeforeNextJump;
                anim.SetTrigger("jump");
        }
    }

    public abstract class CatStateBase : StateBase<CatBehaviour>
    {
        protected CatStateBase(CatBehaviour entity) : base(entity) {}
    }

    public class IdleState : CatStateBase {
        public IdleState(CatBehaviour entity) : base(entity) {}

        public override void Enter()
        {
            Debug.Log("Entered Idle State");
        }
    }

    public class PatrolState : CatStateBase
    {
        public PatrolState(CatBehaviour entity) : base(entity) { }
    }

    public class DrinkState : CatStateBase
    {
        public DrinkState(CatBehaviour entity) : base(entity) { }
    }
    public class PlayState : CatStateBase
    {
        public PlayState(CatBehaviour entity) : base(entity) { }
    }

    public class HungryState : CatStateBase
    {
        public HungryState(CatBehaviour entity) : base(entity) { }
    }

    public class EatState : CatStateBase
    {
        public EatState(CatBehaviour entity) : base(entity) { }
    }
}