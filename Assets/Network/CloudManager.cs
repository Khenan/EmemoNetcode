using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models.Data.Player;
using Unity.Services.Core;
using UnityEngine;
using SaveOptions = Unity.Services.CloudSave.Models.Data.Player.SaveOptions;

public class CloudManager : MonoBehaviour
{
    public static CloudManager Instance { get; private set; }

    public struct CloudResultString { public bool success; public string value; }
    public struct CloudResultVector3Int { public bool success; public Vector3Int value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            UnityServicesInitializeAsync();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private async void UnityServicesInitializeAsync()
    {
        try
        {
            await UnityServices.InitializeAsync();
            Debug.Log("Unity Services Initialized");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public async Task SaveData(string _key, object _object)
    {
        Dictionary<string, object> _data = new Dictionary<string, object>{
          {_key, _object}
        };
        await CloudSaveService.Instance.Data.Player.SaveAsync(_data);
        Debug.Log($"Saved data {_key}: {_object}");
    }

    public async void SavePublicData(string _key, object _object)
    {
        Dictionary<string, object> _data = new Dictionary<string, object> { { _key, _object } };
        await CloudSaveService.Instance.Data.Player.SaveAsync(_data, new SaveOptions(new PublicWriteAccessClassOptions()));
        Debug.Log($"Saved public data {_key}: {_object}");
    }

    public async Task<CloudResultString> LoadDataString(string _key)
    {
        CloudResultString _result = new CloudResultString();
        var _playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { _key });
        if (_playerData.TryGetValue(_key, out Unity.Services.CloudSave.Models.Item _keyValue))
        {
            _result.value = _keyValue.Value.GetAs<string>();
            _result.success = true;
            Debug.Log($"{_key} value: {_keyValue.Value.GetAs<string>()}");
        }
        else
        {
            _result.success = false;
        }
        return _result;
    }
    public async Task<CloudResultVector3Int> LoadDataVector3Int(string _key)
    {
        CloudResultVector3Int _result = new CloudResultVector3Int();
        var _playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { _key });
        if (_playerData.TryGetValue(_key, out Unity.Services.CloudSave.Models.Item _keyValue))
        {
            _result.value = _keyValue.Value.GetAs<Vector3Int>();
            _result.success = true;
            Debug.Log($"{_key} value: {_keyValue.Value.GetAs<Vector3Int>()}");
        }
        else
        {
            _result.success = false;
        }
        return _result;
    }

}