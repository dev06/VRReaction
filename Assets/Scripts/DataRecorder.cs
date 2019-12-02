using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class DataRecorder : MonoBehaviour
{
    private static string PATH = "";
    private float time;
    private bool _startTimer;

    void Awake()
    {
        PATH = Application.dataPath + "/VRReaction";
    }

    public void StartRecording()
    {
        _startTimer = true;
        StartCoroutine("IStartTimer");
    }

    IEnumerator IStartTimer()
    {
        time = 0;
        while (_startTimer)
        {
            time += Time.deltaTime;
            yield return null;
        }
    }

    public void StopRecording(string fileName)
    {
        int min = (int)time / 60;
        int sec = (int)time % 60;
        string t = getTime(min) + " " + getTime(sec);

        try
        {
            if (!Directory.Exists(PATH))
            {
                Directory.CreateDirectory(PATH);
            }

            string date = System.DateTime.Now.ToString().Replace("/", ".");
            date = date.Replace(" ", "_");
            date = date.Replace(":", ".");

            StreamWriter writer = new StreamWriter(PATH + "/" + fileName + "_" + date + ".txt", true);
            writer.WriteLine(t);
            writer.Close();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error writing to file " + e);
        }
    }

    public void WriteSignInfo(List<float> f, float totalSessionTime, string fileName)
    {
        try
        {
            if (!Directory.Exists(PATH))
            {
                Directory.CreateDirectory(PATH);
            }

            string date = System.DateTime.Now.ToString().Replace("/", ".");
            date = date.Replace(" ", "_");
            date = date.Replace(":", ".");
            StreamWriter writer = new StreamWriter(PATH + "/" + fileName + "_" + date + ".txt", true);

            for (int i = 0; i < f.Count; i++)
            {
                writer.WriteLine("Sign Post: " + (i + 1) + " | Time:  " + f[i]);
            }

            float[] info = getInfo(f);
            writer.WriteLine("Total Looking Time: " + info[0]);
            writer.WriteLine("Average Time: " + info[1]);
            writer.WriteLine("Time took to complete level: " + totalSessionTime);
            writer.Close();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error writing to file " + e);
        }
    }

    private float[] getInfo(List<float> f)
    {
        float[] info = new float[2];
        float average = 0;
        float sum = 0;
        for (int i = 0; i < f.Count; i++)
        {
            sum += f[i];
        }

        info[0] = sum;
        average = sum / f.Count;
        info[1] = average;
        return info;
    }


    private string getTime(int t)
    {
        return t < 10 ? "0" + (int)(t) : "" + (int)t;
    }

}