using System;
using System.Collections.Generic;
using UnityEngine;


public class DialogueContainer : MonoBehaviour
{
    [SerializeField] TextAsset dialogueFile;
    public static DialogueContainer instance;

    public void Start()
    {
        instance = this;
    }

    public string[] CustomQuarry(Dialogue[] dialogues, string key)
    {
        foreach (Dialogue d in dialogues)
        {
            if (d.Key == key)
            {
                return d.DialogueItems;
            }
        }

        return null;
    }

    // store an array of Dialogue entries (JSON has an array of objects in the TextAsset)
    public Dialogue[] AllDialogue;

    public void Awake()
    {
        Load();
    }

    public void Load()
    {
        // read JSON from the provided TextAsset and populate AllDialogue
        if (dialogueFile == null)
        {
            throw new NullReferenceException();
            return;
        }

        try
        {
            // get trimmed text
            string jsonText = dialogueFile.text?.Trim() ?? string.Empty;

            if (jsonText.StartsWith("["))
            {
                // top-level array: deserialize directly into Dialogue[] via helper
                AllDialogue = JsonHelper.FromJson<Dialogue>(jsonText) ?? new Dialogue[0];
            }
            else
            {
                // assume wrapped object with AllDialogue property
                var data = JsonUtility.FromJson<DialogueFile>(jsonText);
                AllDialogue = data != null && data.AllDialogue != null ? data.AllDialogue : new Dialogue[0];
            }
        }
        catch (Exception ex)
        {
            AllDialogue = new Dialogue[0];
            throw new ArgumentException(".json file is malformed or invalid", ex);
        }
    }

    public void SwitchActive(bool active) => ScrollingText.instance.textComponent.gameObject.SetActive(active);

    // Helper: return all keys that were loaded
    public string[] GetAllKeys()
    {
        if (AllDialogue == null) return new string[0];
        var keys = new List<string>(AllDialogue.Length);
        foreach (var d in AllDialogue)
            if (d != null && d.Key != null)
                keys.Add(d.Key);
        return keys.ToArray();
    }

    public void QueueDialogue(string key)
    {
        string[] queueable = CustomQuarry(AllDialogue, key);
        if (queueable == null)
        {
            Debug.LogWarning($"DialogueContainer.QueueDialogue: No dialogue found for key '{key}'.");
            return;
        }
        ScrollingText.instance.MakeTextQueue(queueable);
    }
}

// wrapper required because JsonUtility cannot parse a top-level array directly
[Serializable]
public class DialogueFile
{
    public Dialogue[] AllDialogue;
}

[Serializable]
public class Dialogue
{
    // match JSON property names exactly (JsonUtility is case-sensitive)
    public string Key;
    public string[] DialogueItems;
}

// small helper to deserialize top-level JSON arrays using JsonUtility
internal static class JsonHelper
{
    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }

    public static T[] FromJson<T>(string jsonArray)
    {
        if (string.IsNullOrWhiteSpace(jsonArray))
            return new T[0];

        string wrapped = "{\"Items\":" + jsonArray + "}";
        var wrapper = JsonUtility.FromJson<Wrapper<T>>(wrapped);
        return wrapper != null && wrapper.Items != null ? wrapper.Items : new T[0];
    }
}