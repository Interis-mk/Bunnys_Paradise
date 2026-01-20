using UnityEngine;
using System;

namespace TopDown.Interaction.SceneLoading
{
    public class CallAfterDelay : MonoBehaviour
    {
        float delay;
        float age;
        Action action;

        // Will never call this frame, always the next frame at the earliest
        public static CallAfterDelay Create(float delay, Action action)
        {
            CallAfterDelay cad = new GameObject("CallAfterDelay").AddComponent<CallAfterDelay>();
            cad.delay = Mathf.Max(0f, delay);
            cad.action = action;
            cad.age = 0f;
            return cad;
        }

        void Update()
        {
            if (action != null && age > delay)
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }

                Destroy(gameObject);
            }
        }

        void LateUpdate()
        {
            age += Time.deltaTime;
        }
    }
}