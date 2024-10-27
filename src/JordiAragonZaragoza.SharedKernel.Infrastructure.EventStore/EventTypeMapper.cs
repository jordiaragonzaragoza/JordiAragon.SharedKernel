namespace JordiAragonZaragoza.SharedKernel.Infrastructure.EventStore
{
    using System;
    using System.Collections.Concurrent;
    using JordiAragonZaragoza.SharedKernel.Contracts.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Helpers;

    public class EventTypeMapper : ISingletonDependency
    {
        public static readonly EventTypeMapper Instance = new();

        private readonly ConcurrentDictionary<string, Type> typeMap = new();
        private readonly ConcurrentDictionary<Type, string> typeNameMap = new();

        public void AddCustomMap<T>(string eventTypeName)
            => this.AddCustomMap(typeof(T), eventTypeName);

        public void AddCustomMap(Type eventType, string eventTypeName)
        {
            this.typeNameMap.AddOrUpdate(eventType, eventTypeName, (_, typeName) => typeName);
            this.typeMap.AddOrUpdate(eventTypeName, eventType, (_, type) => type);
        }

        public string ToName<TEventType>()
            => this.ToName(typeof(TEventType));

        public string ToName(Type eventType)
            => this.typeNameMap.GetOrAdd(eventType, type =>
        {
            var eventTypeName = type.FullName!;

            this.typeMap.TryAdd(eventTypeName, type);

            return eventTypeName;
        });

        public Type ToType(string eventTypeName) => this.typeMap.GetOrAdd(eventTypeName, typeName =>
        {
            var type = TypeHelper.GetFirstMatchingTypeFromCurrentDomainAssembly(typeName);

            this.typeNameMap.TryAdd(type, typeName);

            return type;
        });
    }
}