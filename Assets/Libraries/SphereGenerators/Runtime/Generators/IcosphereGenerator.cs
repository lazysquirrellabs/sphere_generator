using Unity.Collections;
using UnityEngine;

namespace LazySquirrelLabs.SphereGenerators.Generators
{
	/// <summary>
	/// Generates spherical meshes, using a regular icosahedron as base.
	/// </summary>
	public sealed class IcosphereGenerator : SphereGenerator
	{
		#region Fields

		/// <summary>
		/// Mesh's triangle indices. These represent triangles of a regular icosahedron, obtained via experimentation.
		/// </summary>
		private static readonly int[] IcosphereIndices =
		{
			 0,  1,  2,
			 0,  3,  1,
			 0,  2,  4,
			 3,  0,  5,
			 0,  4,  5,
			 1,  3,  6,
			 1,  7,  2,
			 7,  1,  6,
			 4,  2,  8,
			 7,  8,  2,
			 9,  3,  5,
			 6,  3,  9,
			 5,  4, 10,
			 4,  8, 10,
			 9,  5, 10,
			 7,  6, 11,
			 7, 11,  8,
			11,  6,  9,
			 8, 11, 10,
			10, 11,  9
		};

		/// <summary>
		/// Mesh's vertices. These represent vertices of a regular icosahedron, obtained via experimentation.
		/// </summary>
		private static readonly Vector3[] IcosphereVertices =
		{
			new(0.8506508f,           0.5257311f,         0f),            // 0
			new(0.000000101405476f,   0.8506507f,        -0.525731f),     // 1
			new(0.000000101405476f,   0.8506506f,         0.525731f),     // 2
			new(0.5257309f,          -0.00000006267203f, -0.85065067f),   // 3
			new(0.52573115f,         -0.00000006267203f,  0.85065067f),   // 4
			new(0.8506508f,          -0.5257311f,         0f),            // 5
			new(-0.52573115f,         0.00000006267203f, -0.85065067f),   // 6
			new(-0.8506508f,          0.5257311f,         0f),            // 7
			new(-0.5257309f,          0.00000006267203f,  0.85065067f),   // 8
			new(-0.000000101405476f, -0.8506506f,        -0.525731f),     // 9
			new(-0.000000101405476f, -0.8506507f,         0.525731f),     // 10
			new(-0.8506508f,         -0.5257311f,         0f)             // 11
		};

		#endregion

		#region Properties

		private protected override NativeArray<int> Indices { get; }

		private protected override NativeArray<Vector3> Vertices { get; }

		#endregion

		#region Setup

		internal IcosphereGenerator(float radius, ushort depth) : base(radius, depth, "Icosphere")
		{
			Indices = new NativeArray<int>(IcosphereIndices, Allocator.Temp);
			Vertices = new NativeArray<Vector3>(IcosphereVertices, Allocator.Temp);
		}

		#endregion
	}
}