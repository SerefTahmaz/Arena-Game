using System;
using Item;
using UnityEngine;

public abstract class BaseItemSO : SerializableScriptableObject
{
    [SerializeField] protected string m_ItemName;
    [SerializeField] protected ItemType m_ItemType;
    
    public Action OnChanged { get; set; }
    
    public string ItemName => m_ItemName;
    public ItemType ItemType => m_ItemType;
    public abstract Sprite ItemSprite { get; }
    
    public virtual void Save()
    {
    }

    public virtual void Load()
    {
    }
}