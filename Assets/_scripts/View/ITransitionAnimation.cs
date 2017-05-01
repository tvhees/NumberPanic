using System;
using System.Collections;
using RSG;

namespace View
{
    public interface ITransitionAnimation
    {
        IPromise ScreenEnterAnimation();
        IPromise ScreenExitAnimation();
        void SetActive(bool value);
    }
}