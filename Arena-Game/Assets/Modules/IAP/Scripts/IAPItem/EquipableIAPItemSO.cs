using System.Collections.Generic;
using ArenaGame.Currency;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "EquipableIAPItemSO", menuName = "Game/IAP/EquipableIAPItemSO", order = 0)]
    public class EquipableIAPItemSO : BaseIAPItemSO
    {
        [SerializeField] private List<ArmorItemSO> m_ArmorItemSOs;

        public override void GiveReward()
        {
            base.GiveReward();
            var playerCharacter = GameplayStatics.GetPlayerCharacterSO();
            foreach (var armorItemSo in m_ArmorItemSOs)
            {
                var insArmor = armorItemSo.DuplicateUnique();
                insArmor.Save();
                playerCharacter.GetCharacterSave().AddInventory(insArmor);
                playerCharacter.GetCharacterSave().EquipItem(insArmor);
            }
        }
    }
}