using System;
using System.Collections.Generic;
using ContentLib.API.Model.Mods;
using ContentLib.API.Model.Mods.Content.Types;
using ContentLib.Core.Utils;
using ContentLib.EnemyAPI.Model.Enemy;
using UnityEngine;

namespace ContentLib.Core.Loader;

public class CustomContentManager
{
    private readonly Dictionary<Type, IFactory<ICustomContent>> _factories = new();
    private readonly Dictionary<Type, Action<ICustomContent>> _registrationActions;
    private static readonly Lazy<CustomContentManager> instance = new Lazy<CustomContentManager>(() => new CustomContentManager());
    public static CustomContentManager Instance => instance.Value;
    public CustomContentManager()
    {
        _registrationActions = new Dictionary<Type, Action<ICustomContent>>
        {
            { typeof(ICustomEnemyContent), RegisterEnemy },
        };
        
    }

    public void RegisterContent(ICustomContent content)
    {
        Type contentType = content.GetType();
        
        if (_registrationActions.TryGetValue(contentType, out var registrationAction))
        {
            registrationAction(content);
        }
        else
        {
            Debug.LogWarning($"No registration action found for content type: {contentType.Name}. Cannot register.");
        }
    }

    public void RegisterFactory(Type type,IFactory<ICustomContent> factory) => _factories.Add(type, factory);


    private void RegisterEnemy(ICustomContent content)
    {
        if (!(content is IEnemy enemy))
        {
            Debug.Log("Content attempting to be registered is not an in stance of IEnemy");
        }
        //TODO put conversion into the EnemyAI / other enemy stuff needed here. 
        Debug.Log($"Registered enemy: {content.Properties.ClassPath}");
        //TODO put the registration logic here
    }

}