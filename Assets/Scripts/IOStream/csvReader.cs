using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

// Read함수
// csv파일을 읽어와서 string상태로 List에 저장 후 리턴한다.
// 1. 한줄한줄읽으면서 맨앞이 #이면 ',' 단위로 나누어 subject 배열에 담는다.
// 2. #이아닐경우, ',' 단위로 나누어 Dictionary에 subject와 그에맞는 값으로 담는다.
// 3. Dictionary에 담은값을 List에 담아주고 리턴한다.
public class csvReader : MonoBehaviour
{
    private UnityWebRequest request;
    private string sourceFileText;
    private bool isDownLoading = false;
    public bool IsDownLoading { get { return isDownLoading; } }

    public void csvReaderInit(string csvFileURL)
    {
        StartCoroutine(DownLoadCSV(csvFileURL));
    }

    private string ExtractFileID(string url)
    {
        url = url.Replace("https://drive.google.com/file/d/", "");
        url = url.Replace("/view?usp=sharing", "");

        var prefix = "http://drive.google.com/uc?export=view&id=";
        url = prefix + url;

        return url;
    }

   public IEnumerator DownLoadCSV(string fileURL)
    {
        var filePath = ExtractFileID(fileURL);

        request = UnityWebRequest.Get(filePath);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            sourceFileText = request.downloadHandler.text;
            Debug.Log(request.downloadHandler.text);
            isDownLoading = true;
        }
    }

    public List<Dictionary<string, string>> Read(string fileURL)
    {
        StringReader sr = new StringReader(sourceFileText);
        List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();

        List<string> subject = new List<string>();

        while (true)
        {
            string line = sr.ReadLine();
            if (line == null) break;

            if(line.IndexOf("#") == 0)
            {
                string[] _subject = line.Split(',');
                for (int i = 0; i < _subject.Length; ++i)
                {
                    subject.Add(_subject[i]);
                }
                subject[0] = subject[0].Substring(1);
                continue;
            }

            var value = line.Split(',');
            Dictionary<string, string> dic = new Dictionary<string, string>();
            for(int i =0; i<value.Length; ++i)
            {
                dic.Add(subject[i], value[i]);
            }
            list.Add(dic);
        }
        sr.Close();
        return list;
    }

    public List<string> ReadSubject(string fileURL)
    {
        StringReader sr = new StringReader(sourceFileText);
        List<string> subject = new List<string>();

        while (true)
        {
            string line = sr.ReadLine();
            if (line == null) break;

            if (line.IndexOf("#") == 0)
            {
                string[] _subject = line.Split(',');
                for (int i = 0; i < _subject.Length; ++i)
                {
                    subject.Add(_subject[i]);
                }
                subject[0] = subject[0].Substring(1);
                sr.Close();
                return subject;
            }
        }
        return subject;
    }
}
