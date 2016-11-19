using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DependencyInjectionContainer;

namespace DITests
{
	[TestClass]
	public class DIContainerTests
	{
		[TestMethod]
		public void RegisterTypeToResolveMoreThanOnceThrowsException()
		{
			// Arrange
			var diContainer = new DIContainer();
			bool argumentExceptionThrown = false;

			// Act
			try
			{
				diContainer.Register<IRepository, EERepository>();
				diContainer.Register<IRepository, EERepository>();
			}
			catch (ArgumentException ex)
			{
				argumentExceptionThrown = true;
			}
			catch
			{
				argumentExceptionThrown = false;
			}

			// Assert
			Assert.AreEqual(argumentExceptionThrown, true);
		}

		[TestMethod]
		public void RetrieveTypeNotRegisteredThrowsException()
		{
			// Arrange
			var diContainer = new DIContainer();
			var argumentExceptionThrown = false;

			// Act
			try
			{
				var instance = diContainer.GetInstance<IRepository>();
			}
			catch (ArgumentException)
			{
				argumentExceptionThrown = true;
			}
			catch
			{
				argumentExceptionThrown = false;
			}

			// Assert
			Assert.AreEqual(argumentExceptionThrown, true);
		}

		[TestMethod]
		public void GetInstanceForIRepository()
		{
			// Arrange
			var diContainer = new DIContainer();
			diContainer.Register<IRepository, EERepository>();

			// Act
			var eeRepository = diContainer.GetInstance<IRepository>();

			// Assert
			Assert.AreNotEqual(eeRepository, null);
			Assert.AreEqual(typeof(EERepository).Name, "EERepository");

		}
	}


	public interface IRepository { }

	public class EERepository : IRepository { }

}
