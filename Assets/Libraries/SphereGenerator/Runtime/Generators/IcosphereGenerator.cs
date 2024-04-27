using UnityEngine;

namespace LazySquirrelLabs.SphereGenerator.Generators
{
	/// <summary>
	/// Generates spherical meshes, using a regular icosahedron as base.
	/// </summary>
	internal sealed class IcosphereGenerator : SphereGenerator
	{
		#region Fields

		/// <summary>
		/// Mesh's triangle indices. These represent triangles of a regular icosahedron, obtained via experimentation.
		/// </summary>
		private static readonly int[] IcosphereIndices =
		{
			0, 1, 2,
			0, 3, 1,
			0, 2, 4,
			3, 0, 5,
			0, 4, 5,
			1, 3, 6,
			1, 7, 2,
			7, 1, 6,
			4, 2, 8,
			7, 8, 2,
			9, 3, 5,
			6, 3, 9,
			5, 4, 10,
			4, 8, 10,
			9, 5, 10,
			7, 6, 11,
			7, 11, 8,
			11, 6, 9,
			8, 11, 10,
			10, 11, 9
		};

		/// <summary>
		/// Mesh's vertices. These represent vertices of a regular icosahedron, obtained via experimentation.
		/// </summary>
		private static readonly Vector3[] IcosphereVertices =
		{
			new(0.8506508f, 0.5257311f, 0f),
			new(0.000000101405476f, 0.8506507f, -0.525731f),
			new(0.000000101405476f, 0.8506506f, 0.525731f),
			new(0.5257309f, -0.00000006267203f, -0.85065067f),
			new(0.52573115f, -0.00000006267203f, 0.85065067f),
			new(0.8506508f, -0.5257311f, 0f),
			new(-0.52573115f, 0.00000006267203f, -0.85065067f),
			new(-0.8506508f, 0.5257311f, 0f),
			new(-0.5257309f, 0.00000006267203f, 0.85065067f),
			new(-0.000000101405476f, -0.8506506f, -0.525731f),
			new(-0.000000101405476f, -0.8506507f, 0.525731f),
			new(-0.8506508f, -0.5257311f, 0f)
		};

		#endregion

		#region Properties

		private protected override int[] Indices => IcosphereIndices;
		private protected override Vector3[] Vertices => IcosphereVertices;

		#endregion

		#region Setup

		internal IcosphereGenerator(float radius, ushort fragmentationDepth) : base(radius, fragmentationDepth) { }

		#endregion
	}
}