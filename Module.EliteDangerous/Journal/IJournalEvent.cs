using Module.EliteDangerous.DataModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Module.EliteDangerous.Journal {

    /// <summary>
    /// Interface for parsed C# objects for journal events.
    /// </summary>
    public interface IJournalEvent {
        /// <summary>
        /// Updates the values in the given model to reflect changes caused by this event.
        /// </summary>
        void ApplyUpdate(EliteDangerousDataModel model);
    }

    /// <summary>
    /// Utility class for journal events.
    /// </summary>
    public static class JournalEvent {
        static JournalEvent() {
            JournalEventConverter = new JournalEventConverter();
            JournalEventSettings = new JsonSerializerSettings();
            JournalEventSettings.Converters.Add(JournalEventConverter);
        }

        public static JsonConverter JournalEventConverter { get; }
        public static JsonSerializerSettings JournalEventSettings { get; }
    }

    /// <summary>
    /// Converter that will attempt to convert IJournalEvent types to their concrete implementation.
    /// </summary>
    public class JournalEventConverter : JsonConverter {

        private static readonly Dictionary<string, Type> knownEventTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        static JournalEventConverter() {
            // Populate the known event subtype dictionary with any type marked with JournalEventTypeAttribute
            foreach (var @type in typeof(JournalEvent).Assembly.GetTypes())
                if (type.GetCustomAttribute<JournalEventTypeAttribute>() is JournalEventTypeAttribute attr)
                    knownEventTypes.Add(attr.EventName, type);
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(IJournalEvent);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            var obj = JObject.Load(reader);

            // Read the "event" JSON property and see if it is an event that we care about
            if (knownEventTypes.TryGetValue((string)obj.Property("event"), out var eventType)) {
                // Create a new object from that event type, populate it and return it
                var eventInstance = Activator.CreateInstance(eventType);
                serializer.Populate(obj.CreateReader(), eventInstance);
                return eventInstance;
            }
            return null;
        }

        // We do not need to support writing events :)
        public override bool CanWrite => false;
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();
    }

    /// <summary>
    /// Type used to decorate IJournalEvents to allow them to be deserialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class JournalEventTypeAttribute : Attribute {
        public string EventName { get; }
        public JournalEventTypeAttribute(string eventName) => EventName = eventName;
    }
}
