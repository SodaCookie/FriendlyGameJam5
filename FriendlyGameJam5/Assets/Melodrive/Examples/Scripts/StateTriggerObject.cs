using UnityEngine;

public class StateTriggerObject : MonoBehaviour
{
    [Range(0.1f, 5.0f)]
    public float speed = 1.0f;
    public Vector3 targetPosition = new Vector3();

    public enum State
    {
        Wait,
        Move
    }

    public State state = State.Wait;

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update () {
        if (state == State.Move)
        {
            if (transform.position == targetPosition)
            {
                state = State.Wait;
            }
            else
            {
                // Move towards our target
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            }
        }
    }
}
