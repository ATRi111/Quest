//using System;
using System.Collections;
using UnityEngine;

namespace Public
{
    public static class CTool
    {
        public static Quaternion s_ZeroQuaternion = new Quaternion();
        public static GameObject FindFromUI(string name) => GameObject.Find("UI").transform.Find(name).gameObject;
        public static IEnumerator Wait(float duration)
        {
            for (float timer = 0; timer < duration; timer += Time.deltaTime)
                yield return null;
        }
        public static Vector2 RandomVector2()
            => new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
        public static Vector3 RandomVector3()
            => new Vector3(Random.Range(-1, 1), Random.Range(-1, 1),0);
        public static Vector2 Angle2Direction(float angle)
        => new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
        public static float Direction2Angle(Vector2 direction)
            => Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
    }
    //可伤害单位的接口
    public interface IDamagable
    {
        void GetDamage(int damage);
    }
    //友方可伤害单位的接口
    public interface IDamagable_Friendly
    {
        void GetDamage(int damage);
    }
    public class CSigleton<T> : MonoBehaviour
        where T:CSigleton<T>
    {
        public static T Instance { get; private set; }
        protected virtual void Awake()
        {
            Instance = (T)this;
            DontDestroyOnLoad(gameObject);
        }
        protected virtual void OnSceneLoaded(int index){}
    }

}
