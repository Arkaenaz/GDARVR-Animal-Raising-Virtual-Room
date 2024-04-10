using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

public class CatBehaviour : StateMachine<CatBehaviour>
{
    [SerializeField] private EntityStatField _statField;

    [Header("AI Properties")]
    [SerializeField]
    public float _wanderRadius = 10.0f;
    [SerializeField]
    public float _wanderTimer = 5.0f;


    public float movementSpeed = 3;
    public float jumpForce = 300;
    public float timeBeforeNextJump = 1.2f;
    private float canJump = 0f;

    Animator _animator;
    Rigidbody rb;

    public Bar _hungerBar;
    public Bar _thirstBar;
    public Bar _moodBar;

    public float hungerFallRate = 10;
    public float thirstFallRate = 20;
    public float moodFallRate = 15;

    private NavMeshAgent _agent;

    private IdleState _idleState;
    private PatrolState _patrolState;
    private DrinkState _drinkState;
    private PlayState _playState;
    private HungryState _hungryState;
    private EatState _eatState;

    void Start()
    {
        _animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        
        _agent = GetComponent<NavMeshAgent>();

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
        // Debug.Log("hi");
        CurrentState?.Update();
        _hungerBar.UpdateBar(_statField.maxHunger, _statField.currentHunger);
        _thirstBar.UpdateBar(_statField.maxThirst, _statField.currentThirst);
        _moodBar.UpdateBar(_statField.maxMood, _statField.currentMood);

        _statField.currentHunger -= Time.deltaTime / hungerFallRate;
        _statField.currentThirst -= Time.deltaTime / thirstFallRate;
        _statField.currentMood -= Time.deltaTime / moodFallRate;

        if (_statField.currentHunger < 0)
            _statField.currentHunger = 0;
        if (_statField.currentThirst < 0)
            _statField.currentThirst = 0;
        if (_statField.currentMood < 0)
            _statField.currentMood = 0;
    }

    void ControllPlayer()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
            _animator.SetInteger("Walk", 1);
        }
        else {
            _animator.SetInteger("Walk", 0);
        }

        transform.Translate(movement * movementSpeed * Time.deltaTime, Space.World);

        if (Input.GetButtonDown("Jump") && Time.time > canJump)
        {
                rb.AddForce(0, jumpForce, 0);
                canJump = Time.time + timeBeforeNextJump;
                _animator.SetTrigger("jump");
        }
    }

    public abstract class CatStateBase : StateBase<CatBehaviour>
    {
        protected CatStateBase(CatBehaviour entity) : base(entity) {}
    }

    public class IdleState : CatStateBase
    {
        private float _wanderTicks;
        public IdleState(CatBehaviour entity) : base(entity) {}
        public override void Enter()
        {
            Debug.Log("Entered Idle State");
            _wanderTicks = 0.0f;
        }

        public override void Update()
        {
            _wanderTicks += Time.deltaTime;

            if (Random.Range(0, 100) <= 100 - Entity._statField.currentHunger)
                Entity.SwitchState(Entity._eatState);
            if (Random.Range(0, 100) <= 100 - Entity._statField.currentThirst)
                Entity.SwitchState(Entity._drinkState);
            if (Random.Range(0, 100) <= 100 - Entity._statField.currentMood)
                Entity.SwitchState(Entity._playState);

            if (_wanderTicks >= Entity._wanderTimer)
            {
                Entity.SwitchState(Entity._patrolState);
                _wanderTicks = 0.0f;
            }
        }
    }

    public class PatrolState : CatStateBase
    {
        public PatrolState(CatBehaviour entity) : base(entity) { }
        public override void Enter()
        {
            Debug.Log("Entered Patrol State");
            Vector3 newPos = NavUtility.RandomNavSphere(Entity.transform.position, Entity._wanderRadius, -1);
            Entity._agent.SetDestination(newPos);
            Entity._animator.SetInteger("Walk", 1);
        }

        public override void Update()
        {
            if (!Entity._agent.pathPending)
            {
                if (Entity._agent.remainingDistance <= Entity._agent.stoppingDistance)
                {
                    if (!Entity._agent.hasPath || Entity._agent.velocity.sqrMagnitude == 0f)
                    {
                        Entity.SwitchState(Entity._idleState);
                    }
                }
            }
        }

        public override void Exit()
        {
            Entity._animator.SetInteger("Walk", 0);
        }
    }

    public class DrinkState : CatStateBase
    {
        public DrinkState(CatBehaviour entity) : base(entity) { }
        public override void Enter()
        {
            Entity._statField.currentThirst = Entity._statField.maxThirst;
        }

        public override void Update()
        {
            Entity.SwitchState(Entity._idleState);
        }
    }
    public class PlayState : CatStateBase
    {
        public PlayState(CatBehaviour entity) : base(entity) { }
        public override void Enter()
        {
            Entity._statField.currentMood = Entity._statField.maxMood;
        }
        public override void Update()
        {
            Entity.SwitchState(Entity._idleState);
        }
    }

    public class HungryState : CatStateBase
    {
        public HungryState(CatBehaviour entity) : base(entity) { }
        
    }

    public class EatState : CatStateBase
    {
        public EatState(CatBehaviour entity) : base(entity) { }
        public override void Enter()
        {
            Entity._statField.currentHunger = Entity._statField.maxHunger;
        }
        public override void Update()
        {
            Entity.SwitchState(Entity._idleState);
        }
    }
}