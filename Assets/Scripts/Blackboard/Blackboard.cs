using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shadee.Blackboards
{
    public class Blackboard
    {
        Dictionary<EBlackboardKey, int> IntValues = new Dictionary<EBlackboardKey, int>();
        Dictionary<EBlackboardKey, float> FloatValues = new Dictionary<EBlackboardKey, float>();
        Dictionary<EBlackboardKey, bool> BoolValues = new Dictionary<EBlackboardKey, bool>();
        Dictionary<EBlackboardKey, string> StringValues = new Dictionary<EBlackboardKey, string>();
        Dictionary<EBlackboardKey, Vector3> Vector3Values = new Dictionary<EBlackboardKey, Vector3>();
        Dictionary<EBlackboardKey, GameObject> GameObjectValues = new Dictionary<EBlackboardKey, GameObject>();
        Dictionary<EBlackboardKey, object> genericValues = new Dictionary<EBlackboardKey, object>();

        public void SetGeneric<T>(EBlackboardKey key, T value)
        {
            genericValues[key] = value;
        }

        public T GetGeneric<T>(EBlackboardKey key)
        {
            if(!genericValues.ContainsKey(key))
                throw new ArgumentException($"Blackboard does not contain key {key} in GenericValues");

            return (T)genericValues[key];
        }

        public bool TryGetGeneric<T>(EBlackboardKey key, out T value, T defaultValue = default)
        {
            if(!genericValues.ContainsKey(key))
            {
                value = defaultValue;
                return false;
            }

            value = (T)genericValues[key];
            return true;
        }

        public void Set(EBlackboardKey key, int value)
        {
            IntValues[key] = value;
        }

        public int GetInt(EBlackboardKey key)
        {
            if(!IntValues.ContainsKey(key))
                throw new ArgumentException($"Blackboard does not contain key {key} in IntValues");

            return IntValues[key];
        }

        public bool TryGet(EBlackboardKey key, out int value, int defaultValue = 0)
        {
            if(!IntValues.ContainsKey(key))
            {
                value = defaultValue;
                return false;
            }

            value = IntValues[key];
            return true;
        }

        public void Set(EBlackboardKey key, float value)
        {
            FloatValues[key] = value;
        }

        public float GetFloat(EBlackboardKey key)
        {
            if(!FloatValues.ContainsKey(key))
                throw new ArgumentException($"Blackboard does not contain key {key} in FloatValues");

            return FloatValues[key];
        }

        public bool TryGet(EBlackboardKey key, out float value, float defaultValue = 0.0f)
        {
            if(!FloatValues.ContainsKey(key))
            {
                value = defaultValue;
                return false;
            }

            value = FloatValues[key];
            return true;
        }

        public void Set(EBlackboardKey key, bool value)
        {
            BoolValues[key] = value;
        }

        public bool GetBool(EBlackboardKey key)
        {
            if(!BoolValues.ContainsKey(key))
                throw new ArgumentException($"Blackboard does not contain key {key} in BoolValues");

            return BoolValues[key];
        }

        public bool TryGet(EBlackboardKey key, out bool value, bool defaultValue = false)
        {
            if(!BoolValues.ContainsKey(key))
            {
                value = defaultValue;
                return false;
            }

            value = BoolValues[key];
            return true;
        }

        public void Set(EBlackboardKey key, string value)
        {
            StringValues[key] = value;
        }

        public string GetString(EBlackboardKey key)
        {
            if(!StringValues.ContainsKey(key))
                throw new ArgumentException($"Blackboard does not contain key {key} in StringValues");

            return StringValues[key];
        }

        public bool TryGet(EBlackboardKey key, out string value, string defaultValue = "")
        {
            if(!StringValues.ContainsKey(key))
            {
                value = defaultValue;
                return false;
            }

            value = StringValues[key];
            return true;
        }

        public void Set(EBlackboardKey key, Vector3 value)
        {
            Vector3Values[key] = value;
        }

        public Vector3 GetVector3(EBlackboardKey key)
        {
            if(!Vector3Values.ContainsKey(key))
                throw new ArgumentException($"Blackboard does not contain key {key} in Vector3Values");

            return Vector3Values[key];
        }

        public bool TryGet(EBlackboardKey key, out Vector3 value, Vector3 defaultValue = default)
        {
            if(!Vector3Values.ContainsKey(key))
            {
                value = defaultValue;
                return false;
            }

            value = Vector3Values[key];
            return true;
        }

        public void Set(EBlackboardKey key, GameObject value)
        {
            GameObjectValues[key] = value;
        }

        public GameObject GetGameObject(EBlackboardKey key)
        {
            if(!GameObjectValues.ContainsKey(key))
                throw new ArgumentException($"Blackboard does not contain key {key} in GameObjectValues");

            return GameObjectValues[key];
        }

        public bool TryGet(EBlackboardKey key, out GameObject value, GameObject defaultValue = null)
        {
            if(!GameObjectValues.ContainsKey(key))
            {
                value = defaultValue;
                return false;
            }

            value = GameObjectValues[key];
            return true;
        }


    }
}
