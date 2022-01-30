using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.Net.Http;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using System.Security.Cryptography;
using System;
using System.Net;
using System.Text.RegularExpressions;

public class PlayFlowCloud : EditorWindow
{
    private static PlayFlowConfig data;


    private string version = "1";
    private static string defaultPath = @"Builds\Linux\Server\PlayFlowCloud\PlayFlowCloudServerFiles\Server.x86_64";
    private string serverlUrl = "";
    private string port = "";
    private string serverArguments = "";
    private string token = "";

    private static float t = 0;
    private bool enableSSL = false;

    private string logs = "PlayFlow Logs: ";

    public string[] options = new string[] { "North America", "Europe", "Southeast Asia | Oceanic", "East Asia" };
    private string[] servers = new string[] { "https://api.playflow.app/", "https://eu.api.playflow.app/", "https://sea.api.playflow.app/", "https://ea.api.playflow.app/" };
    public int index = 0;
    private string apiUrl = "https://api.playflow.app/";

    Vector2 scroll;

    // This method will be called on load or recompile
    [InitializeOnLoadMethod]
    private static void OnLoad()
    {
        // if no data exists yet create and reference a new instance
        if (!data)
        {
            // as first option check if maybe there is an instance already
            // and only the reference got lost
            // won't work ofcourse if you moved it elsewhere ...
            data = AssetDatabase.LoadAssetAtPath<PlayFlowConfig>("Assets/PlayFlowCloud/PlayFlowConfig.asset");

            // if that was successful we are done
            if (data) return;

            // otherwise create and reference a new instance
            data = CreateInstance<PlayFlowConfig>();

            AssetDatabase.CreateAsset(data, "Assets/PlayFlowCloud/PlayFlowConfig.asset");
            AssetDatabase.Refresh();
        }
    }

