using System;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "SkinItem", menuName = "SkinItem", order = 0)]
    public class SkinItem : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private string m_SkinKey;
        [SerializeField] private Sprite m_Icon;
        [SerializeField] private int m_Price;

        [SerializeField,HideInInspector] private string m_Id;
        
        public string SkinKey => m_SkinKey;
        public Sprite Icon => m_Icon;
        public int Price => m_Price;

        public void UnlockItem()
        {
            PlayerPrefs.SetString(m_Id,"true");
        }

        public bool IsUnlocked()
        {
            if (PlayerPrefs.GetString(m_Id) == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(m_Id))
            {
                m_Id = Guid.NewGuid().ToString();
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssetIfDirty(this);
            }
#endif
        }
    }
}