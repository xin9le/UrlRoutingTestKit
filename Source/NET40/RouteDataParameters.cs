using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Routing;



namespace UrlRoutingTestKit
{
	/// <summary>
	/// Provides the route parameters for test.
	/// </summary>
	internal class RouteDataParameters : DynamicObject
	{
		/// <summary>
		/// Hold route parameters except controller and action.
		/// </summary>
		private readonly Dictionary<string, object> parameters = null;


		/// <summary>
		/// Initializes a new instance from the specified route information.
		/// </summary>
		/// <param name="data">route information</param>
		public RouteDataParameters(RouteData data)
		{
			//--- Hold copied parameters
			this.parameters	= data.Values.ToDictionary(x => x.Key, x => x.Value);
		}


		/// <summary>
		/// Provides the implementation for operations that invoke a member.
		/// </summary>
		/// <param name="binder">Provides information about the dynamic operation.</param>
		/// <param name="args">The arguments that are passed to the object member during the invoke operation.</param>
		/// <param name="result">The result of the member invocation.</param>
		/// <returns>Whether the operation has succeeded.</returns>
		public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
		{
			//--- Check testable
			result = null;
			if (args.Length != 1)
				Assert.Fail("An argument is required for this method.");
			
			//--- Test parameter and chain this again
			this.Test(binder.Name, args[0]);
			result = this;
			return true;
		}


		/// <summary>
		/// Tests the parameter which has specified name.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="expect">Expected value.</param>
		public void Test(string name, object expect)
		{
			//--- Check testable
			if (!this.parameters.ContainsKey(name))
				Assert.Fail("Parameter '{0}' does not exist.", name);
	
			//--- Test
			var actual = this.parameters[name];
			if (expect != null)
			if (actual != null)
			if (expect.GetType() != actual.GetType())
			{
				//--- Compare as string if each types are different.
				expect = (expect == null) ? null : expect.ToString();
				actual = (actual == null) ? null : actual.ToString();
			}
			Assert.AreEqual(expect, actual);

			//--- Remove checked parameter (don't allow to recheck)
			this.parameters.Remove(name);
		}
	}
}