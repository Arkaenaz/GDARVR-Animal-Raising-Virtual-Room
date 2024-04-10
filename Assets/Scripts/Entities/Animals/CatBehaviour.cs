using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class CatBehaviour : StateMachine<CatBehaviour>
{
    [SerializeField] private EntityStatField _statField;

    [SerializeField] private GameObject _foodBowl;
    [SerializeField] private GameObject _waterDispenser;
    [SerializeField] private GameObject _ballToy;

    [Header("AI Properties")]
    [SerializeField]
    public float _wanderRadius = 0.1f;
    [SerializeField]
    public float _wanderTimer = 5.0f;

    public float BehaviorTimer = 2.0f;

    public float movementSpeed = 3;
    public float jumpForce = 300;
    public float timeBeforeNextJump = 1.2f;
    private float canJump = 0f;

    Animator _animator;
    Rigidbody rb;

    //public UnityEvent<float, float> HungerUpdate;
    //public UnityEvent<float, float> ThirstUpdate;
    //public UnityEvent<float, float> MoodUpdate;


    [SerializeField] private Bar _hungerBar;
    [SerializeField] private Bar _thirstBar;
    [SerializeField] private Bar _moodBar;

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
        //_hungerBar.UpdateBar(_statField.maxHunger, _statField.currentHunger);
        //_thirstBar.UpdateBar(_statField.maxThirst, _statField.currentThirst);
        //_moodBar.UpdateBar(_statField.maxMood, _statField.currentMood);

        PetManager.Instance.HungerUpdate?.Invoke(_statField.maxHunger, _statField.currentHunger);
        PetManager.Instance.ThirstUpdate?.Invoke(_statField.maxThirst, _statField.currentThirst);
        PetManager.Instance.MoodUpdate?.Invoke(_statField.maxMood, _statField.currentMood);

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
        else
        {
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

    void OnEnable()
    {
        PetManager.Instance.HungerUpdate += _hungerBar.UpdateBar;
        PetManager.Instance.ThirstUpdate += _thirstBar.UpdateBar;
        PetManager.Instance.MoodUpdate += _moodBar.UpdateBar;
    }

    void OnDisable()
    {
        PetManager.Instance.HungerUpdate -= _hungerBar.UpdateBar;
        PetManager.Instance.ThirstUpdate -= _thirstBar.UpdateBar;
        PetManager.Instance.MoodUpdate -= _moodBar.UpdateBar;
    }

    public abstract class CatStateBase : StateBase<CatBehaviour>
    {
        protected CatStateBase(CatBehaviour entity) : base(entity) { }
    }

    public class IdleState : CatStateBase
    {
        private float _wanderTicks;
        private float _behaviorTicks;
        public IdleState(CatBehaviour entity) : base(entity) { }
        public override void Enter()
        {
            Debug.Log("Entered Idle State");
            _wanderTicks = 0.0f;
            _behaviorTicks = 0.0f;
        }

        public override void Update()
        {
            _wanderTicks += Time.deltaTime;
            _behaviorTicks += Time.deltaTime;

            if (_behaviorTicks >= Entity.BehaviorTimer)
            {
                switch (Random.Range(0, 3))
                {
                    case 0:
                        if (Random.Range(0, 100) <= 100 - Entity._statField.currentHunger)
                            Entity.SwitchState(Entity._eatState);
                        break;
                    case 1:
                        if (Random.Range(0, 100) <= 100 - Entity._statField.currentThirst)
                            Entity.SwitchState(Entity._drinkState);
                        break;
                    case 2:
                        if (Random.Range(0, 100) <= 100 - Entity._statField.currentMood)
                            Entity.SwitchState(Entity._playState);
                        break;
                }

                _behaviorTicks = 0.0f;
            }


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
            Debug.Log("Entered Drink State");
            
            Entity._agent.SetDestination(Entity._waterDispenser.transform.position);
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
                        Entity._statField.currentThirst = Entity._statField.maxThirst;
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
    public class PlayState : CatStateBase
    {
        public PlayState(CatBehaviour entity) : base(entity) { }
        public override void Enter()
        {
            Debug.Log("Entered Play State");

            Entity._agent.SetDestination(Entity._ballToy.transform.position);
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
                        Entity._statField.currentMood = Entity._statField.maxMood;
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

    public class HungryState : CatStateBase
    {
        public HungryState(CatBehaviour entity) : base(entity) { }

    }

    public class EatState : CatStateBase
    {
        public EatState(CatBehaviour entity) : base(entity) { }
        public override void Enter()
        {
            Debug.Log("Entered Eat State");

            Entity._agent.SetDestination(Entity._foodBowl.transform.position);
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
                        Entity._statField.currentHunger = Entity._statField.maxHunger;
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
}