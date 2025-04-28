using UI.Views;
using UnityEngine;

namespace UI.Controllers
{
    public class UIControllerBase : MonoBehaviour
    {
        [SerializeField] protected UIViewBase view;
        protected void Awake() => view.SetController(this);
    }
}
