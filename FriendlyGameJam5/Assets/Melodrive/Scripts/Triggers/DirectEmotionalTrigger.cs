using UnityEngine;

namespace Melodrive.Triggers
{
    /**
     * The direct emotional trigger allows you to create Valence/Arousal changes
     * based on colliding objects in the scene.
     */
    public class DirectEmotionalTrigger : MelodriveTrigger
    {
        [Range(-1.0f, 1.0f)]
        public float enterValence = 0.0f;
        [Range(-1.0f, 1.0f)]
        public float enterArousal = 0.0f;
        [Range(-1.0f, 1.0f)]
        public float exitValence = 0.0f;
        [Range(-1.0f, 1.0f)]
        public float exitArousal = 0.0f;

        override protected void TriggerEnter(Collider other)
        {
            md.SetVA(new Vector2(enterValence, enterArousal));
        }

        override protected void TriggerExit(Collider other)
        {
            if (exitValence != enterValence || exitArousal != enterArousal)
                md.SetVA(new Vector2(exitValence, exitArousal));
        }
    }
}
