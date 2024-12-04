using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class Focus : MonoBehaviour, IInteractable
{
    [field: SerializeField] public string verbName { get; set; }

    [Header("Camera Setup")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform normalCameraPosition;
    [SerializeField] private Transform computerViewPosition;
    [SerializeField] private GameObject lookField;

    [Header("Camera Settings")]
    [SerializeField] private float transitionDuration = 1.0f;
    [SerializeField] private Ease transitionEase = Ease.InOutQuad;
    //[SerializeField] private float rotationSmoothness = 5f;

    [Header("==Debuging==")]
    [SerializeField] private KeyCode exitKey = KeyCode.Escape;

    private CinemachineTransposer cameraTransposer;
    private CinemachinePOV cameraPOV;
    private bool isViewingComputer = false;
    private Vector3 originalOffset;
    private Transform currentTarget;

    public void Interact()
    {
        EnterComputerView();
    }

    // Start is called before the first frame update
    private void Start()
    {
        cameraTransposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        cameraPOV = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        if (cameraTransposer != null)
        {
            originalOffset = cameraTransposer.m_FollowOffset;
        }

        currentTarget = virtualCamera.Follow;
    }

    private void Update()
    {
        if (isViewingComputer && Input.GetKeyDown(exitKey))
        {
            ExitComputerView();
        }
    }

    public void EnterComputerView()
    {
        if (isViewingComputer) return;
        lookField.SetActive(false);
        PlayerMovement.Instance.SetCanMove(false);

        // Smooth rotation to center
        DOTween.To(() => cameraPOV.m_HorizontalAxis.Value,
                   x => cameraPOV.m_HorizontalAxis.Value = x,
                   0f,
                   transitionDuration)
            .SetEase(transitionEase);

        DOTween.To(() => cameraPOV.m_VerticalAxis.Value,
                   x => cameraPOV.m_VerticalAxis.Value = x,
                   0f,
                   transitionDuration)
            .SetEase(transitionEase);

        isViewingComputer = true;
        MoveToComputer();
    }

    public void ExitComputerView()
    {
        if (!isViewingComputer) return;
        lookField.SetActive(true);

        PlayerMovement.Instance.SetCanMove(true);

        isViewingComputer = false;

        ReturnToNormalView();
    }

    private void MoveToComputer()
    {
        DOTween.To(() => cameraTransposer.m_FollowOffset,
                   x => cameraTransposer.m_FollowOffset = x,
                   computerViewPosition.localPosition,
                   transitionDuration)
            .SetEase(transitionEase)
            .SetUpdate(true);

        virtualCamera.Follow = computerViewPosition;
    }

    private void ReturnToNormalView()
    {
        DOTween.To(() => cameraTransposer.m_FollowOffset,
                   x => cameraTransposer.m_FollowOffset = x,
                   originalOffset,
                   transitionDuration)
            .SetEase(transitionEase)
            .SetUpdate(true);

        virtualCamera.Follow = currentTarget;
    }

    public bool IsViewingComputer()
    {
        return isViewingComputer;
    }


}
