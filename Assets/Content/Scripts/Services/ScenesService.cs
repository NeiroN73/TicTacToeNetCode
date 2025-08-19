using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Content.Scripts.Services
{
    public class ScenesService : Service
    {
        public async UniTask LoadSceneAsync(string name, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            var scene = Addressables.LoadSceneAsync(name, loadSceneMode);
            await UniTask.WaitUntil(() => scene.IsDone);
        }
    }
}