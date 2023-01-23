using Others;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "MaterialsData", menuName = "ScriptableObjects/Materials/MaterialsData")]
    public class MaterialsData : ScriptableObject
    {
        [SerializeField] private List<MaterialSetting> _materials;

        public List<MaterialSetting> Materials
        {
            get => _materials;
            set => _materials = value;
        }
    }
}