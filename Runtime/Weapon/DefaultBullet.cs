using System;
using UnityEngine;
using Util;

namespace Weapon
{
    public class DefaultBullet : MonoBehaviour
    {
        private Vector3 _targetPosition;
        private float _speed;
        private float _damage;
        private float _repulsiveForce;
        protected Vector3 Direction;
        private bool _useDirection;
        protected GameObject User;
             
        public Action<GameObject, Vector3, Vector3> OnDestroy;
        
        public virtual void Init(Vector3 target, float speed, float damage, float repulsiveForce, GameObject user, bool useDirection)
        {
            User = user;
            _targetPosition = target;
            _useDirection = useDirection;
            if (_useDirection)
            {
                Direction =  (_targetPosition - transform.position).normalized;
                GetComponent<Rigidbody>().velocity = Direction * speed;
            }

            _speed = speed;
            _damage = damage;
            _repulsiveForce = repulsiveForce;
            
            transform.LookAt(target);
        }

        protected virtual void TrajectoryUpdate()
        {
            if (_useDirection)
            {
                GetComponent<Rigidbody>().velocity = Direction * _speed;
                // transform.position += Direction * Time.deltaTime * _speed;
            }
            else
            {
                /*transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _speed);
                if (Vector3.Distance(transform.position, _targetPosition) <= 0.01f)
                {
                    Destroy(gameObject);
                    var ch = gameObject.GetChilds("");
                    foreach (var t in ch) t.transform.SetParent(null);
                    
                    OnDestroy?.Invoke(transform.position, Vector3.zero);
                }*/
            }
        }

        protected virtual void Refresh()
        {
            TrajectoryUpdate();
        }
        
        private void FixedUpdate()
        {
            Refresh();
        }

        protected virtual void DestroyBullet(GameObject withWhomCollide, Vector3 normal)
        {
            OnDestroy?.Invoke(withWhomCollide, transform.position, normal);
            Destroy(gameObject);  
        }

        protected virtual void DamageCollidedObject(GameObject withWhomCollide)
        {
            if (withWhomCollide.GetComponent<Damagable>()) withWhomCollide.GetComponent<Damagable>().Damage(_damage, User);
        }
        
        protected virtual void UnParentBulletChildren()
        {
            // Un parent all children
            var ch = gameObject.GetChilds("");
            foreach (var t in ch) t.transform.SetParent(null);
        }
        
        protected virtual void AddForceToCollidedObject(GameObject withWhomCollide)
        {
            if (withWhomCollide.GetComponent<Rigidbody>())
                withWhomCollide.GetComponent<Rigidbody>().AddForce(Direction * _repulsiveForce, ForceMode.Impulse);
        }
        
        protected virtual void OnCollision(GameObject withWhomCollide, Vector3 normal)
        {
            UnParentBulletChildren();
            AddForceToCollidedObject(withWhomCollide);
            DamageCollidedObject(withWhomCollide);
            DestroyBullet(withWhomCollide, normal);
        }
        
        private void OnCollisionEnter(Collision other)
        {
            OnCollision(other.gameObject, other.contacts[0].normal);
        }
    }
}