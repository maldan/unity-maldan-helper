using System;
using System.Collections.Generic;
using System.Linq;
using Helper;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace Spawn
{
    [Serializable]
    public class EnemyList
    {
        [SerializeField] public GameObject enemy;
        [SerializeField, Range(0f, 1f)] public float amount;
    }
         
    public class SpawnEnemy : MonoBehaviour
    {
        public float delay;
        public int maxEnemyAmount;
        public EnemyList[] enemy; 
        public bool isActive;
        public Vector3[] spawnPoint;
        public Vector3 spawnArea = Vector3.zero;
        public Vector3 spawnAreaOffset;
        public Action<GameObject> onSpawn;
        //public Color color;
        //public bool isSpawnOnce;
        
        private float _timer;
        private List<GameObject> _enemyList = new List<GameObject>();

        private void Start()
        {
            /*if (isSpawnOnce)
            {
                for (var i = 0; i < maxEnemyAmount; i++)
                {
                    Spawn();
                }
            }
            
            Destroy(gameObject);*/
        }

        private void OnDrawGizmos()
        {
            for (var i = 0; i < spawnPoint.Length; i++)
            {
                Gizmos.DrawSphere(transform.position + spawnPoint[i], 0.3f);
            }
            
            Gizmos.DrawWireCube(transform.position + spawnAreaOffset, spawnArea);
            
            /*var oldColor = Gizmos.color;
            Gizmos.color = color;
            
            var children = gameObject.GetChilds("");
            foreach (var child in children)
            {
                Gizmos.DrawWireCube(child.transform.position, child.transform.localScale);
            }
            Gizmos.color = oldColor;*/
        }

        public void Spawn()
        {
            // Clear dead enemy
            for (var i = 0; i < _enemyList.Count; i++)
            {
                if (_enemyList[i]) continue;
                _enemyList.RemoveAt(i);
                i -= 1;
            }
            
            if (_enemyList.Count >= maxEnemyAmount)
            {
                return;
            }
            
            var selectedEnemy = enemy[enemy.Length - 1].enemy;
            
            // Try to find enemy to spawn
            for (var x = 0; x < 1024; x++)
            {
                // Get random id
                var enemyId = Random.Range(0, enemy.Length);
                var amount = 0;
                    
                // Count current amount of that enemy
                for (var i = 0; i < _enemyList.Count; i++)
                {
                    if (_enemyList[i].GetComponent<ObjectInfo>().id == enemyId)
                    {
                        amount += 1;
                    }
                }

                // Amount is OK
                if (amount / (float)maxEnemyAmount <= enemy[enemyId].amount)
                {
                    selectedEnemy = enemy[enemyId].enemy;
                    break;
                }
            }
            
            /*var children = gameObject.GetChilds("");
            if (children.Count > 0)
            {
                return;
            }*/

            if (spawnArea != Vector3.zero)
            {
                var e = Instantiate(selectedEnemy, transform.position + spawnAreaOffset + spawnArea.Random(), Quaternion.identity);
                e.AddComponent<ObjectInfo>();
                _enemyList.Add(e);
                onSpawn?.Invoke(e);
                return;
            }
            
            if (spawnPoint.Length > 0)
            {
                var e = Instantiate(selectedEnemy, transform.position + spawnPoint[Random.Range(0, spawnPoint.Length - 1)], Quaternion.identity);
                e.AddComponent<ObjectInfo>();
                _enemyList.Add(e);
                onSpawn?.Invoke(e);
            }
            else
            {
                var e = Instantiate(selectedEnemy, transform.position, Quaternion.identity);
                e.AddComponent<ObjectInfo>();
                _enemyList.Add(e);
                onSpawn?.Invoke(e);
            }
        }
        
        private void Update()
        {
            if (!isActive)
            {
                return;
            }
            
            _timer += Time.deltaTime;
            if (_timer > delay)
            {
                _timer = 0;

                // Clear dead enemy
                for (var i = 0; i < _enemyList.Count; i++)
                {
                    if (_enemyList[i]) continue;
                    _enemyList.RemoveAt(i);
                    i -= 1;
                }
                
                if (_enemyList.Count >= maxEnemyAmount)
                {
                    return;
                }
               
                Spawn();
                
                /*var eList = enemy.OrderBy(x => x.priority).ToList();
                var priority = Random.Range(0f, 1f);
                var selectedEnemy = enemy[eList.Count - 1].enemy;
                
                Debug.Log("PP " + priority);
                
                for (var i = 0; i < eList.Count; i++)
                {
                    if (priority < eList[i].priority)
                    {
                        selectedEnemy = eList[i].enemy;
                        Debug.Log(selectedEnemy.gameObject.name);
                        break;
                    }
                }*/

                // Get priority

                /*var priority = Random.Range(0, 1f);
                for (var i = 0; i < enemyPrioirty.Length; i++)
                {
                    if (enemyPrioirty[i] < priority)
                    {
                                
                    }
                }*/
            }
        }
    }
}