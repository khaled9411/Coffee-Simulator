using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TL.Core
{
    public class Context : MonoBehaviour
    {
        public GameObject storage;
        public GameObject home;
        public GameObject cafe;
        public GameObject restaurant;
        public float MinDistance = 5f;
        
        [HideInInspector] public string resourceTag = "resource";
        public Dictionary<DestinationType, List<Transform>> Destinations { get; private set; }

        private void Start()
        {
            List<Transform> restDestinations = new List<Transform>() { home.transform };
            List<Transform> storageDestinations = new List<Transform>() { storage.transform };
            List<Transform> cafeDestinations = new List<Transform>() { cafe.transform };
            List<Transform> restaurantDestinations = new List<Transform>() { restaurant.transform }; ;

            Destinations = new Dictionary<DestinationType, List<Transform>>()
            {
                { DestinationType.rest, restDestinations},
                { DestinationType.storage, storageDestinations },
                { DestinationType.resource, restaurantDestinations },
                { DestinationType.cafe, cafeDestinations }

            };
        }

        private List<Transform> GetAllResources()
        {
            Transform[] gameObjects = FindObjectsOfType<Transform>() as Transform[];
            List<Transform> resources = new List<Transform>();
            foreach (Transform go in gameObjects)
            {
                if (go.gameObject.tag == resourceTag)
                {
                    resources.Add(go);
                }
            }
            return resources;
        }

    }
}