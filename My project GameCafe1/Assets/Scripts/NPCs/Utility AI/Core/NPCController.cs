using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TL.UtilityAI;
using TL.UtilityAI.Actions;
using UnityEngine.AI;

namespace TL.Core
{
    public enum State
    {
        decide,
        move,
        execute
    }

    public enum NPCType
    {
        Worker,
        Walker,
        Troublemaker,
        Citizen
    }

    public class NPCController : MonoBehaviour
    {
        public MoveController mover { get; set; }
        public AIBrain aiBrain { get; set; }
        public NPCStats stats { get; set; }
        [field: SerializeField] public NPCType type { get; set; }

        public Context context;

        private State currentState;
        private NPCVisualController visualController;
        private NPCAnimationEvents animationEvents;
        private FoodOrderSystem foodOrderSystem;
        private CustomerSatisfaction customerSatisfaction;

        public State GetCurrentState() { return currentState; }
        public void SetCurrentState(State state)
        {
            currentState = state;

            if (currentState == State.execute)
            {

                Animator animator = GetComponentInChildren<Animator>();
                if (animator != null)
                {
                    animator.SetBool("IsWalking", false);
                }

                aiBrain.bestAction.Execute(this);

                if (aiBrain.bestAction is Playe)
                {
                    CafeManager.instance.OnIsOpenChanage -= ChangeMindForPlay;
                    Transform seat = mover.destination;
                    transform.position = seat.position;
                    transform.rotation = seat.rotation;
                    GetComponent<NavMeshAgent>().enabled = false;
                }

            }
            else if (currentState == State.move)
            {
                GetComponentInChildren<Animator>().SetBool("IsWalking", true);
                aiBrain.bestAction.SetRequiredDestination(this);
            }
        }


        private IBuyableRespondable currentDevice = null;
        public Transform currentDestination;

        public TroublemakerNPC troublemaker { get; private set; }

        private DeviceScreen deviceScreen;
        // Start is called before the first frame update
        void Start()
        {
            mover = GetComponent<MoveController>();
            aiBrain = GetComponent<AIBrain>();
            stats = GetComponent<NPCStats>();
            visualController = GetComponent<NPCVisualController>();
            animationEvents = GetComponent<NPCAnimationEvents>();
            foodOrderSystem = GetComponent<FoodOrderSystem>();
            customerSatisfaction = GetComponent<CustomerSatisfaction>();

            SetupAnimationEventListeners();

            if (type == NPCType.Troublemaker)
                troublemaker = GetComponent<TroublemakerNPC>();
        }
        // Update is called once per frame
        void Update()
        {
            FSMTick();

            stats.UpdateEnergy(AmIAtRestDestination());
            stats.UpdateHunger();
        }

        public void FSMTick()
        {
            if (GetCurrentState() == State.decide)
            {
                aiBrain.previouBsestAction = aiBrain.bestAction;
                aiBrain.DecideBestAction();

                if (aiBrain.bestAction is Playe)
                {
                    CafeManager.instance.OnIsOpenChanage += ChangeMindForPlay;
                    Faint.Instance.onFaint += ResetMindForPlay;
                    Bed.Instance.onSleep += ResetMindForPlay;
                }
                else
                {
                    CafeManager.instance.OnIsOpenChanage -= ChangeMindForPlay;
                    Faint.Instance.onFaint -= ResetMindForPlay;
                    Bed.Instance.onSleep -= ResetMindForPlay;
                }

                //Don't play twice
                if (aiBrain.previouBsestAction == aiBrain.bestAction && aiBrain.bestAction is Playe)
                {
                    if (type == NPCType.Walker)
                    {
                        aiBrain.bestAction = new Walk();
                        aiBrain.bestAction.SetRequiredDestination(this);
                        SetCurrentState(State.move);
                        return;
                    }

                    stats.energy = Random.Range(20, 40);
                    stats.money = Random.Range(100, 300);
                    stats.hunger = Random.Range(50, 80);
                    return;
                }

                SetCurrentState(State.move);
            }
            else if (GetCurrentState() == State.move)
            {

                float distance = Vector3.Distance(mover.destination.position, this.transform.position);
                if (distance < 1f)
                {
                    SetCurrentState(State.execute);
                }
            }
            else if (GetCurrentState() == State.execute)
            {
                if (aiBrain.finishedExecutingBestAction == true)
                {
                    Debug.Log("Exit execute state");
                    if (aiBrain.bestAction is Playe)
                    {
                        Debug.Log($"Releasing device after execution for NPC {gameObject.name}");
                        CafeManager.instance.LeaveItem(currentDevice);
                        CafeManager.instance.AddMoney((Device)currentDevice);
                        currentDevice = null;
                    }
                    SetCurrentState(State.decide);
                }
            }
        }

