using System;
using Danmaku.Util;
using UnityEngine;

namespace Danmaku.Character
{
    public enum EnemyState
    {
        Idle,
        Prepare,
        Active,
        Away
    }

    public class DanmakuEnemy : MonoBehaviour
    {
        private EnemyState _state;

        public float activeTime;
        public float prepareTime;
        
        // public Action OnActive;
        public Action OnPrepare;
        public Vector3 awayPosition;

        public EnemyState State
        {
            get => _state;
            set
            {
                _state = value;
                if (value == EnemyState.Prepare)
                {
                    OnPrepare?.Invoke();
                }

                if (value == EnemyState.Active)
                {
                    GetComponent<DanmakuSpawner>().isActive = true;
                }

                if (value == EnemyState.Away)
                {
                    GetComponent<DanmakuSpawner>().isActive = false;
                }
            }
        }
    }
}