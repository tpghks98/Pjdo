using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


[Serializable]
public class JobInfo
{
    public string   name;
    public bool     isHave;
    public int      highScore;
    public bool     isFirst;
};

//[System.Serializable]
//public struct PlayerSetting
//{
//    bool sound;
//    bool bgm;
//}


public class PlayerData : MonoBehaviour
{

    #region Singleton

    private static PlayerData instance;

    public static PlayerData getInstance
    {
        get
        {
            if (instance == null)
            {
                GameObject newObj = new GameObject("Player Data");
                instance    =   newObj.AddComponent<PlayerData>();
            }
            return instance;
        }
    }
    #endregion

    [HideInInspector]
    public List<JobInfo> jobList;

    public string selectedJob
    {
        get;
        set;
    }
    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            Debug.Log("Fake Data Delete");
        }
        else// first 
        {
            DontDestroyOnLoad(gameObject);
            Debug.Log("First");

            Load();
            selectedJob =   null;
            instance    =   this;
        }

    }


    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerData.dat");
        
        // Save Data
        bf.Serialize(file,jobList);
        file.Close();

    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Open);

            // Load data
            jobList = (List<JobInfo>)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            jobList = new List<JobInfo>();

            // TODO : json 에서 불러와야됨..; 순서대로
            jobList.Add(InitJob("Doctor", false, 0));
            jobList.Add(InitJob("Sportsman", false, 0));
            jobList.Add(InitJob("Teacher", false, 0));
            jobList.Add(InitJob("Writer", false, 0));
            jobList.Add(InitJob("Firefighter", false, 0));
            jobList.Add(InitJob("President", false, 0));
        }
    }

    JobInfo InitJob(string name, bool have, int highScore)
    {
        JobInfo jobinfo     =   new JobInfo();
        jobinfo.name        =   name;
        jobinfo.isHave      =   false;
        jobinfo.highScore   =   highScore;
        jobinfo.isFirst     =   true;

        return jobinfo;
    }

}
