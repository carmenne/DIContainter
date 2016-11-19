using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace DependencyInjectionContainer
{
	/// <summary>
	/// Dependency Injection Container with basic functionality
	/// The internal data structure is a Dictionary that maps 
	/// types to be resolves (interfaces) to resolved types
	/// (concrete implementations of the interface)
	/// </summary>
    public class DIContainer
    {
		private Dictionary<string, string> TypeResolver;
		private Dictionary<string, object> TypeResolverSingleton;

		public DIContainer()
		{
			TypeResolver = new Dictionary<string, string>();
			TypeResolverSingleton = new Dictionary<string, object>();
		}


		public void Register<U, V>() where V: new()
		{
			TypeResolver.Add(typeof(U).Name, typeof(V).Name);
		}


		public T GetInstance<T>()
		{
			return (T)GetInstance(typeof(T).Name);
		}
		private object GetInstance(string name)
		{
			var className = LookupType(name);
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			var type = assemblies.Where(a => (a.GetTypes().Where(p => p.Name == className).Any())).First().
						GetTypes().Where(p => p.Name == className).First();

			return Activator.CreateInstance(type); ;

		}

		private string LookupType(string name)
		{
			if (!TypeResolver.ContainsKey(name))
				throw new ArgumentException("Type is not registered. Register type before retrieving it!");

			var resolved = TypeResolver[name];

			return resolved;
		}
	}
}
