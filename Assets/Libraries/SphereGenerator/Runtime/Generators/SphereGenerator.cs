using Unity.Collections;
using UnityEngine;

namespace LazySquirrelLabs.SphereGenerator.Generators
{
	/// <summary>
	/// Base class for all shape generators.
	/// </summary>
	internal abstract class SphereGenerator
	{
		#region Fields

		protected const int MaxVertexCountUInt16 = 65_535;

		#endregion
		
		#region Internal

		/// <summary>
		/// Generates the shape.
		/// </summary>
		/// <param name="allocator">The allocation strategy used when creating vertex and index buffers.</param>
		/// <returns>The generated mesh.</returns>
		internal abstract Mesh Generate(Allocator allocator);

		#endregion
	}
}