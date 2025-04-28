using UI.Controllers;
using UnityEngine;

namespace UI.Views
{
    public class UIViewBase : MonoBehaviour
    {
        protected  UIControllerBase Controller;
        public void SetController(UIControllerBase controller) => Controller = controller;
    }
}