using UnityEngine;
using Melodrive.Ensembles;

namespace Melodrive.Triggers
{
    /**
     * The ensemble trigger allows you to create ensemble changes
     * based on colliding objects in the scene.
     */
    public class EnsembleTrigger : MelodriveTrigger
    {
        public Ensemble enterEnsemble;
        public Ensemble exitEnsemble;

        override protected void TriggerEnter(Collider other)
        {
            md.SetEnsemble(enterEnsemble);
        }

        override protected void TriggerExit(Collider other)
        {
            if (exitEnsemble != enterEnsemble)
                md.SetEnsemble(exitEnsemble);
        }
    }
}
