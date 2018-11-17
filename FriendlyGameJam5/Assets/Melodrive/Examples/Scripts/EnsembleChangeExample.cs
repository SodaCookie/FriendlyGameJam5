using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Melodrive;
using Melodrive.Styles;

public class EnsembleChangeExample : MelodriveObject
{
    private Dropdown ensembleSelect;

    // Use this for initialization
    void Start ()
    {
        // Ensembles are dependent on Style, so we need to listen for when that changes
        if (md)
            md.CueChange += new MelodrivePlugin.CueChangeHandler(CueChange);

        ensembleSelect = GameObject.Find("EnsembleSelect").GetComponent<Dropdown>();
    }

    public void OnEnsembleChange()
    {
        if (md)
            md.SetEnsemble(md.GetEnsembleID(ensembleSelect.value));
    }

    public void UpdateEnsembleList()
    {
        string currentEnsemble = md.GetCurrentEnsemble();

        ensembleSelect.ClearOptions();
        int value = 0;
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        for (int i = 0; i < md.GetNumEnsembles(); i++)
        {
            Dropdown.OptionData item = new Dropdown.OptionData();
            string id = md.GetEnsembleID(i);
            item.text = md.GetEnsembleName(id);
            options.Add(item);
            if (id == currentEnsemble)
                value = i;
        }

        ensembleSelect.AddOptions(options);
        ensembleSelect.value = value;
    }

    private void CueChange(string cue, string seedName, Style style)
    {
        UpdateEnsembleList();
    }
}
