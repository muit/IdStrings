using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Runtime.Serialization;

using UnityEngine;
using UnityEditor;


namespace IdStrings
{
    [Serializable, DataContract]
    public struct IdString : IEquatable<IdString>, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector]
        private ulong hash;

        [SerializeField]
        private string serializedValue;


        public IdString(string str = "")
        {
            hash = 0;
            serializedValue = null;
            Set(str);
        }

        public void Set(string str)
        {
            hash = IdTable.Register(str);
        }

        public bool IsNone() { return hash == 0; }
        public override string ToString() { return IdTable.Find(hash); }
        
        public static bool operator ==(IdString a, IdString b) { return a.Equals(b); }
        public static bool operator !=(IdString a, IdString b) { return !a.Equals(b); }

        public bool Equals(IdString other) { return hash == other.hash; }
        public override bool Equals(object o)
        {
            return (o is IdString) && Equals((IdString)o);
        }

        public override int GetHashCode() { return (int)hash; }

        public static implicit operator IdString(string str)
        {
            return new IdString(str);
        }
        public static explicit operator string(IdString id) { return id.ToString(); }

        // Serializing as an string
        [DataMember]
        string TypeString
        {
            get { return (hash == 0) ? null : ToString(); }
            set
            {
                Debug.Log(value);
                if (value == null)
                    hash = 0;
                else
                    Set(value);
            }
        }

        public void OnBeforeSerialize()
        {
            serializedValue = ToString();
        }

        public void OnAfterSerialize()
        {
            serializedValue = null;
        }
        
        public void OnAfterDeserialize()
        {
            Set(serializedValue);
            serializedValue = null;
        }
    }

    struct IdTable
    {
        public static ulong Register(string str)
        {
            if (str == null)
                return 0;

            byte[] chars = Encoding.ASCII.GetBytes(str);
            return RegisterString(chars, chars.Length);
        }

        public static string Find(ulong hash)
        {
            if (hash == 0)
                return "";

            int size;
            IntPtr ptr = FindString(hash, out size);
            return Marshal.PtrToStringAnsi(ptr, size);
        }

        // DLL Imports
        [DllImport("IdStrings")]
        private static extern ulong RegisterString(byte[] charPtr, int length);

        [DllImport("IdStrings", CharSet = CharSet.Ansi)]
        private static extern IntPtr FindString(ulong hash, out int size);
    }


    /** EDITOR */

    [CustomPropertyDrawer(typeof(IdString))]
    public class IdStringDrawer : PropertyDrawer
    {
        SerializedProperty hash;
        SerializedProperty serializedValue;
        bool cache = false;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!cache)
            {
                hash = property.FindPropertyRelative("hash");
                serializedValue = property.FindPropertyRelative("serializedValue");
                cache = true;
            }

            UInt64 hashValue = (ulong)hash.longValue;
            string strValue = IdTable.Find(hashValue);


            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            
            var strRect = new Rect(position.x, position.y, position.width - 90, position.height);
            string newValue = EditorGUI.TextField(strRect, strValue);

            // If value changed, assign the hash
            if (!strValue.Equals(newValue))
            {
                serializedValue.stringValue = newValue;
            }

            EditorGUI.EndProperty();
        }
    }
}
