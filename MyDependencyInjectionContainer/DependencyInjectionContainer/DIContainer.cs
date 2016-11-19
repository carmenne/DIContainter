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

		/// <summary>
		/// Registers the TypeToResolve(interface) and resolved type (implementation)
		/// in a dictionary.
		/// The registration can be done for TypeToResolve only once.
		/// If attempted more than once, ArgumentException is thrown
		/// </summary>
		/// <typeparam name="U"></typeparam>
		/// <typeparam name="V"></typeparam>
		public void Register<U, V>() where V: new()
		{
			TypeResolver.Add(typeof(U).Name, typeof(V).Name);
		}

		/// <summary>
		/// Wrapper for object GetInstance
		/// Casts the object to the resolved type that corresponds to an actual implementations
		/// </summary>
		/// <typeparam name="T">Type of the interface that should be resolved to an actual implementation</typeparam>
		/// <returns>The instance of the type that represents the implementation</returns>
		public T GetInstance<T>()
		{
			return (T)GetInstance(typeof(T).Name);
		}

		/// <summary>
		/// Created an instance corresponding to the lookuped name in dictionary
		/// </summary>
		/// <param name="name">Used to search the correspoinding type (of an implementation) in dictionary</param>
		/// <returns>Returns an instance (type object) of the resolved type</returns>
		private object GetInstance(string name)
		{
			var className = LookupType(name);
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			var type = assemblies.Where(a => (a.GetTypes().Where(p => p.Name == className).Any())).First().
						GetTypes().Where(p => p.Name == className).First();

			return Activator.CreateInstance(type); ;

		}

		/// <summary>
		/// Lookup method that search after the key given as input parameter
		/// </summary>
		/// <param name="name">key used to search in dictionary</param>
		/// <returns>Gets the value from the dictionary corresponding to the key given as parameter.
		/// Returns exception if the value is not present</returns>
		private string LookupType(string name)
		{
			if (!TypeResolver.ContainsKey(name))
				throw new ArgumentException("Type is not registered. Register type before retrieving it!");

			var resolved = TypeResolver[name];

			return resolved;
		}
	}
}
