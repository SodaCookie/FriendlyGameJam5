using UnityEngine;
using UnityEngine.UI;
using Melodrive;

public class DiscreteEmotionExample : MelodriveObject
{ 
    public void OnEmotionChange()
    {
        Dropdown emotionChange = GameObject.Find("EmotionSelect").GetComponent<Dropdown>();
        string emotion = emotionChange.options[emotionChange.value].text.ToLower();
        md.SetEmotion(emotion);
    }
}
