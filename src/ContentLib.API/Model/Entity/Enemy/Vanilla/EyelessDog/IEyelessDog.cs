using ContentLib.Core.Model.Entity.Util;
using ContentLib.EnemyAPI.Model.Enemy;
using UnityEngine;

namespace ContentLib.API.Model.Entity.Enemy.Vanilla.EyelessDog;

/// <summary>
/// Interface representing the general functionality of the Eyeless Dog Vanilla Enemy. 
/// </summary>
public interface IEyelessDog : IEnemy, IKillable
{
    #region Fields

    /// <summary>
    /// Boolean representing whether this Eyeless Dog is lunging or not.
    /// </summary>
    bool isLunging { get; }
    /// <summary>
    /// Integer representing the current suspicion level of this Eyeless Dog.
    /// This determines the Dog's AI state.
    /// </summary>
    int SuspicionLevel { get; }
    /// <summary>
    /// Vector3 Position of where the Eyeless Dog guesses the source of a sound is.
    /// This will be where the dog will lunge toward.
    /// </summary>
    Vector3 GuessedSearchPosition { get; }
    /// <summary>
    /// Vector3 Position of exactly where a sound this Eyeless Dog heard originated.
    /// Used as the center of the Dog's search for the source of the sound.
    /// </summary>
    Vector3 AbsoluteSearchPosition { get; }


    #endregion
    
    #region actions

    /// <summary>
    /// Causes this Eyeless Dog to lunge toward where it is currently heading
    /// </summary>
    void Lunge();
    /// <summary>
    /// Causes this Eyeless Dog to pick up the given body in its mouth
    /// </summary>
    /// <param name="body">A dead player body</param>
    void GrabDeadBody(DeadBodyInfo body);
    /// <summary>
    /// Causes this Eyeless Dog to drop any body from its mouth it may be holding.
    /// </summary>
    void DropDeadBody();
    /// <summary>
    /// Causes this Eyeless Dog to alert all other Eyeless Dogs to their position.
    /// This causes all other Dogs to converge on this dog's position as if they heard a sound.
    /// </summary>
    void AlertOtherDogs();


    #endregion

    #region suspicion
    
    /// <summary>
    /// Causes this Eyeless Dog to hear a noise at the given position, sometimes increasing their suspicion level.
    /// </summary>
    /// <param name="noisePosition">The Vector3 position of the noise</param>
    /// <param name="noiseLoudness">The Float loudness of the noise</param>
    /// <param name="timesNoisePlayedInOneSpot">The Integer amount of times the noise was played in the same spot, defaults to 0</param>
    /// <param name="noiseID">The Integer ID representing the type of the noise to be heard, defaults to 0</param>
    void DetectNoise(Vector3 noisePosition, float noiseLoudness, int timesNoisePlayedInOneSpot = 0, int noiseID = 0);
    /// <summary>
    /// Causes this Eyeless Dog to immediately charge toward the given position as if it heard a sound.
    /// This is usually called whenever another Eyeless Dog uses their own AlertOtherDogs() method.
    /// </summary>
    /// <param name="howlPosition"></param>
    /// <seealso cref="AlertOtherDogs()">AlertOtherDogs()</seealso>
    void ReactToOtherDogHowl(Vector3 howlPosition);

    #endregion
}