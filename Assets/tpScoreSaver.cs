using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class tpScoreSaver : MonoBehaviour {

    class jsonClass
    {
        public List<Player> Players { get; set; }
    }

    string xmlPath;

    void Start()
    {
        xmlPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "..\\LocalLow\\Steel Crate Games\\Keep Talking and Nobody Explodes\\TwitchPlaysUsers.xml");
        GetComponent<KMGameInfo>().OnStateChange += (state) =>
        {
            if (state == KMGameInfo.State.Quitting || state == KMGameInfo.State.Setup) StartCoroutine(Poster());
        };

        GetComponent<KMBombInfo>().OnBombSolved += () =>
        {
            StartCoroutine(Poster());
        };

        GetComponent<KMBombInfo>().OnBombExploded += () =>
        {
            StartCoroutine(Poster());
        };
    }

    IEnumerator Poster()
    {
        var toSerialize = new jsonClass()
        {
            Players = new List<Player>()
        };
        foreach (var Player in XDocument.Parse(String.Join("", File.ReadAllLines(xmlPath))).Root.Elements("LeaderboardEntry"))
        {
            var player = new Player();
            Func<string, string> find = (n) =>
            {
                var element = Player.Element(n);
                return element == null ? null : element.Value;
            };
            player.UserName = find("UserName");
            var colors = Player.Element("UserColor");
            player.UserColor = new Dictionary<string, int>()
                {
                    { "r", (int)(float.Parse(colors.Element("r").Value) * 100) },
                    { "g", (int)(float.Parse(colors.Element("g").Value) * 100) },
                    { "b", (int)(float.Parse(colors.Element("b").Value) * 100) },
                };
            player.SolveCount = find("SolveCount");
            player.StrikeCount = find("StrikeCount");
            player.SolveScore = find("SolveScore");
            player.Rank = find("Rank");
            player.TotalSoloClears = find("TotalSoloClears");
            player.SoloRank = find("SoloRank");
            var oo = find("OptOut");
            player.OptedOut = oo == "true" ? true : false;
            toSerialize.Players.Add(player);
        }
        string Serialized = JsonConvert.SerializeObject(toSerialize, Formatting.None);
        yield return Post(String.Format("http://{0}/setReq/{1}/{2}",tpData.IP, tpData.pwd, tpData.Name), Serialized);
    }

    IEnumerator Post(string url, string bodyJsonString)
    {
        UnityWebRequest request = null;
        do
        {
            request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
        } while (request.isNetworkError || request.isHttpError);
    }
}