    // Add menu item named "My Window" to the Window menu
    [MenuItem("PlayFlow/PlayFlowCloud Server")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(PlayFlowCloud));
    }

    private static GUISkin _uiStyle;
    public static GUISkin uiStyle
    {
        get
        {
            if (_uiStyle != null)
                return _uiStyle;
            _uiStyle = GetUiStyle();
            return _uiStyle;
        }
    }

    private static GUISkin GetUiStyle()
    {
        var searchRootAssetFolder = Application.dataPath;
        var playFlowGuiPath = Directory.GetFiles(searchRootAssetFolder, "PlayFlowSkin.guiskin", SearchOption.AllDirectories);
        foreach (var eachPath in playFlowGuiPath)
        {
            var loadPath = eachPath.Substring(eachPath.LastIndexOf("Assets"));
            return (GUISkin)AssetDatabase.LoadAssetAtPath(loadPath, typeof(GUISkin));
        }
        return null;
    }

    void getGlobalValues()
    {
        var serializedObject = new SerializedObject(data);
        token = serializedObject.FindProperty("token").stringValue;
        serverArguments = serializedObject.FindProperty("serverArguments").stringValue;
        port = serializedObject.FindProperty("playflowUrl").stringValue;
        enableSSL = serializedObject.FindProperty("enableSSL").boolValue;
        index = serializedObject.FindProperty("serverLocation").intValue;
        apiUrl = servers[index];
    }

    void OnGUI()
    {
        var serializedObject = new SerializedObject(data);
        // fetches the values of the real instance into the serialized one
        serializedObject.Update();

        var configtoken = serializedObject.FindProperty("token");
        var configserverArguments = serializedObject.FindProperty("serverArguments");
        var configport = serializedObject.FindProperty("playflowUrl");
        var configenableSSL = serializedObject.FindProperty("enableSSL");
        var configapiUrl = serializedObject.FindProperty("serverLocation");

        scroll = EditorGUILayout.BeginScrollView(scroll);
        GUI.skin = uiStyle;

        GUILayout.Label("PlayFlow Cloud Server Deploy Settings");
        EditorGUILayout.LabelField("Use the PlayFlow Server Port number as your game's port for both the clients and server", uiStyle.GetStyle("labelsmall"));

        configtoken.stringValue = EditorGUILayout.TextField("PlayFlow App Token", configtoken.stringValue, uiStyle.textField);
        configport.stringValue = EditorGUILayout.TextField("PlayFlow Server URL:Port", configport.stringValue, uiStyle.textField);
        configserverArguments.stringValue =  EditorGUILayout.TextField("Arguments (optional)", configserverArguments.stringValue, uiStyle.textField);
        configenableSSL.boolValue = EditorGUILayout.Toggle("Enable SSL for WebSockets", configenableSSL.boolValue);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Server Location", uiStyle.GetStyle("labelsmall"));
        index = EditorGUILayout.Popup(configapiUrl.intValue, options);
        configapiUrl.intValue = index;
        EditorGUILayout.EndHorizontal();

        token = configtoken.stringValue;

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Get PlayFlow App Token"))
        {
            System.Diagnostics.Process.Start("https://app.playflowcloud.com");

        }

        if (GUILayout.Button("Assign PlayFlow Port"))
        {
            try
            {
                getPort();
            }
            catch (Exception e)
            {

                logs = "Port Generation Failed! StackTrace: " + e.StackTrace;
            }
        }

        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Publish Server"))
        {
            try
            {
                buildServer();
            }
            catch (Exception e)
            {
                logs = "PlayFlow Build & Publish Failed! StackTrace: " + e.StackTrace;
                EditorUtility.ClearProgressBar();
            }

        }

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Get Server Logs"))
        {
            try
            {
                getServerLogs();

            }
            catch (Exception e)
            {

                logs = "Port Generation Failed! StackTrace: " + e.StackTrace;
            }
        }

        if (GUILayout.Button("Restart Server"))
        {
            try
            {
                restartServer();

            }
            catch (Exception e)
            {
                logs = "Server restart failed! Logs: " + e.StackTrace;
                EditorUtility.ClearProgressBar();
            }
        }


        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Documentation"))
        {
            System.Diagnostics.Process.Start("https://playflowdev.github.io/PlayFlowDocumentation/#/");

        }

        if (GUILayout.Button("YouTube"))
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/channel/UC8MVcq6a4PwzUh3jTlBKu8w");

        }

        if (GUILayout.Button("Discord"))
        {
            System.Diagnostics.Process.Start("https://discord.gg/FAFFyt9DDX");

        }


        EditorGUILayout.EndHorizontal();
        EditorGUILayout.TextArea(logs, uiStyle.textArea);
        EditorGUILayout.EndScrollView();
        serializedObject.ApplyModifiedProperties();
       


    }

    public void OnInspectorUpdate()
    {
        // This will only get called 10 times per second.
        Repaint();
    }

    private async void getServerLogs()
    {
        getGlobalValues();
        EditorUtility.DisplayProgressBar("PlayFlowCloud", "Getting Logs", 0.5f);
        await getServerLogsAPI();
        EditorUtility.ClearProgressBar();
    }

    private async Task getServerLogsAPI()
    {
        string actionUrl = apiUrl + "logs";
        using (var client = new HttpClient())
        using (var formData = new MultipartFormDataContent())
        {
            formData.Headers.Add("serverToken", token);
            formData.Headers.Add("version", version);

            var response = await client.PostAsync(actionUrl, formData);

            if (!response.IsSuccessStatusCode)
            {
                Debug.Log("Get Logs Failed.");
                logs = "PlayFlow Logs: Server Log Get Failed. If error persists, contact the PlayFlow Discord Support: https://discord.gg/FAFFyt9DDX  \n API Response Code: " + response + "\nAPI Response: " + await response.Content.ReadAsStringAsync();
            }
            else
            {
                string templogs = await response.Content.ReadAsStringAsync();

                logs = "PlayFlow Logs: " + templogs;
                Debug.Log(logs);
            }
        }
    }

    private async void restartServer()
    {
        getGlobalValues();
        EditorUtility.DisplayProgressBar("PlayFlowCloud", "Restarting Server", 0.5f);
        await restartServerAPI();
        EditorUtility.ClearProgressBar();
    }

    private async Task restartServerAPI()
    {
        string actionUrl = apiUrl + "restart";
        using (var client = new HttpClient())
        using (var formData = new MultipartFormDataContent())
        {
            formData.Headers.Add("serverToken", token);
            formData.Headers.Add("version", version);
            formData.Headers.Add("serverArguments", serverArguments);


            var response = await client.PostAsync(actionUrl, formData);

            if (!response.IsSuccessStatusCode)
            {
                Debug.Log("Restart Failed.");
                logs = "PlayFlow Logs: Server Restart failed. If error persists, contact the PlayFlow Discord Support: https://discord.gg/FAFFyt9DDX  \n API Response Code: " + response + "\nAPI Response: " + await response.Content.ReadAsStringAsync(); 
            }
            else
            {
                string templogs = System.Text.Encoding.UTF8.GetString(await response.Content.ReadAsByteArrayAsync());

                logs = "PlayFlow Logs: " + templogs;
            }
        }
    }
    private async void buildServer()
    {

        if (string.IsNullOrEmpty(port))
        {
            Debug.LogError("Port value is not defined. Please put your Server's port in the Port Text box for PlayFlow Cloud");
        }
        else
        {


            List<string> scenes = new List<string>();
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    scenes.Add(scene.path);
                }
            }

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = scenes.ToArray();
            buildPlayerOptions.locationPathName = defaultPath;

          
    
            buildPlayerOptions.target = BuildTarget.StandaloneLinux64;

