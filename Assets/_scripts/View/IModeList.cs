using System.Reflection;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets._scripts.View
{
    public interface IModeList
    {
        /// <summary>
        /// Create a new entry in the dropdown list for each mode specified in the game manager
        /// </summary>
        void GetOptionList();
    }
}