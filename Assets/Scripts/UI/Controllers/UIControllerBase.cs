using UI.Views;
using UnityEngine;

namespace UI.Controllers
{   
    /// <summary>
    /// Base class for all UI controllers in the MVC architecture.
    /// Handles the connection between the controller and its corresponding view.
    /// </summary>
    public class UIControllerBase : MonoBehaviour
    {
        /// <summary>
        /// Reference to the UI view associated with this controller.
        /// Should be assigned via the Inspector.
        /// </summary>
        [SerializeField] protected UIViewBase view;
        
        /// <summary>
        /// Registers this controller instance with the view when the script is awakened.
        /// </summary>
        protected void Awake() => view.SetController(this);
    }
}
