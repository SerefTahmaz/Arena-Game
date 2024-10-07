using System;
using ArenaGame.Managers.SaveManager;
using DefaultNamespace;
using Gameplay.Item;
using Item;
using UnityEngine;
using UnityEngine.UI;

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

[CreateAssetMenu(fileName = "Armor Item", menuName = "Game/Item/Armor Item", order = 0)]
public class ArmorItemSO : BaseItemSO
{
    [SerializeField] private int m_Level;
    [SerializeField] private int m_NextLevelIncrement;
    [SerializeField] private ArmorItemTemplate m_ArmorItemTemplate;

    public int Level
    {
        get => m_Level;
        set => m_Level = value;
    }

    public ArmorItemTemplate ItemTemplate
    {
        get => m_ArmorItemTemplate;
        set => m_ArmorItemTemplate = value;
    }

    public ArmorType ArmorType => m_ArmorItemTemplate.ArmorType;

    public int NextLevelIncrement
    {
        get => m_NextLevelIncrement;
        set => m_NextLevelIncrement = value;
    }

    public override Sprite ItemSprite => m_ArmorItemTemplate.ItemSprite;

    public override void Save()
    {
        base.Save();
        ItemSaveHandler.Load();
        if (!ItemSaveHandler.SaveData.ArmorItems.ContainsKey(Guid.ToHexString()))
        {
            ItemSaveHandler.SaveData.ArmorItems.Add(Guid.ToHexString(), new ArmorItem());
        }
            
        ItemSaveHandler.SaveData.ArmorItems[Guid.ToHexString()].m_Level = m_Level;
        ItemSaveHandler.SaveData.ArmorItems[Guid.ToHexString()].m_NextLevelIncrement = NextLevelIncrement;
        
        ItemSaveHandler.SaveData.ArmorItems[Guid.ToHexString()].m_ItemName = m_ItemName;
        ItemSaveHandler.SaveData.ArmorItems[Guid.ToHexString()].m_ItemType = m_ItemType;

        ItemSaveHandler.SaveData.ArmorItems[Guid.ToHexString()].m_ArmorItemTemplateGUID = m_ArmorItemTemplate ? m_ArmorItemTemplate.Guid.ToHexString() : "";
            
        ItemSaveHandler.Save();
        OnChanged?.Invoke();
    }

    public override void Load()
    {
        base.Load();
        ItemSaveHandler.Load();
        if (ItemSaveHandler.SaveData.ArmorItems.ContainsKey(Guid.ToHexString()))
        {
            m_Level = ItemSaveHandler.SaveData.ArmorItems[Guid.ToHexString()].m_Level;
            m_NextLevelIncrement = ItemSaveHandler.SaveData.ArmorItems[Guid.ToHexString()].m_NextLevelIncrement;

            m_ItemName = ItemSaveHandler.SaveData.ArmorItems[Guid.ToHexString()].m_ItemName;
            m_ItemType = ItemSaveHandler.SaveData.ArmorItems[Guid.ToHexString()].m_ItemType;

            var templateItemGuid =
                ItemSaveHandler.SaveData.ArmorItems[Guid.ToHexString()].m_ArmorItemTemplateGUID;
            m_ArmorItemTemplate = ItemListSO.GetItemByGuid<ArmorItemTemplate>(templateItemGuid);
        }
    }

    public void IncreaseLevelIncrement()
    {
        Load();
        NextLevelIncrement++;

        if (NextLevelIncrement >= 6)
        {
            NextLevelIncrement = 0;
            Level++;
        }
        Save();
    }
}