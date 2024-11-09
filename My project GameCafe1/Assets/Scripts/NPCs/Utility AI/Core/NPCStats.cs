using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TL.Core
{
    public class NPCStats : MonoBehaviour
    {
        private float _energy;
        public float energy
        {
            get { return _energy; }
            set
            {
                _energy = Mathf.Clamp(value, 0, 100);
                OnStatValueChanged?.Invoke();
            }
        }

        private float _hunger;
        public float hunger
        {
            get { return _hunger; }
            set
            {
                _hunger = Mathf.Clamp(value, 0, 100);
                OnStatValueChanged?.Invoke();
            }
        }

        private float _money;
        public float money
        {
            get { return _money; }
            set
            {
                _money = Mathf.Clamp(value, 0, 1000);
                OnStatValueChanged?.Invoke();
            }
        }

        private float _cafe;
        public float cafe
        {
            get { return _cafe; }
            set
            {
                _cafe = Mathf.Clamp(value, 0, 100);
                OnStatValueChanged?.Invoke();

            }
        }

        [SerializeField] private float timeToDecreaseHunger = 5f;
        [SerializeField] private float timeToDecreaseEnergy = 5f;
        private float timeLeftEnergy;
        private float timeLeftHunger;


        public delegate void StatValueChangedHandler();
        public event StatValueChangedHandler OnStatValueChanged;

        // Start is called before the first frame update
        void Start()
        {
            hunger = Random.Range(20, 80);
            energy = Random.Range(20, 80);
            money = Random.Range(10, 1000);
            cafe = Random.Range(10, 100);

            // Test case: NPC will likely work
            //hunger = 0;
            //energy = 100;
            //money = 50;
            //cafe = 30;

            // Test case: NPC will likely eat
            //hunger = 90;
            //energy = 50;
            //money = 700;
            //cafe = 20;

            // Test case: NPC will likely sleep
            //hunger = 0;
            //energy = 10;
            //money = 500;
            //cafe = 40;

            // Test case: NPC will likely Play
            //hunger = 0;
            //energy = 50;
            //money = 500;
            //cafe = 90;
        }



        public void UpdateHunger()
        {
            if (timeLeftHunger > 0)
            {
                timeLeftHunger -= Time.deltaTime;
                return;
            }

            timeLeftHunger = timeToDecreaseHunger;
            hunger += 1;
        }

        public void UpdateEnergy(bool shouldNotUpdateEnergy)
        {
            if (shouldNotUpdateEnergy)
            {
                return;
            }

            if (timeLeftEnergy > 0)
            {
                timeLeftEnergy -= Time.deltaTime;
                return;
            }

            timeLeftEnergy = timeToDecreaseEnergy;
            energy -= 1;
        }
    }
}