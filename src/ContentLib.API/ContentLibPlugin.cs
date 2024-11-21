using System.Linq;
using System.Runtime.CompilerServices.Model.Managers;
using BepInEx;
using ContentLib.API;
using ContentLib.API.Model;
using UnityEngine;

namespace System.Runtime.CompilerServices;
/// <summary>
/// Abstract child of the BaseUnityPlugin, used to load mods that utilise the Content-Lib API. 
/// </summary>
public abstract class ContentLibPlugin : BaseUnityPlugin
{
    protected virtual void Awake()
    {
        Initialize();
        OnAwake();
    }
    
    
    protected void Initialize()
    {
        IAPILoader apiLoader = FindObjectsByType<MonoBehaviour>(FindObjectsInactive
            .Include,FindObjectsSortMode.None)
            .OfType<IAPILoader>()
            .ToArray()[0];
        ContentLibAPI.Instance.InitializeAPI(apiLoader);
        if (ContentLibAPI.Instance == null)
        { 
            throw new NullReferenceException();
        }
    }
    protected abstract void OnAwake();
}