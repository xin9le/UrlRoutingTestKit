using System;
using System.Collections.Generic;



namespace UrlRoutingTestKit
{
	/// <summary>
	/// Provides IEnumerable&lt;T&gt; extension methods.
	/// </summary>
	internal static class EnumerableExtensions
	{
		/// <summary>
		/// Performs the specified action on each element of the specified collection.
		/// </summary>
		/// <typeparam name="T">The type of elements of the collection.</typeparam>
		/// <param name="collection">Enumeratable object collection.</param>
		/// <param name="action">The Action&lt;T&gt; to perform on each element of collection.</param>
		public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
		{
			foreach (var item in collection)
				action(item);
		}
	}
}