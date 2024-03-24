using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Inventory.Model.ItemSO;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EdibleItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        [SerializeField]
        private List<ModifierData> modifiersData = new List<ModifierData>();

        public string ActionName => "Consume";

        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            foreach (ModifierData data in modifiersData)
            {
                data.statModifier.AffectCharacter(character, data.value);
            }

            return true;
        }

        //public AudioClip actionSFX { get; private set; }


    }


        public interface IDestroyableItem
        {

        }

        public interface IItemAction
        {
            public string ActionName { get; }
            //public AudioClip actionSFX { get; }

            bool PerformAction(GameObject character, List<ItemParameter> itemState);
        }

        [Serializable]

        public class ModifierData
        {
            public CharacterStatModifierSO statModifier;
            public float value;
        }
    

}

