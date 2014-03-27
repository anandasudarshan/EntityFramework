// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.AspNet.DependencyInjection;
using Microsoft.AspNet.DependencyInjection.Fallback;
using Microsoft.AspNet.Logging;
using Microsoft.Data.Entity.ChangeTracking;
using Microsoft.Data.Entity.Identity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Storage;
using Microsoft.Data.Entity.Utilities;

namespace Microsoft.Data.Entity
{
    public class EntityConfigurationBuilder
    {
        private readonly ServiceCollection _serviceCollection;

        public EntityConfigurationBuilder()
        {
            _serviceCollection = EntityServices.GetDefaultServices();
        }

        public EntityConfigurationBuilder([NotNull] ServiceCollection serviceCollection)
        {
            Check.NotNull(serviceCollection, "serviceCollection");

            _serviceCollection = serviceCollection;
        }

        public virtual ServiceCollection ServiceCollection
        {
            get { return _serviceCollection; }
        }

        public virtual EntityConfiguration BuildConfiguration()
        {
            return new EntityConfiguration().Initialize(_serviceCollection.BuildServiceProvider());
        }

        // Dingletons

        public virtual EntityConfigurationBuilder UseModel([NotNull] IModel model)
        {
            Check.NotNull(model, "model");

            UseModelSource(new InstanceModelSource(model));

            return this;
        }

        private class InstanceModelSource : IModelSource
        {
            private readonly IModel _model;

            public InstanceModelSource(IModel model)
            {
                _model = model;
            }

            public IModel GetModel(EntityContext _)
            {
                return _model;
            }
        }

        public virtual EntityConfigurationBuilder UseModelSource([NotNull] IModelSource modelSource)
        {
            Check.NotNull(modelSource, "modelSource");

            _serviceCollection.AddInstance<IModelSource>(modelSource);

            return this;
        }

        public virtual EntityConfigurationBuilder UseDataStore([NotNull] DataStore dataStore)
        {
            Check.NotNull(dataStore, "dataStore");

            _serviceCollection.AddInstance<DataStore>(dataStore);

            return this;
        }

        public virtual EntityConfigurationBuilder UseEntitySetInitializer([NotNull] EntitySetInitializer initializer)
        {
            Check.NotNull(initializer, "initializer");

            _serviceCollection.AddInstance<EntitySetInitializer>(initializer);

            return this;
        }

        public virtual EntityConfigurationBuilder UseEntitySetSource([NotNull] EntitySetSource source)
        {
            Check.NotNull(source, "source");

            _serviceCollection.AddInstance<EntitySetSource>(source);

            return this;
        }

        public virtual EntityConfigurationBuilder UseIdentityGeneratorFactory([NotNull] IdentityGeneratorFactory factory)
        {
            Check.NotNull(factory, "factory");

            _serviceCollection.AddInstance<IdentityGeneratorFactory>(factory);

            return this;
        }

        public virtual EntityConfigurationBuilder UseActiveIdentityGenerators([NotNull] ActiveIdentityGenerators generators)
        {
            Check.NotNull(generators, "generators");

            _serviceCollection.AddInstance<ActiveIdentityGenerators>(generators);

            return this;
        }

        public virtual EntityConfigurationBuilder UseEntitySetFinder([NotNull] EntitySetFinder finder)
        {
            Check.NotNull(finder, "finder");

            _serviceCollection.AddInstance<EntitySetFinder>(finder);

            return this;
        }

        public virtual EntityConfigurationBuilder UseEntityKeyFactorySource([NotNull] EntityKeyFactorySource source)
        {
            Check.NotNull(source, "source");

            _serviceCollection.AddInstance<EntityKeyFactorySource>(source);

            return this;
        }

        public virtual EntityConfigurationBuilder UseClrCollectionAccessorSource([NotNull] ClrCollectionAccessorSource source)
        {
            Check.NotNull(source, "source");

            _serviceCollection.AddInstance<ClrCollectionAccessorSource>(source);

            return this;
        }

        public virtual EntityConfigurationBuilder UseClrPropertyGetterSource([NotNull] ClrPropertyGetterSource source)
        {
            Check.NotNull(source, "source");

            _serviceCollection.AddInstance<ClrPropertyGetterSource>(source);

            return this;
        }

        public virtual EntityConfigurationBuilder UseClrPropertySetterSource([NotNull] ClrPropertySetterSource source)
        {
            Check.NotNull(source, "source");

            _serviceCollection.AddInstance<ClrPropertySetterSource>(source);

            return this;
        }

