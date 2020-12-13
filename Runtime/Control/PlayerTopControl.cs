using System;
using Camera;
using UnityEngine;
using Util;

namespace Control
{
    public enum PlayerTopMovementType
    {
        ChangePosition,
        ChangeVelocity
    }
    
    public class PlayerTopControl : MonoBehaviour
    {
        public LayerMask AimLayerMask;
        
        private Vector3 _direction;

        private float _dashCooldown;

        private float _speed;
        private int _jumpCount;
        private float _dashSpeed;
        private bool _isDashStarted;
        private float _groundCooldown;
        private float _lastFallSpeed;
        private float _baseSmoothRate = 0.25f;
        private float _finalSmoothRate = 32f;

        public float MaxSpeed = 1f;
        public float MaxDashSpeed = 1f;
        public float MaxJumpPower = 4f;
        public float GroundIfFallSpeedMoreThan = 40f;
        public float ControlPower { get; set; } = 1f;

        public Vector3 LookingAtAny { get; private set; }
        public Vector3 LookingAtWithCollision { get; private set; }
        public Vector3 LookingAt { get; private set; }
        public Vector3 LookingAtDirection { get; private set; }

        public PlayerTopMovementType MovementType = PlayerTopMovementType.ChangePosition;
        
        public Vector3 CurrentDirection { get; private set; } = new Vector3(0, 0, 1);
        public float CurrentSpeed => _speed;
        
        public bool IsJump => _jumpCount > 0;
        public bool IsRun { get; private set; }
        public bool IsDash { get; private set; }
        public bool IsFall => GetComponent<Rigidbody>().velocity.y < -1f;
        public bool IsGround { get; private set; }
        public bool IsInputLock { get; set; }

        // Events
        public Action OnDashStart;
        public Action OnDashEnd;
        public Action<float> OnDash;
        public Action OnJump;
        public Action<float> OnGround;

        // Parts
        public GameObject Head;
        public GameObject Body;
        public GameObject Legs;
        public GameObject Aim;

        private void Start()
        {
        }

        private void OnDrawGizmos()
        {
            if (Aim)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(Aim.transform.position, LookingAtWithCollision);
            }
        }

