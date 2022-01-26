using BetterObsideo.Utility;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BetterObsideo.MonoBehaviours
{
    public class DebuggerController : MonoBehaviour
    {
        private static DebuggerController instance;
        public static DebuggerController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject(typeof(DebuggerController).Name).AddComponent<DebuggerController>();
                }
                return instance;
            }
        }

        public struct Message
        {
            public bool Prefix;
            public string Text;
        }

        private IDictionary<string, Message> dictionary;
        public IDictionary<string, Message> Dictionary
        {
            get
            {
                if (dictionary == null)
                {
                    dictionary = new SortedDictionary<string, Message>();
                }
                return dictionary;
            }
        }

        private IList<Message> list;
        public IList<Message> List
        {
            get
            {
                if (list == null)
                {
                    list = new List<Message>();
                }
                return list;
            }
        }

        public GUIStyle Style { get; set; }
        public Rect Position { get; set; }

        public string Text
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var item in Dictionary)
                {
                    sb.AppendLine(DebuggerUtility.GenerateString(item.Value.Text, item.Key, item.Value.Prefix));
                }
                if (Dictionary.Count > 0)
                {
                    sb.AppendLine();
                }
                foreach (var item in List)
                {
                    sb.AppendLine(DebuggerUtility.GenerateString(item.Text, null, item.Prefix));
                }
                return sb.ToString();
            }
        }

        public void Awake()
        {
            Style = new GUIStyle
            {
                fontSize = 18,
                clipping = TextClipping.Overflow
            };

            Style.normal.textColor = Color.white;

            Position = new Rect(0, 0, 0, 0);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                ClearMessages();
            }
        }

        public void OnGUI()
        {
            GUI.Label(Position, Text, Style);
        }

        public void ClearMessages()
        {
            Dictionary.Clear();
            List.Clear();
        }

        public void AddMessage(Message message, string key = null)
        {
            if (key != null)
            {
                Dictionary[key] = message;
            }
            else
            {
                List.Add(message);
            }
        }
    }
}
