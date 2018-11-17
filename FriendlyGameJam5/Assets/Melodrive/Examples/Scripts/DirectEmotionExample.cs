using UnityEngine;
using UnityEngine.UI;
using Melodrive;

public class DirectEmotionExample : MelodriveObject
{
    private Vector2 va;
    private bool changed = false;

	// Use this for initialization
	void Start () {
        va = new Vector2();
	}
	
	// Update is called once per frame
	void Update () {
        if (changed)
        {
            md.SetVA(va);
            changed = false;
        }
    }

    public void OnValenceChange()
    {
        Slider vSlider = GameObject.Find("ValenceSlider").GetComponent<Slider>();
        va.x = vSlider.value;
        changed = true;
    }

    public void OnArousalChange()
    {
        Slider aSlider = GameObject.Find("ArousalSlider").GetComponent<Slider>();
        va.y = aSlider.value;
        changed = true;
    }
}
