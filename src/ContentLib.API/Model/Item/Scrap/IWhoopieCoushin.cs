using UnityEngine;

namespace ContentLib.API.Model.Item.Scrap;

/// <summary>
/// Interface representing the general functionality of a whoopie cushion scrap item. 
/// </summary>
public interface IWhoopieCoushin : IVanillaScrap
{
    /// <summary>
    /// Gets all the potential fart audio clips that can play when stepping on the whoopie cushion (God... I hope there
    /// is a good reason for you to need this...) 
    /// </summary>
    AudioClip[] FartClips { get; set; }
}