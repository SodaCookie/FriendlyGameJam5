using UnityEngine;
using UnityEngine.UI;
using Melodrive;
using Melodrive.Styles;

public class StyleChangeExample : MelodriveObject
{
    private void Start()
    {
        md.Init(Style.Ambient, Melodrive.Emotions.Emotion.Happy);

        // When changing styles, we reccomend they are preloaded first to avoid any audio jitters
        md.PreloadStyles();
        md.Play();
    }

    public void OnStyleChange()
    {
        Dropdown styleChange = GameObject.Find("StyleSelect").GetComponent<Dropdown>();
        string style = styleChange.options[styleChange.value].text.ToLower();
        md.SetStyle(style);
    }
}
