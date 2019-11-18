﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetsFetcher : MonoBehaviour {
    public string spreadSheetId;
    public string tabId;

    private static string filePath = "Assets/Resources/Downloaded Spreadsheets/";

    public static Dictionary<string, List<string>> dataDictionary;

    public static GoogleSheetsFetcher instance;

    private static bool isDownloading = false;
    private void Awake() {
        if (!instance) {
            instance = this;
        } else {
            Destroy(instance);
        }
    }

    public void Start() {
        //Initialize();
    }

    public void Initialize(string _tabId = null) {
        dataDictionary = new Dictionary<string, List<string>>();

        if (!string.IsNullOrEmpty(_tabId))
        {
            tabId = _tabId;
        }

        Action<string> commCallback = (csv) => {
            // Load data here
            Debug.Log("Callback: " + csv);

            List<List<string>> parsedCsv = ParseCSV(csv);

            // Go through first row to get keys
            for (int col = 0; col < parsedCsv[0].Count; col++) {
                string columnName = parsedCsv[0][col];

                List<string> columnData = new List<string>();

                for (int row = 1; row < parsedCsv.Count; row++) {
                    columnData.Add(parsedCsv[row][col]);
                }

                dataDictionary.Add(columnName, columnData);
            }

            //NodePopulator.instance.SetNodesDatabase(dataDictionary);
        };

        StartCoroutine(DownloadCSVCoroutine(spreadSheetId, commCallback, true, "Wine", tabId));
    }

    public static List<string> GetDataByColumn(string columnName) {
        return dataDictionary[columnName];
    }

    public static void PrintDataByColumn(string columnName) {
        for (int i = 0; i < dataDictionary[columnName].Count; i++) {
            Debug.Log(dataDictionary[columnName][i]);
        }
    }

    public static List<List<string>> ParseCSV(string text) {
        text = CleanReturnInCsvTexts(text);

        var list = new List<List<string>>();
        var lines = Regex.Split(text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);

        bool jumpedFirst = false;

        /*
        foreach (var line in lines) {
            // We don't want to jump the header
            /*
            if (!jumpedFirst) {
                jumpedFirst = true;
                continue;
            }
            */
            /*
            var values = Regex.Split(line, SPLIT_RE);

            var entry = new List<string>();
            for (var j = 0; j < header.Length && j < values.Length; j++) {
                var value = values[j];
                value = DecodeSpecialCharsFromCSV(value);
                entry.Add(value);
            }
            list.Add(entry);
        }
        */

        for (int i = 0; i < lines.Length; i++) {
            var values = Regex.Split(lines[i], SPLIT_RE);

            var entry = new List<string>();
            for (var j = 0; j < header.Length && j < values.Length; j++) {
                var value = values[j];
                value = DecodeSpecialCharsFromCSV(value);
                entry.Add(value);
            }
            list.Add(entry);
        }

        return list;
    }

    public static string CleanReturnInCsvTexts(string text) {
        text = text.Replace("\"\"", "'");

        if (text.IndexOf("\"") > -1) {
            string clean = "";
            bool insideQuote = false;
            for (int j = 0; j < text.Length; j++) {
                if (!insideQuote && text[j] == '\"') {
                    insideQuote = true;
                } else if (insideQuote && text[j] == '\"') {
                    insideQuote = false;
                } else if (insideQuote) {
                    if (text[j] == '\n')
                        clean += "<br>";
                    else if (text[j] == ',')
                        clean += "<c>";
                    else
                        clean += text[j];
                } else {
                    clean += text[j];
                }
            }
            text = clean;
        }
        return text;
    }

    private static void CompleteDownload(string csv) {
        Debug.Log("Complete download");
        
        isDownloading = false;

        List<List<string>> parsedCsv = ParseCSV(csv);

        // Go through first row to get keys
        for (int col = 0; col < parsedCsv[0].Count; col++) {
            string columnName = parsedCsv[0][col];

            List<string> columnData = new List<string>();

            for (int row = 1; row < parsedCsv.Count; row++) {
                columnData.Add(parsedCsv[row][col]);
            }

            dataDictionary.Add(columnName, columnData);

            Debug.Log("Adding column name: " + columnName);
        }

        DataNodePopulator.instance.SetNodesDatabase(dataDictionary);
    }

    public static IEnumerator DownloadCSVCoroutine(string docId, Action<string> callback,
                                                   bool saveAsset = false, string assetName = null, string sheetId = null) {

        if (isDownloading)
        {
            yield return null;
        }

        isDownloading = true;

        string url =
            "https://docs.google.com/spreadsheets/d/" + docId + "/export?format=csv";

        if (!string.IsNullOrEmpty(sheetId))
            url += "&gid=" + sheetId;

        using (UnityWebRequest www = UnityWebRequest.Get(url)) {
            UnityWebRequestAsyncOperation asyncWebRequest = www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            } else {
                while (!asyncWebRequest.isDone) {
                    Debug.Log("Download progress: " + asyncWebRequest.progress);
                    yield return null;
                }

                Debug.Log("Download complete");

                Debug.Log(www.downloadHandler.text);

                CompleteDownload(www.downloadHandler.text);

                /*
                if (saveAsset) {
                    if (!string.IsNullOrEmpty(assetName)) {
                        File.WriteAllText(filePath + assetName + ".csv", www.downloadHandler.text);
                    } else {
                        throw new Exception("assetName is null");
                    }
                }
                */
            }
            /*
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            } else {
                callback(www.downloadHandler.text);
                if (saveAsset) {
                    if (!string.IsNullOrEmpty(assetName)) {
                        File.WriteAllText(filePath + assetName + ".csv", www.downloadHandler.text);
                    } else {
                        throw new Exception("assetName is null");
                    }
                }
            }*/
        }

        /*
        WWWForm form = new WWWForm();
        WWW download = new WWW(url, form);

        yield return download;

        if (!string.IsNullOrEmpty(download.error)) {
            Debug.Log("Error downloading: " + download.error);
        } else {
            callback(download.text);
            if (saveAsset) {
                if (!string.IsNullOrEmpty(assetName))
                    File.WriteAllText("Assets/Resources/Downloaded Spreadsheets/" + assetName + ".csv", download.text);
                else {
                    throw new Exception("assetName is null");
                }
            }
        }
        */
    }


    //CSV reader from https://bravenewmethod.com/2014/09/13/lightweight-csv-reader-for-unity/

    public static readonly string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    public static readonly string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    public static readonly char[] TRIM_CHARS = { '\"' };

    public static List<List<string>> ReadCSV(string file) {
        var data = Resources.Load(file) as TextAsset;
        return ParseCSV2(data.text);
    }

    public static List<List<string>> ParseCSV2(string text) {
        text = CleanReturnInCsvTexts(text);

        var list = new List<List<string>>();
        var lines = Regex.Split(text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);

        bool jumpedFirst = false;

        foreach (var line in lines) {
            if (!jumpedFirst) {
                jumpedFirst = true;
                continue;
            }
            var values = Regex.Split(line, SPLIT_RE);

            var entry = new List<string>();
            for (var j = 0; j < header.Length && j < values.Length; j++) {
                var value = values[j];
                value = DecodeSpecialCharsFromCSV(value);
                entry.Add(value);
            }
            list.Add(entry);
        }
        return list;
    }

    public static string DecodeSpecialCharsFromCSV(string value) {
        value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "").Replace("<br>", "\n").Replace("<c>", ",");
        return value;
    }
}
