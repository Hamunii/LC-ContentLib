using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ContentLib.Core.Model.Event.Listener;
using Dissonance;
using UnityEngine;

namespace ContentLib.API.Model.Event
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
        private readonly Dictionary<Type, Delegate> _eventHandlers = new Dictionary<Type, Delegate>();

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

            if (_eventHandlers.TryGetValue(eventType, out var existingHandler))
            {
                _eventHandlers[eventType] = Delegate.Combine(existingHandler, handler);
            }
            else
            {
                _eventHandlers[eventType] = handler;
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
            if (!_eventHandlers.ContainsKey(eventType)) return;
            
            _eventHandlers[eventType] = Delegate.Remove(_eventHandlers[eventType], handler);
            if (_eventHandlers[eventType] == null)
            {
                _eventHandlers.Remove(eventType);
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
            if (!_eventHandlers.TryGetValue(eventType, out var handler)) return;
            
            var eventHandler = handler as Action<TEvent>;
            eventHandler?.Invoke(gameEvent);
        }
        
        /// <summary>
        /// Registers a class that implements IListener and subscribes to each IGameEvent method within the class. 
        /// </summary>
        /// <param name="listener">The listener to register.</param>
        /// <exception cref="InvalidOperationException">Called if the methods marked with the EventDelegate
        /// Attribute are not correctly formatted.</exception>
        public void RegisterListener(IListener listener)
        {
            Debug.Log($"GameEventManager::RegisterListener {listener.GetType()}");
            var listenerType = listener.GetType();
            var methodsWithAttribute = listenerType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(method => method.GetCustomAttributes(typeof(EventDelegateAttribute), true).Any());

            foreach (var method in methodsWithAttribute)
            {
                var parameters = method.GetParameters();
                if (parameters.Length != 1)
                {
                    throw new InvalidOperationException($"Method {method.Name} does not have exactly one parameter.");
                }
                var eventType = parameters[0].ParameterType;
                
                if (!typeof(IGameEvent).IsAssignableFrom(eventType))
                    throw new InvalidOperationException($"Method {method.Name} parameter is not a game event!");
                
                var subscribeMethod = typeof(GameEventManager).GetMethod(nameof(Subscribe))
                    ?.MakeGenericMethod(eventType);

                // I know it's unlikely, but it's a nice hail mary check. 
                if (subscribeMethod == null)
                {
                    throw new InvalidOperationException($"Unable to find Subscribe method for event type {eventType.Name}.");
                }
                var actionDelegate = Delegate.CreateDelegate(typeof(Action<>).MakeGenericType(eventType), listener, method);
                subscribeMethod.Invoke(this, new object[] { actionDelegate });
            }
        }
    }
}