        private void HandleInput()
        {
            if (IsInputLock)
            {
                IsRun = false;
                return;
            }
            
            // Check if run
            IsRun = Input.GetKey(KeyCode.W) ||
                    Input.GetKey(KeyCode.S) ||
                    Input.GetKey(KeyCode.A) ||
                    Input.GetKey(KeyCode.D);
            
            // Direction control
            if (Input.GetKey(KeyCode.A))
            {
                _direction.z = 0;
                _direction.x = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                _direction.z = 0;
                _direction.x = 1;
            }

            if (Input.GetKey(KeyCode.W))
            {
                _direction.z = 1;
                _direction.x = 0;

                if (Input.GetKey(KeyCode.A))
                {
                    _direction.z = 0.75f;
                    _direction.x = -0.75f;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    _direction.z = 0.75f;
                    _direction.x = 0.75f;
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _direction.z = -1;
                _direction.x = 0;

                if (Input.GetKey(KeyCode.A))
                {
                    _direction.z = -0.75f;
                    _direction.x = -0.75f;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    _direction.z = -0.75f;
                    _direction.x = 0.75f;
                }
            }
            
            // Dash
            if (Input.GetKeyDown(KeyCode.LeftShift) && _dashCooldown <= 0)
            {
                _dashSpeed = MaxDashSpeed;
                _dashCooldown = 0.45f;
                _isDashStarted = true;
                IsDash = true;
                OnDashStart?.Invoke();
            }
            
            // Jump control
            if (Input.GetKeyDown(KeyCode.Space) && _jumpCount < 2)
            {
                transform.position += new Vector3(0, 0.12f, 0);
                GetComponent<Rigidbody>().AddForce(new Vector3(0, MaxJumpPower, 0), ForceMode.VelocityChange);
                _jumpCount += 1;
                OnJump?.Invoke();
            }
        }
        
        private void Update()
        {
            HandleInput();
            
            // Can't run while ground
            if (IsGround) IsRun = false;

            // Smooth direction
            CurrentDirection += (_direction - CurrentDirection) / (_finalSmoothRate * 0.5f);

            // Ground cooldown
            _groundCooldown -= Time.deltaTime;
            if (_groundCooldown <= 0f) IsGround = false;

            // Speed control
            if (IsRun && !IsGround)
            {
                if (IsJump || IsFall) _speed += (MaxSpeed * 0.7f * ControlPower - _speed) / _finalSmoothRate;
                else _speed += (MaxSpeed * ControlPower - _speed) / _finalSmoothRate;
            }
            else _speed += (0 - _speed) / _finalSmoothRate;
            
            // Dash logic
            _dashCooldown -= Time.deltaTime;
            if (_dashCooldown <= 0 && _isDashStarted)
            {
                OnDashEnd?.Invoke();
                _isDashStarted = false;
                _dashSpeed = 0;
                IsDash = false;
            }

            if (_isDashStarted) OnDash?.Invoke(_dashCooldown / 0.2f);
            if (IsDash) _speed += (_dashSpeed * ControlPower - _speed) / _finalSmoothRate;

            // Check camera
            if (UnityEngine.Camera.main != null)
            {
                var camera = UnityEngine.Camera.main.GetComponent<TopCamera>();
                if (camera)
                {
                    var hitPoint = camera.RayHitPointAny;
                    var q = Quaternion.LookRotation(hitPoint - transform.position);

                    if (Head)
                    {
                        Head.transform.rotation = Quaternion.Lerp(
                            Head.transform.rotation,
                            Quaternion.Euler(new Vector3(0, q.eulerAngles.y, 0)),
                            Time.deltaTime * 6f
                        );
                    }

                    if (Body)
                    {
                        Body.transform.rotation = Quaternion.Lerp(
                            Body.transform.rotation,
                            Quaternion.Euler(new Vector3(0, q.eulerAngles.y, 0)),
                            Time.deltaTime * 20f
                        );
                    }
                    else transform.rotation = Quaternion.Euler(new Vector3(0, q.eulerAngles.y, 0));

                    LookingAt = hitPoint;
                    LookingAtDirection = (hitPoint - transform.position).normalized;
                }
            }

            //  Legs control
            if (Legs)
            {
                if (CurrentDirection != Vector3.zero)
                {
                    Legs.transform.rotation = Quaternion.Lerp(
                        Legs.transform.rotation,
                        Quaternion.LookRotation(CurrentDirection, Vector3.up),
                        Time.deltaTime * 10f
                    );
                }
            }

            _lastFallSpeed = GetComponent<Rigidbody>().velocity.y;

            // Smooth rate
            _finalSmoothRate = _baseSmoothRate / Time.deltaTime;
            
            // Looking with collision
            if (Aim)
            {
                RaycastHit hit;
                var ray = new Ray(Aim.transform.position, (LookingAt - Aim.transform.position).normalized);
                if (UnityEngine.Physics.Raycast(ray, out hit, Mathf.Infinity, AimLayerMask))
                {
                    LookingAtWithCollision = hit.point;
                    LookingAtAny = LookingAtWithCollision;
                }
                else
                {
                    LookingAtAny = LookingAt;
                }
            }
        }

        private void FixedUpdate()
        {
            if (MovementType == PlayerTopMovementType.ChangePosition)
            {
                GetComponent<Rigidbody>()
                    .MovePosition(transform.position + CurrentDirection * _speed * Time.fixedDeltaTime);
            }

            if (MovementType == PlayerTopMovementType.ChangeVelocity)
            {
                var v = GetComponent<Rigidbody>().velocity;
                v.x = CurrentDirection.x * _speed;
                v.z = CurrentDirection.z * _speed;
                GetComponent<Rigidbody>().velocity = v;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            var info = other.gameObject.GetComponent<ObjectInfo>();
            if (info && info.HasTag("Ground"))
            {
                /*if (GetComponent<Rigidbody>().velocity.y < -8)
                {
                    IsGround = true;
                    _groundCooldown = 0.4f;
                    
                    OnGround?.Invoke(2);
                } else
                if (GetComponent<Rigidbody>().velocity.y < -3)
                {
                    
                }*/

                //if (_lastFallSpeed < -GroundIfFallSpeedMoreThan)
                {
                    IsGround = true;
                    _groundCooldown = Math.Min(Math.Abs(_lastFallSpeed) / GroundIfFallSpeedMoreThan * 0.3f, 0.5f);
                    OnGround?.Invoke(Math.Abs(_lastFallSpeed) / GroundIfFallSpeedMoreThan * 15f);
                }
            }
        }

        private void OnCollisionStay(Collision other)
        {
            var info = other.gameObject.GetComponent<ObjectInfo>();
            if (info && info.HasTag("Ground"))
            {
                _jumpCount = 0;
            }
        }
    }
}