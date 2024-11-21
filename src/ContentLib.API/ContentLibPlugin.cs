using System.Linq;
using System.Runtime.CompilerServices.Model.Managers;
using BepInEx;
using ContentLib.API;
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

    

    protected virtual void Initialize()
    {
        ContentLibAPI.Instance.InitializeAPI();
    }
    protected abstract void OnAwake();
}