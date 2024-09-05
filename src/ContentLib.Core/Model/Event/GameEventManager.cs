using System;
using System.Collections.Generic;
using UnityEngine;

namespace ContentLib.Core.Model.Event
{
    /// <summary>
    /// Manager responsible for the subscription and triggering of In-Game Events, for the purposes of API calls. <i>
    /// Developer Note: I swear to god I didn't start this with the intention of making a full event api... whoops... </i>
    /// </summary>
    public class GameEventManager
    {
        /// <summary>
        /// Singleton pattern call via a lazy implementation of the manager.
        /// </summary>
        private static readonly Lazy<GameEventManager> instance = new Lazy<GameEventManager>(() => new GameEventManager());

        /// <summary>
        /// Gets the singleton instance of the manager.
        /// </summary>
        public static GameEventManager Instance => instance.Value;

        /// <summary>
        /// Dictionary containing the various delegated handlers for specified GameEvents, determined by their
        /// GameEventType. 
        /// </summary>
        private readonly Dictionary<Type, Delegate> eventHandlers = new Dictionary<Type, Delegate>();

        /// <summary>
        /// Private constructor to ensure the GameEventManager is only obtainable via singleton method.
        /// </summary>
        private GameEventManager() { }

        /// <summary>
        /// Subscribes an event handler (i.e. a method that takes in an implementation of IGameEvent) to its respective
        /// In-Game-Event, via its GameEventType.
        /// </summary>
        /// <param name="handler">The method responsible for handling the Event.</param>
        /// <typeparam name="TEvent">The type parameter of IGameEvent child to handle.</typeparam>
        public void Subscribe<TEvent>(Action<TEvent> handler) where TEvent : IGameEvent
        {
            Type? eventType = typeof(TEvent).BaseType;

            if (eventHandlers.TryGetValue(eventType, out var existingHandler))
            {
                eventHandlers[eventType] = Delegate.Combine(existingHandler, handler);
            }
            else
            {
                eventHandlers[eventType] = handler;
            }
        }


   
        /// <summary>
        /// Unsubscribes a delegate from a specified Event Type.
        /// </summary>
        /// <param name="eventType">The type of event to unsubscribe the handler from.</param>
        /// <param name="handler">The handler of the Event.</param>
        /// <typeparam name="TEvent">The type parameter of the IGameEvent child to unsuscribe the handler from.</typeparam>
        public void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : IGameEvent
        {
            Type? eventType = typeof(TEvent).BaseType;
            if (!eventHandlers.ContainsKey(eventType)) return;
            
            eventHandlers[eventType] = Delegate.Remove(eventHandlers[eventType], handler);
            if (eventHandlers[eventType] == null)
            {
                eventHandlers.Remove(eventType);
            }
        }

        /// <summary>
        /// Triggers the given In-Game Event, in turn triggering the event handling logic, if applicable. 
        /// </summary>
        /// <param name="gameEvent">The game event to trigger.</param>
        /// <typeparam name="TEvent">The type parameter of the Triggered Event.</typeparam>
        public void Trigger<TEvent>(TEvent gameEvent) where TEvent : IGameEvent
        {
            Type? eventType = typeof(TEvent).BaseType;
            Debug.Log($"$GameEventManager::Trigger: Game event type: {eventType}");
            if (!eventHandlers.TryGetValue(eventType, out var handler)) return;
            
            var eventHandler = handler as Action<TEvent>;
            eventHandler?.Invoke(gameEvent);
        }
    }
}
