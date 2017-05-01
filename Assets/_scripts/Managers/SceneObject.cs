using UnityEngine;

namespace Managers
{
    [ManagerDependency(typeof(ManagerContainer))]
    public class SceneObject : BaseMonoBehaviour
    {
        [SerializeField] private AnimManager.SceneObjects animationObjects;

        protected override void Awake()
        {
            base.Awake();
            GetManager<AnimManager>().SetObjects(animationObjects);
        }
    }
}