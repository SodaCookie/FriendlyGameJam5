using UnityEngine;
using Melodrive.States;

namespace Melodrive.Triggers
{
    /**
     * The state trigger allows you to create State changes
     * based on colliding objects in the scene.
     */
    public class StateTrigger : MelodriveTrigger
    {
        public State enterState;
        public State exitState;

        override protected void TriggerEnter(Collider other)
        {
            if (enterState != null)
                md.SetStateOptions(enterState);
        }

        override protected void TriggerExit(Collider other)
        {
            if (exitState != null && exitState != enterState)
                md.SetStateOptions(exitState);
        }
    }
}
