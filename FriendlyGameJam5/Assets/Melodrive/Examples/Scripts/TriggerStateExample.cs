using UnityEngine;
using Melodrive;
using Melodrive.Styles;
using Melodrive.Emotions;
using Melodrive.Ensembles;

public class TriggerStateExample : MelodriveObject
{
    MelodriveListener listener;

    void Start()
    {
        md.Init(Style.House, Emotion.Happy);
        md.SetEnsemble(Ensemble.DreamyHouse);
        md.CreateMusicalSeed();
        md.Play();

        listener = FindMelodriveListener();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject != listener.gameObject)
                {
                    StateTriggerObject stateTrigger = listener.GetComponent<StateTriggerObject>();
                    stateTrigger.targetPosition = hit.collider.transform.position;
                    stateTrigger.state = StateTriggerObject.State.Move;
                }
            }
        }
    }
}
