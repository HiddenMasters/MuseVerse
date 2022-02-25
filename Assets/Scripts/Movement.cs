using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Movement : MonoBehaviour
{
    /*************************************************************************
    *                               Definitions
    *************************************************************************/

    #region .

    public enum CameraType
    {
        FpCamera,
        TpCamera
    };

    [Serializable]
    public class Components
    {
        public Camera tpCamera;
        public Camera fpCamera;
        public Rigidbody rigid;
        public Transform transform;

        [HideInInspector] public Transform cameraOffset;
        [HideInInspector] public Transform tpRig;

        [HideInInspector] public GameObject tpCamObj;
        [HideInInspector] public GameObject fpCamObj;
    }

    [Serializable]
    public class KeyOption
    {
        public KeyCode moveForward = KeyCode.W;
        public KeyCode moveBackward = KeyCode.S;
        public KeyCode moveLeft = KeyCode.A;
        public KeyCode moveRight = KeyCode.D;
        public KeyCode run = KeyCode.LeftShift;
        public KeyCode switchCamera = KeyCode.Tab;
        public KeyCode showCursor = KeyCode.LeftAlt;
    }

    [Serializable]
    public class MovementOption
    {
        [Range(1f, 10f), Tooltip("이동속도")]
        public float speed = 10f;
        
        [Range(1f, 3f), Tooltip("달리기 이동속도 증가 계수")]
        public float runningCoef = 1.5f;
        
        [Range(0f, 1f), Tooltip("이동 가속도")]
        public float acceleration = 0.75f;
    }

    [Serializable]
    public class CameraOption
    {
        [Tooltip("게임 시작 시 카메라")]
        public CameraType initialCamera;
        
        [Range(1f, 10f), Tooltip("카메라 상하좌우 회전속도")]
        public float rotationSpeed = 2f;
        
        [Range(-90f, 0f), Tooltip("올려다보기 제한 각도")]
        public float lookUpDegree = -60f;
        
        [Range(0f, 75f), Tooltip("내려다보기 제한 각도")]
        public float lookDownDegree = 75f;
        
        [Range(0f, 3.5f), Tooltip("줌 확대 최대 거리")]
        public float zoomInDistance = 3f;
        
        [Range(0f, 75f), Tooltip("줌 축소 최대 거리")]
        public float zoomOutDistance = 3f;
        
        [Range(0f, 75f), Tooltip("줌 속도")]
        public float zoomSpeed = 20f;
        
        [Range(0f, 75f), Tooltip("줌 가속")]
        public float zoomAccel = 0.1f;
    }

    [Serializable]
    public class CharacterState
    {
        public bool isCurrentFp;
        public bool isMoving;
        public bool isRunning;
        public bool isCursorActive;
    }
    
    #endregion
    
    /*************************************************************************
    *                           Fields, Properties
    *************************************************************************/

    #region .

    public Components Com => _components;
    public MovementOption MoveOption => _moveOption;
    public KeyOption Key => _keyOption;
    public CameraOption CamOption => _cameraOption;
    public CharacterState State => _state;

    [SerializeField] private Components _components = new Components();
    [Space, SerializeField] private MovementOption _moveOption = new MovementOption();
    [Space, SerializeField] private KeyOption _keyOption = new KeyOption();
    [Space, SerializeField] private CameraOption _cameraOption = new CameraOption();
    [Space, SerializeField] private CharacterState _state = new CharacterState();

    /*              Time.deltaTime 항상 저장            */
    private float _deltaTime;
    
    /*              마우스 움직임을 통해 얻는 회전 값            */
    private Vector3 _rotation;
    
    /*              TP 카메라 ~ Rig 초기 거리            */
    private float _tpCamZoomInitialDistance;

    /*              TP 카메라 휠 입력 값            */
    private float _tpCameraWheelInput = 0;
    
    /*              선형보간된 현제 휠 입력 값            */
    private float _currentWheel;

    
    /*
     * Current Movement Variables
     */
    
    /*              키보드 WASD 입력으로 얻는 로컬 이동 벡터           */
    [SerializeField]
    private Vector3 _moveDir;
    
    /*              월드 이동 벡터              */
    [SerializeField]
    private Vector3 _worldMoveDir;

    #endregion
    
    /*************************************************************************
    *                               Unity Events
    *************************************************************************/

    #region .

    private void Start()
    {
        InitComponents();
        InitSettings();
    }

    private void Update()
    {
        ShowCursorToggle();
        CameraViewToggle();
        SetValuesByKeyInput();
        TpCameraZoom();
        Rotate();
        Move();
    }

    #endregion
    
    /*************************************************************************
    *                               Check Methods
    *************************************************************************/

    #region .

    private void LogNotInitializedComponentError<T>(T component, string componentName) where T : Component
    {
        if(component == null)
            Debug.LogError($"{componentName} 컴포넌트를 인스펙터에 넣어주세요");
    }
    
    #endregion
    
    /*************************************************************************
    *                               Init Methods
    *************************************************************************/

    #region .

    private void InitComponents()
    {
        LogNotInitializedComponentError(Com.tpCamera, "TP Camera");
        LogNotInitializedComponentError(Com.tpCamera, "TP Camera");

        Com.tpCamObj = Com.tpCamera.gameObject;
        Com.tpRig = Com.tpCamera.transform.parent;

        Com.fpCamObj = Com.fpCamera.gameObject;
        Com.cameraOffset = Com.fpCamera.transform.parent;
    }

    private void InitSettings()
    {
        // 모든 카메라 게임오브젝트 비활성화
        var allCams = FindObjectsOfType<Camera>();
        foreach (var cam in allCams)
        {
            cam.gameObject.SetActive(false);
        }
        
        // 설정한 카메라 하나만 활성화
        State.isCurrentFp = (CamOption.initialCamera == CameraType.FpCamera);
        Com.fpCamObj.SetActive(State.isCurrentFp);
        Com.tpCamObj.SetActive(!State.isCurrentFp);
        
        // 줌
        _tpCamZoomInitialDistance = Vector3.Distance(Com.tpRig.position, Com.tpCamera.transform.position);
    }

    #endregion
    
    /*************************************************************************
    *                                   Methods
    *************************************************************************/

    #region .

    private void SetValuesByKeyInput()
    {
        float h = 0f, v = 0f;
        
        // WASD 방향키로 움직이는 경우
        if (Input.GetKey(Key.moveForward)) v += 1.0f;
        if (Input.GetKey(Key.moveBackward)) v -= 1.0f;
        if (Input.GetKey(Key.moveLeft)) h -= 1.0f;
        if (Input.GetKey(Key.moveRight)) h += 1.0f;

        Vector3 moveInput = new Vector3(h, 0f, v).normalized;
        _moveDir = Vector3.Lerp(_moveDir, moveInput, MoveOption.acceleration);
        //  마우스 회전
        _rotation = new Vector2(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y"));
        State.isMoving = _moveDir.sqrMagnitude > 0.01f;
        State.isRunning = Input.GetKey(Key.run);    // 달리기 버튼을 눌렀을 경우
    }

    private void Move()
    {
        // 이동하지 않는 경우, 미끄럼 방지
        if (State.isMoving == false)
        {
            Com.rigid.velocity = new Vector3(0f, Com.rigid.velocity.y, 0f);
            return;
        }
        
        // 실제 이동 벡터 계산
        _worldMoveDir = Com.cameraOffset.TransformDirection(_moveDir);
        _worldMoveDir *= (MoveOption.speed) * (State.isRunning ? MoveOption.runningCoef : 1f);
        
        // Y축 속도는 유지하면서 XZ평면 이동
        Com.rigid.velocity = new Vector3(_worldMoveDir.x, Com.rigid.velocity.y, _worldMoveDir.z);
    }

    private void Rotate()
    {
        if (State.isCurrentFp)
        {
            if(!State.isCursorActive)
                RotateFP();
        }
        else
        {
            if (!State.isCursorActive)
                RotateTP();
            RotateFPRoot();
        }
    }

    /// <summary> 1인칭 회전 </summary>
    private void RotateFP()
    {
        float deltaCoef = Time.deltaTime * 50f;

        // 상하 : FP Rig 회전
        float xRotPrev = Com.cameraOffset.localEulerAngles.x;
        float xRotNext = xRotPrev + _rotation.y
            * CamOption.rotationSpeed * deltaCoef;

        if (xRotNext > 180f)
            xRotNext -= 360f;

        // 좌우 : FP Root 회전
        float yRotPrev = Com.transform.localEulerAngles.y;
        float yRotNext =
            yRotPrev + _rotation.x
            * CamOption.rotationSpeed * deltaCoef;

        // 상하 회전 가능 여부
        bool xRotatable =
            CamOption.lookUpDegree < xRotNext &&
            CamOption.lookDownDegree > xRotNext;

        // FP Rig 상하 회전 적용
        Com.cameraOffset.localEulerAngles = Vector3.right * (xRotatable ? xRotNext : xRotPrev);

        // FP Root 좌우 회전 적용
        Com.transform.localEulerAngles = Vector3.up * yRotNext;
    }

    /// <summary> 3인칭 회전 </summary>
    private void RotateTP()
    {
        float deltaCoef = Time.deltaTime * 50f;

        // 상하 : TP Rig 회전
        float xRotPrev = Com.tpRig.localEulerAngles.x;
        float xRotNext = xRotPrev + _rotation.y
            * CamOption.rotationSpeed * deltaCoef;

        if (xRotNext > 180f)
            xRotNext -= 360f;

        // 좌우 : TP Rig 회전
        float yRotPrev = Com.tpRig.localEulerAngles.y;
        float yRotNext =
            yRotPrev + _rotation.x
            * CamOption.rotationSpeed * deltaCoef;

        // 상하 회전 가능 여부
        bool xRotatable =
            CamOption.lookUpDegree < xRotNext &&
            CamOption.lookDownDegree > xRotNext;

        Vector3 nextRot = new Vector3
        (
            xRotatable ? xRotNext : xRotPrev,
            yRotNext,
            0f
        );

        // TP Rig 회전 적용
        Com.tpRig.localEulerAngles = nextRot;
    }

    /// <summary> 3인칭일 경우 FP Root 회전 </summary>
    private void RotateFPRoot()
    {
        if (State.isMoving == false) return;

        Vector3 dir = Com.tpRig.TransformDirection(_moveDir);
        float currentY = Com.transform.localEulerAngles.y;
        float nextY = Quaternion.LookRotation(dir, Vector3.up).eulerAngles.y;

        if (nextY - currentY > 180f) nextY -= 360f;
        else if (currentY - nextY > 180f) nextY += 360f;

        Com.transform.eulerAngles = Vector3.up * Mathf.Lerp(currentY, nextY, 0.1f);
    }
    
    private void ShowCursorToggle()
    {
        if (Input.GetKeyDown(Key.showCursor))
            State.isCursorActive = !State.isCursorActive;

        ShowCursor(State.isCursorActive);
    }

    private void ShowCursor(bool value)
    {
        Cursor.visible = value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void TpCameraZoom()
    {
        if (State.isCurrentFp) return;                // TP 카메라만 가능
        if (Mathf.Abs(_currentWheel) < 0.01f) return; // 휠 입력 있어야 가능

        Transform tpCamTr = Com.tpCamera.transform;
        Transform tpCamRig = Com.tpRig;

        float zoom = _deltaTime * CamOption.zoomSpeed;
        float currentCamToRigDist = Vector3.Distance(tpCamTr.position, tpCamRig.position);
        Vector3 move = Vector3.forward * zoom * _currentWheel * 10f;

        // Zoom In
        if (_currentWheel > 0.01f)
        {
            if (_tpCamZoomInitialDistance - currentCamToRigDist < CamOption.zoomInDistance)
            {
                tpCamTr.Translate(move, Space.Self);
            }
        }
        // Zoom Out
        else if (_currentWheel < -0.01f)
        {

            if (currentCamToRigDist - _tpCamZoomInitialDistance < CamOption.zoomOutDistance)
            {
                tpCamTr.Translate(move, Space.Self);
            }
        }
    }
    
    private void CameraViewToggle()
    {
        if (Input.GetKeyDown(Key.switchCamera))
        {
            State.isCurrentFp = !State.isCurrentFp;
            Com.fpCamObj.SetActive(State.isCurrentFp);
            Com.tpCamObj.SetActive(!State.isCurrentFp);
        }
    }
    
    #endregion
    
}