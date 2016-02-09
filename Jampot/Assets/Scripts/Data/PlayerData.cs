using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


[Serializable]
public class JobInfo
{
    public string name;
    public bool isHave;
    public int highScore;
};

[Serializable]
public class PlayerSetting
{
    public bool sound;
    public bool bgm;
}


public class PlayerData : Singleton<PlayerData>
{

    #region Save data
    public List<JobInfo> jobList
    {
        get;
        set;
    }

    public PlayerSetting setting
    {
        get;
        set;
    }
    #endregion

    public int currScore
    {
        get;
        set;
    }

    public string selectedJob
    {
        get;
        set;
    }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Debug.Log("Fake Data Delete");
        }
        else// first 
        {
            DontDestroyOnLoad(gameObject);

            Load();
            selectedJob = null;
        }

    }


    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerData.dat");

        // Save Data
        bf.Serialize(file, jobList);
        file.Close();

    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            Debug.Log("Have File");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Open);

            // Load data
            jobList = (List<JobInfo>)bf.Deserialize(file);

            file.Close();
        }
        else
        {
            Debug.Log("no file");
            jobList = new List<JobInfo>();
            setting = new PlayerSetting();

            setting.sound = true;
            setting.bgm = true;

            for (int i = 0; i < JsonManager.Instance.GetJobCount(); i++)
                jobList.Add(InitJob(JsonManager.Instance.GetJobName(i), false, 0));
        }
    }

    JobInfo InitJob(string name, bool have, int highScore)
    {
        JobInfo jobinfo = new JobInfo();
        jobinfo.name = name;
        jobinfo.isHave = false;
        jobinfo.highScore = highScore;


        return jobinfo;
    }

    public bool IsHave(string name)
    {
        for (int i = 0; i < jobList.Count; i++)
        {
            if (jobList[i].name == name && jobList[i].isHave)
                return true;
        }
        return false;
    }
}
