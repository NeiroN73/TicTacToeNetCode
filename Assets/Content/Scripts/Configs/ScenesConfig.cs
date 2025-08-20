using System.Collections.Generic;
using Content.Scripts.Configs.Base;
using TriInspector;
using UnityEngine;

namespace Content.Scripts.Configs
{
    [CreateAssetMenu(fileName = "ScenesConfig", menuName = "Configs/ScenesConfig")]
    public class ScenesConfig : Config
    {
        [Scene][SerializeField] private List<string> _scenes;
        public IReadOnlyList<string> Scenes => _scenes;
    }
}