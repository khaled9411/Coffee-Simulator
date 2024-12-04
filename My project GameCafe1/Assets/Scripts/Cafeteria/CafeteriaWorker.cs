using UnityEngine;
using UnityEngine.AI;

public class CafeteriaWorker : MonoBehaviour, ISaveable
{
    public static CafeteriaWorker Instance { get; private set; }

    [SerializeField]
    private string uniqueID;

    public string UniqueID
    {
        get { return uniqueID; }
        private set { uniqueID = value; }
    }

    public Transform cafeteriaPosition;
    public Animator animator;

    private NavMeshAgent navMeshAgent;
    private Transform targetCustomer;
    public bool isInTheCafeteria { get; set; }

    private bool _hasCafeteriaWorker;
    public bool hasCafeteriaWorker
    {
        get { return _hasCafeteriaWorker; }

        set
        {
            _hasCafeteriaWorker = value;
            cafeteriaVisual?.SetActive(_hasCafeteriaWorker);
        }
    }


    [SerializeField] private GameObject cafeteriaVisual;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        isInTheCafeteria = true;
    }

    public void AssignTask(Transform customerPosition)
    {
        targetCustomer = customerPosition;

        if (targetCustomer != null)
        {
            isInTheCafeteria = false;
            MoveTo(targetCustomer.position);
        }
    }

    void Update()
    {

        if(!hasCafeteriaWorker) return;



        if(navMeshAgent.velocity.magnitude > 0)
            animator.SetBool("IsWalking", true);
        else
            animator.SetBool("IsWalking", false);

        if (!isInTheCafeteria &&
            Vector3.Distance(transform.position, cafeteriaPosition.position) <= 0.3f)
        {
            isInTheCafeteria = true;
        }


        if (targetCustomer == null)
        {
            var nextOrder = CafeteriaSystem.instance.GetOrderToServe();
            if (nextOrder != null)
            {
                if (isInTheCafeteria)
                {
                    AssignTask(nextOrder);
                }
                else
                {
                    MoveToCafeteria();
                }
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, targetCustomer.position) <= 1f)
            {
                ServeCustomer();
            }
        }

        
    }

    private void ServeCustomer()
    {
        Debug.Log( targetCustomer.GetComponent<FoodOrderSystem>().CheckIfOrderServed());
        if (targetCustomer.TryGetComponent<FoodOrderSystem>(out FoodOrderSystem foodOrderSystem) && !foodOrderSystem.CheckIfOrderServed())
        {
            foodOrderSystem.ServeOrder();
        }


        ResetWorker();
    }

    //private void AssignNextTask()
    //{
    //    var nextOrder = CafeteriaSystem.instance.GetOrderToServe();
    //    if (nextOrder != null)
    //    {
    //        AssignTask(nextOrder);
    //    }
    //}

    private void MoveToCafeteria()
    {
        isInTheCafeteria = false;
        MoveTo(cafeteriaPosition.position);
    }

    private void MoveTo(Vector3 targetPosition)
    {
        navMeshAgent.SetDestination(targetPosition);
    }

    public void ResetWorker()
    {
        targetCustomer = null;
        MoveToCafeteria();
    }

    public SaveData SaveData()
    {
        PlayerPrefs.SetInt(UniqueID, hasCafeteriaWorker ? 1 : 0);

        return null;
    }

    public void LoadData(SaveData data)
    {

        hasCafeteriaWorker = PlayerPrefs.GetInt(UniqueID, 0) == 1;

    }
}
