using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension
{
    public const string PLAYER_PREFS_SENSITITVITY_MULTIPLAYER = "SensitivityMultiplyer";
    public static CinemachinePOVExtension Instance { get; private set; }

    [SerializeField] private float horizontalSpeed = 10f;
    [SerializeField] private float verticalSpeed = 10f;
    [SerializeField] private float clampAngel = 80f;
    private float sensitivityMultiplyer = 1;

    //[SerializeField]private InputManager inputManager;
    private Vector3 startingRotation;

    protected override void Awake()
    {
      //  inputManager = InputManager.Instance;
        base.Awake();
        Instance = this;
        sensitivityMultiplyer = PlayerPrefs.GetFloat(PLAYER_PREFS_SENSITITVITY_MULTIPLAYER, 1f);
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if(stage == CinemachineCore.Stage.Aim)
            {
                if (startingRotation == null) startingRotation = transform.localRotation.eulerAngles;
                Vector2 DeltaInput = InputManager.Instance.GetLookVector();
                startingRotation.x += DeltaInput.x * verticalSpeed * Time.deltaTime * sensitivityMultiplyer;
                startingRotation.y += DeltaInput.y * horizontalSpeed * Time.deltaTime * sensitivityMultiplyer;
                startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngel, clampAngel);
                state.RawOrientation = Quaternion.Euler(-startingRotation.y,startingRotation.x, 0);
            }
        }
    }

    public void SetsensitivityMultiplyer(float value)
    {
        sensitivityMultiplyer = value;
        PlayerPrefs.SetFloat(PLAYER_PREFS_SENSITITVITY_MULTIPLAYER, value);
    }
}
