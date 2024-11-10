using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TL.UtilityAI;
using static UnityEditor.Timeline.TimelinePlaybackControls;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using TL.UtilityAI.Actions;

namespace TL.Core
{
    public enum State
    {
        decide,
        move,
        execute
    }

    public class NPCController : MonoBehaviour
    {
        public MoveController mover { get; set; }
        public AIBrain aiBrain { get; set; }
        public NPCStats stats { get; set; }

        public Context context;


        private State currentState;

        public State GetCurrentState() { return currentState; }
        public void SetCurrentState(State state)
        {
            currentState = state;

            if (currentState == State.execute)
            {
                aiBrain.bestAction.Execute(this);
            }
            else if (currentState == State.move)
            {
                aiBrain.bestAction.SetRequiredDestination(this);
            }
        }

        public Transform availableDevice { get; set; }
        private bool hasActiveDevice = false;
        private IBuyableRespondable currentDevice = null;
        public Transform previousRequiredDestination;
        public Transform currentDestination;
        // Start is called before the first frame update
        void Start()
        {
            mover = GetComponent<MoveController>();
            aiBrain = GetComponent<AIBrain>();
            stats = GetComponent<NPCStats>();
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
                if (hasActiveDevice && currentDevice != null)
                {
                    Debug.Log($"Releasing device for NPC {gameObject.name}");
                    CafeManager.instance.LeaveItem(currentDevice);
                    hasActiveDevice = false;
                    currentDevice = null;
                }

                previousRequiredDestination = aiBrain.bestAction?.RequiredDestination;
                aiBrain.DecideBestAction();

                if (aiBrain.bestAction is Playe)
                {
                    availableDevice = CafeManager.instance.GetAvailableItemTransform();
                    if (availableDevice != null)
                    {
                        currentDevice = availableDevice.GetComponent<IBuyableRespondable>();
                        hasActiveDevice = true;

                        SetCurrentState(State.move);
                    }
                }
                else
                {
                    if (Vector3.Distance(mover.destination.position, this.transform.position) < 2f)
                    {
                        SetCurrentState(State.execute);
                    }
                    else
                    {
                        SetCurrentState(State.move);
                    }
                }
            }
            else if (GetCurrentState() == State.move)
            {
                float distance = Vector3.Distance(mover.destination.position, this.transform.position);
                if (distance < 2f)
                {
                    SetCurrentState(State.execute);
                }
            }
            else if (GetCurrentState() == State.execute)
            {
                if (aiBrain.finishedExecutingBestAction == true)
                {
                    Debug.Log("Exit execute state");
                    if (aiBrain.bestAction is Playe && hasActiveDevice && currentDevice != null)
                    {
                        Debug.Log($"Releasing device after execution for NPC {gameObject.name}");
                        CafeManager.instance.LeaveItem(currentDevice);
                        hasActiveDevice = false;
                        currentDevice = null;
                    }
                    SetCurrentState(State.decide);
                }
            }
        }

        private void OnDisable()
        {
            if (hasActiveDevice && currentDevice != null)
            {
                CafeManager.instance.LeaveItem(currentDevice);
            }
        }

        private void OnDestroy()
        {
            if (hasActiveDevice && currentDevice != null)
            {
                CafeManager.instance.LeaveItem(currentDevice);
            }
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
            yield return new WaitForSeconds(time);

            stats.hunger += 10f;
            stats.money -= 20f;
            stats.energy -= 10f;
            stats.cafe -= 10f;

            CashierManager.instance.AddCustomer(this.transform);


            //aiBrain.finishedExecutingBestAction = true;
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