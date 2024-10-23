using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField] private float horizontalSpeed = 10f;
    [SerializeField] private float verticalSpeed = 10f;
    [SerializeField] private float clampAngel = 80f;
    

    [SerializeField]private InputManager inputManager;
    private Vector3 startingRotation;

    protected override void Awake()
    {
      //  inputManager = InputManager.Instance;
        base.Awake();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if(stage == CinemachineCore.Stage.Aim)
            {
                if (startingRotation == null) startingRotation = transform.localRotation.eulerAngles;
                Vector2 DeltaInput = inputManager.GetLookVector();
                startingRotation.x += DeltaInput.x * verticalSpeed * Time.deltaTime;
                startingRotation.y += DeltaInput.y * horizontalSpeed * Time.deltaTime;
                startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngel, clampAngel);
                state.RawOrientation = Quaternion.Euler(-startingRotation.y,startingRotation.x, 0);
            }
        }
    }
}
