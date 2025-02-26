using Unity.Collections;
using UnityEngine;

namespace LazySquirrelLabs.SphereGenerator.Generators
{
	/// <summary>
	/// A sphere generator that uses a cube sphere as basic shape.
	/// </summary>
	public class CubeSphereGenerator : SphereGenerator
	{
		#region Fields

		/// <summary>
		/// A coordinate that when used for all Vector3's axis will deliver a unit vector.
		/// </summary>
		private static readonly float UnitCoordinate = Mathf.Sqrt(1 / 3f);
		
		/// <summary>
		/// Vertices of a cube, obtained via experimentation.
		/// </summary>
		private static readonly Vector3[] CubeSphereVertices =
		{
			new(-UnitCoordinate,  UnitCoordinate, -UnitCoordinate), // 0
			new( UnitCoordinate,  UnitCoordinate, -UnitCoordinate), // 1
			new( UnitCoordinate, -UnitCoordinate, -UnitCoordinate), // 2
			new(-UnitCoordinate, -UnitCoordinate, -UnitCoordinate), // 3
			new(-UnitCoordinate,  UnitCoordinate,  UnitCoordinate), // 4
			new( UnitCoordinate,  UnitCoordinate,  UnitCoordinate), // 5
			new(-UnitCoordinate, -UnitCoordinate,  UnitCoordinate), // 6
			new( UnitCoordinate, -UnitCoordinate,  UnitCoordinate)  // 7
		};
		
		/// <summary>
		/// Indices of a cube, obtained via experimentation.
		/// </summary>
		private static readonly int[] CubeSphereIndices =
		{
			0, 1, 2,
			0, 2, 3,
			0, 4, 1,
			4, 5, 1,
			7, 5, 6,
			6, 5, 4,
			0, 3, 6,
			6, 4, 0,
			1, 5, 7,
			1, 7, 2,
			2, 7, 3,
			7, 6, 3
		};

		#endregion

		#region Properties

		private protected override NativeArray<Vector3> Vertices { get; }
		
		private protected override NativeArray<int> Indices { get; }

		#endregion

		#region Setup

		/// <summary>
		/// <see cref="CubeSphereGenerator"/>'s constructor.
		/// </summary>
		/// <param name="radius">The radius of the generated cube spheres.</param>
		/// <param name="depth">The fragmentation depth of the generated spheres. In order words, how many times the
		/// basic shape will be fragmented to form the sphere mesh. The larger the value, the greater the level of
		/// detail will be (more triangles and vertices) and the longer the generation process takes.</param>
		public CubeSphereGenerator(float radius, ushort depth) : base(depth, "Cube Sphere")
		{
			var vertices = new NativeArray<Vector3>(CubeSphereVertices.Length, Allocator.Temp,
			                                        NativeArrayOptions.UninitializedMemory);

			for (var i = 0; i < CubeSphereVertices.Length; ++i)
			{
				vertices[i] = CubeSphereVertices[i] * radius;
			}

			Vertices = vertices;
			Indices = new NativeArray<int>(CubeSphereIndices, Allocator.Temp);
		}

		#endregion
	}
}