#if UNITY_2021_1_OR_NEWER

            if (Application.unityVersion.CompareTo(("2021.2")) >= 0)
            {
                buildPlayerOptions.subtarget = (int) StandaloneBuildSubtarget.Server;
                buildPlayerOptions.options = BuildOptions.CompressWithLz4HC;
            } else
            {
                buildPlayerOptions.options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode;
            }

#else

            buildPlayerOptions.options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode;

#endif
            BuildPipeline.BuildPlayer(buildPlayerOptions);

            t = 0.35f;

            EditorUtility.DisplayProgressBar("PlayFlowCloud", "Compressing files", t);

            await ZipServerBuild();
        }
      

        EditorUtility.ClearProgressBar();
        
    }

    public async Task ZipServerBuild()
    {
        string directoryToZip = Path.GetDirectoryName(defaultPath);
        if (Directory.Exists(directoryToZip))
        {
            string targetfile = Path.Combine(directoryToZip, @"../Server.zip");
            await ZipPath(targetfile, directoryToZip, null, true, null);
        }
    }

    public async Task ZipPath(string zipFilePath, string sourceDir, string pattern, bool withSubdirs, string password)
    {
        FastZip fz = new FastZip();
        fz.CompressionLevel = Deflater.CompressionLevel.DEFAULT_COMPRESSION;
        fz.CreateZip(zipFilePath, sourceDir, withSubdirs, pattern);

        t = 0.5f;
        EditorUtility.DisplayProgressBar("PlayFlowCloud", "Uploading", t);
        string response = await Upload(zipFilePath);
        serverlUrl = response;
        logs = "PlayFlow Logs: Game server is up and running! Use the following URL & Port for your clients: " + serverlUrl;

        var serializedObject = new SerializedObject(data);

        var configport = serializedObject.FindProperty("playflowUrl");

        configport.stringValue = serverlUrl;
        serializedObject.ApplyModifiedProperties();


        Debug.Log("PlayFlow Logs: Server successfully built!  Game server is up and running! Use the following URL & Port for your clients: " + response);

        if (Directory.Exists(sourceDir))
        {
            Directory.Delete(sourceDir, true);
        }

        if (File.Exists(zipFilePath))
        {
            File.Delete(zipFilePath);
        }
    }

    private async Task<string> Upload(string fileLocation)
    {
        getGlobalValues();
        string actionUrl = apiUrl + "files";
        Uri uri = new Uri(actionUrl);

        byte[] file_bytes = File.ReadAllBytes(fileLocation);
        HttpClientHandler handler = new HttpClientHandler();

        byte[] responseArray;
        using (WebClient client = new WebClient())
        {
            client.Headers.Add("serverToken", token);
            client.Headers.Add("serverArguments", serverArguments);
            client.Headers.Add("enableSSL", enableSSL.ToString());
            client.Headers.Add("version", version);
            responseArray = client.UploadFile(actionUrl, fileLocation);

        }

        return (System.Text.Encoding.ASCII.GetString(responseArray));
    }

    private async void getPort()
    {
        var serializedObject = new SerializedObject(data);
        
        getGlobalValues();
        EditorUtility.DisplayProgressBar("PlayFlowCloud", "Generating Port", 0.5f);
        port = await GetRandomPort();

        var configport = serializedObject.FindProperty("playflowUrl");

        configport.stringValue = port;
        serializedObject.ApplyModifiedProperties();
        EditorUtility.ClearProgressBar();
    }

    private async Task<string> GetRandomPort()
    {

        string actionUrl = apiUrl + "getport";
        using (var client = new HttpClient())
        using (var formData = new MultipartFormDataContent())
        {
            formData.Headers.Add("serverToken", token);
            formData.Headers.Add("version", version);
            var response = await client.PostAsync(actionUrl, formData);

            if (!response.IsSuccessStatusCode)
            {
                Debug.Log("Port Generation Failed.");
                logs = "PlayFlow Logs: Port Generation Failed. Please try again in a few seconds. If error persists, contact the PlayFlow Discord Support: https://discord.gg/FAFFyt9DDX \n API Response Code: " + response +  "\nAPI Response: "+ await response.Content.ReadAsStringAsync();
                return "Port generation failed. See PlayFlow Logs below for more info";
            }
            else
            {
                logs = "PlayFlow Logs: Successfully Generated Port! Use the port specified above in your game server's configuration";
                return await response.Content.ReadAsStringAsync();
            }
        }

    }
}