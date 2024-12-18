using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameInfoItem", menuName = "Game Info/Info Item", order = 1)]
[Serializable]
public class GameInfoItem : ScriptableObject
{
    [Header("Game Areas")]
    public List<AreaInfo> gameAreas = new List<AreaInfo>
    {
        new AreaInfo
        {
            name = "Game Cafe",
            description = "A place where players can buy gaming devices and expand their business. Manage temperature, services, and customer satisfaction.",
            category = "Area",
        },
        new AreaInfo
        {
            name = "Computer Area",
            description = "A section for player feedback and customer reviews. Monitor customer satisfaction and improve services.",
            category = "Area",
        }
    };

    [Header("Services")]
    public List<ServiceInfo> services = new List<ServiceInfo>
    {
        new ServiceInfo
        {
            name = "Cashier Service",
            description = "Hire a cashier to handle customer transactions automatically. Reduces player's direct involvement in cash management.",
            category = "Service",
            costToBuy = 500
        },
        new ServiceInfo
        {
            name = "Cafe Worker",
            description = "Hire a cafe worker to serve food and drinks to customers. Improves customer experience and reduces player's workload.",
            category = "Service",
            costToBuy = 750
        },
        new ServiceInfo
        {
            name = "Cleaning Service",
            description = "Hire a cleaning worker to maintain cafe hygiene once per day. Prevents negative customer reviews and maintains cafe reputation.",
            category = "Service",
            costToBuy = 300
        },
        new ServiceInfo
        {
            name = "Promotion Service",
            description = "Increase customer count by investing in marketing and promotions. Attracts more customers to your game cafe.",
            category = "Service",
            costToBuy = 1000
        }
    };

    [Header("Game Mechanics")]
    public List<MechanicInfo> gameMechanics = new List<MechanicInfo>
    {
        new MechanicInfo
        {
            name = "Energy System",
            description = "Player has a hunger bar. If it depletes, player faints and loses 30 coins. Must eat and rest to maintain energy levels.",
            category = "Mechanic"
        },
        new MechanicInfo
        {
            name = "Sleep Mechanic",
            description = "Player must go to bed by 10 PM. Fainting occurs at midnight, losing 30 coins. Sleeping saves daily achievements and purchases.",
            category = "Mechanic"
        }
    };

    [Header("Locations")]
    public List<LocationInfo> locations = new List<LocationInfo>
    {
        new LocationInfo
        {
            name = "Home",
            description = "Player's primary residence where they sleep and recover. Respawn point after fainting.",
            category = "Location"
        },
        new LocationInfo
        {
            name = "Restaurant",
            description = "Place to buy food and replenish energy. Essential for maintaining player's health and avoiding fainting.",
            category = "Location"
        }
    };

    [Header("Equipment")]
    public List<EquipmentInfo> equipment = new List<EquipmentInfo>
    {
        new EquipmentInfo
        {
            name = "Air Conditioning",
            description = "Must be purchased to regulate temperature as device count increases. Prevents customer discomfort in crowded areas.",
            category = "Equipment",
            costToBuy = 200
        }
    };

    [Serializable]
    public class BaseInfo
    {
        public string name = "";
        [TextArea(3, 10)]
        public string description = "";
        public string category = "";
        public Sprite icon = null;
    }

    [Serializable]
    public class AreaInfo : BaseInfo
    {
    }

    [Serializable]
    public class ServiceInfo : BaseInfo
    {
        public float costToBuy = 0;
    }

    [Serializable]
    public class MechanicInfo : BaseInfo
    {
    }

    [Serializable]
    public class LocationInfo : BaseInfo
    {
    }

    [Serializable]
    public class EquipmentInfo : BaseInfo
    {
        public float costToBuy = 0;
    }
}