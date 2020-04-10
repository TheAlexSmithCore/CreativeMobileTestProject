using System.IO;
using System.Security;
using UnityEngine;

public class JsonReadWrite
{
    public static string GetJsonString(string path) {
        StreamReader strRead = new StreamReader(path);
        return strRead.ReadToEnd();
    }

    public static void CreateJsonData(string path, string content) {
        if(File.Exists(path)) { Debug.Log("File Exists!"); return; }
        File.WriteAllText(path, content);
        Debug.Log("File Created!");
    }

    public static void SetJsonString(string path, string value) {
        File.WriteAllText(path, value);
    }
}