        private void SetupAnimationEventListeners()
        {
            if (animationEvents != null)
            {
                animationEvents.animationEvents.onStartWalking.AddListener(() =>
                {
                    visualController.SetWalkingState(true);
                });

                animationEvents.animationEvents.onStopWalking.AddListener(() =>
                {
                    visualController.SetWalkingState(false);
                });
            }
        }

        private void ChangeMindForPlay(bool cafeIsOpen)
        {
            if (!cafeIsOpen)
            {
                SetCurrentState(State.decide);
            }
        }

        private void ResetMindForPlay()
        {
            visualController.AdjustIKWeight(0);
            visualController.SetSittingState(false);
            animationEvents.TriggerStopSitting();
            deviceScreen?.StopVideo();
            SetCurrentState(State.decide);
            aiBrain.finishedExecutingBestAction = true;
        }

        #region Workhorse methods

        public void OnFinishedAction()
        {
            aiBrain.DecideBestAction();
        }

        public bool AmIAtRestDestination()
        {
            return Vector3.Distance(this.transform.position, context.home.transform.position) <= context.MinDistance;
        }

        public bool TheCafeHasAvalibleSeats()
        {
            return context.currentCafe < context.maxCafe;
        }

        #endregion

        #region Coroutine

        public void DoWork(int time)
        {
            StartCoroutine(WorkCoroutine(time));
        }

        public void DoSleep(int time)
        {
            StartCoroutine(SleepCoroutine(time));
        }

        public void DoPlay(int time)
        {
            StartCoroutine(PlayCoroutine(time));
        }

        public void DoEat(int time)
        {
            StartCoroutine(EatCoroutine(time));
        }

        IEnumerator WorkCoroutine(int time)
        {
            int counter = time;
            while (counter > 0)
            {
                yield return new WaitForSeconds(1);
                counter--;
            }

            Debug.Log("I AM WORKING!");
            // Logic to update things involved with work
            stats.money += 10f;


            // Decide our new best action after you finished this one
            //OnFinishedAction();
            aiBrain.finishedExecutingBestAction = true;
            yield break;
        }

        IEnumerator SleepCoroutine(int time)
        {
            int counter = time;
            while (counter > 0)
            {
                yield return new WaitForSeconds(1);
                counter--;
            }

            Debug.Log("I slept and gained 1 energy!");
            // Logic to update energy
            stats.energy += 10f;
            stats.cafe += 10f;

            // Decide our new best action after you finished this one
            //OnFinishedAction();
            aiBrain.finishedExecutingBestAction = true;
            yield break;
        }


        IEnumerator PlayCoroutine(int time)
        {
            currentDevice = mover.destination.GetComponentInParent<Device>();

            if (currentDevice == null)
            {
                Debug.LogWarning("No device found for the customer.");
                yield break;
            }


            deviceScreen = (currentDevice as Device).GetComponent<DeviceScreen>();
            bool hasSeat = (currentDevice as Device).GetHasSeat();
            Transform leftHandPos = (currentDevice as Device).GetLeftHandPos();
            Transform rightHandPos = (currentDevice as Device).GetRightHandPos();

            if (leftHandPos != null && rightHandPos != null)
            {
                visualController.SetHandIKTargets(leftHandPos, rightHandPos);
            }

            visualController.SetSittingState(hasSeat);
            animationEvents.TriggerStartSitting();
            deviceScreen?.PlayVideo();


            for (float weight = 0f; weight < 1f; weight += Time.deltaTime)
            {
                visualController.AdjustIKWeight(weight);
                yield return null;
            }


            foodOrderSystem.CreateOrder();

            yield return new WaitForSeconds(time);


            stats.hunger += 10f;
            stats.money -= 20f;
            stats.energy -= 10f;
            stats.cafe -= 10f;


            for (float weight = 1f; weight > 0f; weight -= Time.deltaTime)
            {
                visualController.AdjustIKWeight(weight);
                yield return null;
            }

            visualController.SetSittingState(false);
            animationEvents.TriggerStopSitting();
            deviceScreen?.StopVideo();


            yield return new WaitUntil(() => visualController.IsIdleAnimationActive());

            customerSatisfaction.CalculateOverallSatisfaction();

            if (foodOrderSystem.CheckIfOrderServed()) // if Served add to Satisfaction
            {
                customerSatisfaction.UpdateSatisfactionLevel(0.1f);
            }
            else
            {
                customerSatisfaction.UpdateSatisfactionLevel(-0.1f);
            }

            deviceScreen = null;
            CashierManager.instance.AddCustomer(this.transform);

            yield break;
        }



        IEnumerator EatCoroutine(int time)
        {
            int counter = time;
            while (counter > 0)
            {
                yield return new WaitForSeconds(1);
                counter--;
            }

            stats.hunger -= 30f;
            stats.money -= 10f;
            aiBrain.finishedExecutingBestAction = true;
            yield break;
        }
        #endregion

    }
}
