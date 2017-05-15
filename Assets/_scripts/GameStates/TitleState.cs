﻿using Managers;
using RSG;

namespace GameStates
{
    public class TitleState : StateBase
    {
        public override IPromise StartState()
        {
            var animationManager = GetManager<AnimManager>();
            return Promise.Sequence(
                base.StartState,
                animationManager.Title.ScreenEnterAnimation,
                animationManager.Menu.ScreenEnterAnimation
            );
        }

        public override IPromise FinishState()
        {
            return Promise.Sequence(
                () => GetManager<AnimManager>().SubMenus[0].ScreenExitAnimation(),
                () => GetManager<AnimManager>().Menu.ScreenExitAnimation(),
                () => GetManager<AnimManager>().Title.ScreenExitAnimation(),
                () => base.FinishState()
            );
        }
    }
}