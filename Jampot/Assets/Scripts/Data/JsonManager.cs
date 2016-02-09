using UnityEngine;
using System.Collections;
using LitJson;

public class JsonManager : Singleton<JsonManager>
{

    private TextAsset jobText;
    private JsonData jobData;

    void Awake()
    {
        jobText = Resources.Load<TextAsset>("TextFile/JobsRequirement");
        jobData = JsonMapper.ToObject(jobText.text);
    }

    public int GetJobCount()
    {
        return jobData.Count;
    }
    public string GetJobName(int idx)
    {
        return jobData[idx]["Name"].ToString();
    }

    public int GetRequirement(int idx, int gaugeIdx)
    {
        return (int)jobData[idx]["Requirement"][gaugeIdx];
    }
}
