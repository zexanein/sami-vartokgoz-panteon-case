using UI.Controllers;
using UnityEngine;

namespace UI.Views
{
    /// <summary>
    /// Base class for all UI views in the MVC architecture.
    /// Handles the connection between the view and its corresponding controller.
    /// </summary>
    public class UIViewBase : MonoBehaviour
    {
        /// <summary>
        /// Reference to the UI controller associated with this view.
        /// </summary>
        protected  UIControllerBase Controller;
        
        /// <summary>
        /// Sets the controller for this view.
        /// </summary>
        /// <param name="controller"></param>
        public void SetController(UIControllerBase controller) => Controller = controller;
    }
}