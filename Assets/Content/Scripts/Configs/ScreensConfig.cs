using System.Collections.Generic;
using System.Linq;
using Content.Scripts.UI.Base;
using Cysharp.Threading.Tasks;
using TriInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Content.Scripts.Configs
{
    [CreateAssetMenu(fileName = "ScreensConfig", menuName = "Configs/ScreensConfig")]
    public class ScreensConfig : Config
    {
        [field: SerializeField] public Transform Root { get; private set; }

        [TableList(Draggable = true, HideAddButton = false, HideRemoveButton = false, AlwaysExpanded = false)]
        [SerializeField] private List<AddressablePrefabByType<View>> _screens;
         
        public async UniTask<T> Load<T>() where T : View
        {
            var data = _screens.FirstOrDefault(d => d.Type == typeof(T));
            var handle = await Addressables.LoadAssetAsync<GameObject>(data.Asset.AssetGUID);
            var component = handle.GetComponent<T>();
            return component;
        }
    }
}