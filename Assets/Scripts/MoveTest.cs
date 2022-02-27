using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class MoveTest : MonoBehaviour
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

        [HideInInspector] public Transform cameraOffset;
        [HideInInspector] public Transform tpRig;

        [HideInInspector] public GameObject tpCamObj;
        [HideInInspector] public GameObject fpCamObj;
        
        [HideInInspector] public Transform transform;
        [HideInInspector] public CapsuleCollider collider;
        [HideInInspector] public Rigidbody rigid;
        [HideInInspector] public Animator anim;
    }

    [Serializable]
    public class KeyOption
    {
        public KeyCode moveForward = KeyCode.W;
        public KeyCode moveBackward = KeyCode.S;
        public KeyCode moveLeft = KeyCode.A;
        public KeyCode moveRight = KeyCode.D;
        public KeyCode run = KeyCode.LeftShift;
        public KeyCode jump = KeyCode.Space;
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
        
        [Range(1f, 10f), Tooltip("점프 강도")]
        public float jumpForce = 5.5f;
        
        [Range(0.0f, 2.0f), Tooltip("점프 쿨타임")]
        public float jumpCooldown = 1.0f;
        
        [Tooltip("지면으로 체크할 레이어 설정")]
        public LayerMask groundLayerMask = -1;
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
    public class AnimatorOption
    {
        public string paramMoveX = "Move X";
        public string paramMoveZ = "Move Z";
        public string paramDistY = "Dist Y";
        public string paramGrounded = "Grounded";
        public string paramJump = "Jump";
    }
    
    [Serializable]
    public class CharacterState
    {
        public bool isCurrentFp;
        public bool isMoving;
        public bool isRunning;
        public bool isCursorActive;
        public bool isGrounded;
    }
    
    #endregion
    
    /*************************************************************************
    *                           Fields, Properties
    *************************************************************************/

    #region .

    public Components Com => components;
    public MovementOption MoveOption => moveOption;
    public KeyOption Key => keyOption;
    public CameraOption CamOption => cameraOption;
    public AnimatorOption AnimOption => animatorOption;
    public CharacterState State => state;

    [SerializeField] private Components components = new Components();
    [Space, SerializeField] private MovementOption moveOption = new MovementOption();
    [Space, SerializeField] private KeyOption keyOption = new KeyOption();
    [Space, SerializeField] private CameraOption cameraOption = new CameraOption();
    [Space, SerializeField] private AnimatorOption animatorOption = new AnimatorOption();
    [Space, SerializeField] private CharacterState state = new CharacterState();

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

    
    [SerializeField]
    private float distFromGround;
    
    private float _groundCheckRadius;
    private float _currentJumpCooldown;
    
    // Animation Params
    private float _moveX;
    private float _moveZ;

    /*
     * Current Movement Variables
     */
    
    /*              키보드 WASD 입력으로 얻는 로컬 이동 벡터           */
    [SerializeField]
    private Vector3 moveDir;
    
    /*              월드 이동 벡터              */
    [SerializeField]
    private Vector3 worldMoveDir;

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
        _deltaTime = Time.deltaTime;

        // 1. Check, Key Input
        ShowCursorToggle();
        CameraViewToggle();
        SetValuesByKeyInput();
        CheckDistanceFromGround();

        // 2. Behaviors, Camera Actions
        Rotate();
        TpCameraZoom();

        // 3. Updates
        Move();
        Jump();
        UpdateAnimationParams();
        UpdateCurrentValues();
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
        TryGetComponent(out Com.rigid);
        
        Com.transform = GetComponentInChildren<Transform>();
        Com.anim = GetComponentInChildren<Animator>();
        
        Com.tpCamObj = Com.tpCamera.gameObject;
        Com.tpRig = Com.tpCamera.transform.parent;

        Com.fpCamObj = Com.fpCamera.gameObject;
        Com.cameraOffset = Com.fpCamera.transform.parent;
    }

    private void InitSettings()
    {
        // Rigidbody
        if (Com.rigid)
        {
            // 회전은 트랜스폼을 통해 직접 제어할 것이기 때문에 리지드바디 회전은 제한
            Com.rigid.constraints = RigidbodyConstraints.FreezeRotation;
        }
        
        TryGetComponent(out CapsuleCollider cCol);
        _groundCheckRadius = cCol ? cCol.radius : 0.1f;
        
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
        
        // 줌 거리 측정
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
        moveDir = Vector3.Lerp(moveDir, moveInput, MoveOption.acceleration);
        // 마우스 회전
        _rotation = new Vector2(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y"));
        State.isMoving = moveDir.sqrMagnitude > 0.01f;
        State.isRunning = Input.GetKey(Key.run);    // 달리기 버튼을 눌렀을 경우
        // 휠 입력값
        _tpCameraWheelInput = Input.GetAxisRaw("Mouse ScrollWheel");
        _currentWheel = Mathf.Lerp(_currentWheel, _tpCameraWheelInput, CamOption.zoomAccel);

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
        worldMoveDir = Com.cameraOffset.TransformDirection(moveDir);
        worldMoveDir *= (MoveOption.speed) * (State.isRunning ? MoveOption.runningCoef : 1f);
        
        // Y축 속도는 유지하면서 XZ평면 이동
        Com.rigid.velocity = new Vector3(worldMoveDir.x, Com.rigid.velocity.y, worldMoveDir.z);
        // Com.controller.Move(_worldMoveDir * Time.deltaTime);
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

        Vector3 dir = Com.tpRig.TransformDirection(moveDir);
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

        float zoom = Time.deltaTime * CamOption.zoomSpeed;
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
    
    private void UpdateAnimationParams()
    {
        float x, z;

        if (State.isCurrentFp)
        {
            x = moveDir.x;
            z = moveDir.z;

            if (State.isRunning)
            {
                x *= 2f;
                z *= 2f;
            }
        }
        else
        {
            x = 0f;
            z = moveDir.sqrMagnitude > 0f ? 1f : 0f;

            if (State.isRunning)
            {
                z *= 2f;
            }
        }

        // 보간
        const float lerpSpeed = 0.05f;
        _moveX = Mathf.Lerp(_moveX, x, lerpSpeed);
        _moveZ = Mathf.Lerp(_moveZ, z, lerpSpeed);

        Com.anim.SetFloat(AnimOption.paramMoveX, _moveX);
        Com.anim.SetFloat(AnimOption.paramMoveZ, _moveZ);
        Com.anim.SetFloat(AnimOption.paramDistY, distFromGround);
        Com.anim.SetBool(AnimOption.paramGrounded, State.isGrounded);
    }
    
    /// <summary> 땅으로부터의 거리 체크 </summary>
    private void CheckDistanceFromGround()
    {
        Vector3 ro = transform.position + Vector3.up;
        Vector3 rd = Vector3.down;
        Ray ray = new Ray(ro, rd);

        const float rayDist = 500f;
        const float threshold = 0.3f;

        bool cast =
            Physics.SphereCast(ray, _groundCheckRadius, out var hit, rayDist, MoveOption.groundLayerMask);

        distFromGround = cast ? (hit.distance - 1f + _groundCheckRadius) : float.MaxValue;
        State.isGrounded = distFromGround <= _groundCheckRadius + threshold;
    }
    
    private void Jump()
    {
        if (!State.isGrounded) return;
        if (_currentJumpCooldown > 0f) return; // 점프 쿨타임

        if (Input.GetKeyDown(Key.jump))
        {
            Debug.Log("JUMP");

            // 하강 중 점프 시 속도가 합산되지 않도록 속도 초기화
            Com.rigid.velocity = Vector3.zero;

            Com.rigid.AddForce(Vector3.up * MoveOption.jumpForce, ForceMode.VelocityChange);

            // 애니메이션 점프 트리거
            Com.anim.SetTrigger(AnimOption.paramJump);

            // 쿨타임 초기화
            _currentJumpCooldown = MoveOption.jumpCooldown;
        }
    }
    
    private void UpdateCurrentValues()
    {
        if(_currentJumpCooldown > 0f)
            _currentJumpCooldown -= _deltaTime;
    }

    #endregion

    /*************************************************************************
    *                           On Trigger Methods
    *************************************************************************/

    #region .

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Portal"))
        {
            if (other.gameObject.name == "MainPortal")
            {
                AsyncOperation _async;
                _async = SceneManager.LoadSceneAsync("MainScene");
            }
            else if (other.gameObject.name == "PrivatePortal")
            {
                AsyncOperation _async;
                _async = SceneManager.LoadSceneAsync("PrivateScene");
            }
            else if (other.gameObject.name == "AtelierPortal")
            {
                Vector3 destination = GameObject.Find("LobbyPortal").transform.position;
                Com.transform.position = new Vector3(destination.x, destination.y, destination.z - 3);
            }
            else if (other.gameObject.name == "LobbyPortal")
            {
                Vector3 destination = GameObject.Find("AtelierPortal").transform.position;
                Com.transform.position = new Vector3(destination.x, destination.y, destination.z - 3);
            }
        }
    }

    #endregion
    
    
}