        public virtual EntityConfigurationBuilder UseEntityMaterializerSource([NotNull] EntityMaterializerSource source)
        {
            Check.NotNull(source, "source");

            _serviceCollection.AddInstance<EntityMaterializerSource>(source);

            return this;
        }

        public virtual EntityConfigurationBuilder UseLoggerFactory([NotNull] ILoggerFactory factory)
        {
            Check.NotNull(factory, "factory");

            _serviceCollection.AddInstance<ILoggerFactory>(factory);

            return this;
        }

        public virtual EntityConfigurationBuilder UseModelSource<TService>()
            where TService : IModelSource
        {
            _serviceCollection.AddSingleton<IModelSource, TService>();

            return this;
        }

        public virtual EntityConfigurationBuilder UseDataStore<TService>()
            where TService : DataStore
        {
            _serviceCollection.AddSingleton<DataStore, TService>();

            return this;
        }

        public virtual EntityConfigurationBuilder UseEntitySetInitializer<TService>()
            where TService : EntitySetInitializer
        {
            _serviceCollection.AddSingleton<EntitySetInitializer, TService>();

            return this;
        }

        public virtual EntityConfigurationBuilder UseEntitySetSource<TService>()
            where TService : EntitySetSource
        {
            _serviceCollection.AddSingleton<EntitySetSource, TService>();

            return this;
        }

        public virtual EntityConfigurationBuilder UseIdentityGeneratorFactory<TService>()
            where TService : IdentityGeneratorFactory
        {
            _serviceCollection.AddSingleton<IdentityGeneratorFactory, TService>();

            return this;
        }

        public virtual EntityConfigurationBuilder UseActiveIdentityGenerators<TService>()
            where TService : ActiveIdentityGenerators
        {
            _serviceCollection.AddSingleton<ActiveIdentityGenerators, TService>();

            return this;
        }

        public virtual EntityConfigurationBuilder UseEntitySetFinder<TService>()
            where TService : EntitySetFinder
        {
            _serviceCollection.AddSingleton<EntitySetFinder, TService>();

            return this;
        }

        public virtual EntityConfigurationBuilder UseEntityKeyFactorySource<TService>()
            where TService : EntityKeyFactorySource
        {
            _serviceCollection.AddSingleton<EntityKeyFactorySource, TService>();

            return this;
        }

        public virtual EntityConfigurationBuilder UseClrCollectionAccessorSource<TService>()
            where TService : ClrCollectionAccessorSource
        {
            _serviceCollection.AddSingleton<ClrCollectionAccessorSource, TService>();

            return this;
        }

        public virtual EntityConfigurationBuilder UseClrPropertyGetterSource<TService>()
            where TService : ClrPropertyGetterSource
        {
            _serviceCollection.AddSingleton<ClrPropertyGetterSource, TService>();

            return this;
        }

        public virtual EntityConfigurationBuilder UseClrPropertySetterSource<TService>()
            where TService : ClrPropertySetterSource
        {
            _serviceCollection.AddSingleton<ClrPropertySetterSource, TService>();

            return this;
        }

        public virtual EntityConfigurationBuilder UseEntityMaterializerSource<TService>()
            where TService : EntityMaterializerSource
        {
            _serviceCollection.AddSingleton<EntityMaterializerSource, TService>();

            return this;
        }

        public virtual EntityConfigurationBuilder UseLoggerFactory<TService>()
            where TService : ILoggerFactory
        {
            _serviceCollection.AddSingleton<ILoggerFactory, TService>();

            return this;
        }

        // Scoped by context

        public virtual EntityConfigurationBuilder UseEntityStateListener<TService>()
            where TService : IEntityStateListener
        {
            _serviceCollection.AddScoped<IEntityStateListener, TService>();

            return this;
        }

        public virtual EntityConfigurationBuilder UseStateEntryFactory<TService>()
            where TService : StateEntryFactory
        {
            _serviceCollection.AddScoped<StateEntryFactory, TService>();

            return this;
        }

        public virtual EntityConfigurationBuilder UseStateEntryNotifier<TService>()
            where TService : StateEntryNotifier
        {
            _serviceCollection.AddScoped<StateEntryNotifier, TService>();

            return this;
        }

        public virtual EntityConfigurationBuilder UseContextEntitySets<TService>()
            where TService : ContextEntitySets
        {
            _serviceCollection.AddScoped<ContextEntitySets, TService>();

            return this;
        }

        public virtual EntityConfigurationBuilder UseStateManager<TService>()
            where TService : StateManager
        {
            _serviceCollection.AddScoped<StateManager, TService>();

            return this;
        }
    }
}