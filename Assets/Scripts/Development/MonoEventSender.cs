using UnityEngine;

namespace Assets.Scripts.Development
{
    public static class MonoEventSender
    {
        private static MonoEvents events;

        static MonoEventSender()
        {
            CreateObject();
        }
        public static MonoEvents Events
        {
            get
            {
                if (events == null)
                {
                    CreateObject();
                }
                return events;
            }
            set
            {
                events = value;
            }
        }
        private static void CreateObject()
        {
            var go = new GameObject("MonoEventSender");
            go.transform.parent = DynamicSpawn.DontDestroyOnLoadParent;

            events = go.AddComponent<MonoEvents>();
        }
    }
}
