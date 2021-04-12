using System;
using System.Collections.Generic;
using UnityEngine;

namespace Public
{
    //可伤害单位的接口
    public interface IDamagable
    {
        void GetDamage(int damage);
        void Die();
    }
    //友方可伤害单位的接口
    public interface IDamagable_Friendly
    {
        void GetDamage(int damage);
        void Die();
    }
    public class CSigleton<T>:MonoBehaviour
        where T:CSigleton<T>
    {
        public static T Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = (T)this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }
    }
   
}
