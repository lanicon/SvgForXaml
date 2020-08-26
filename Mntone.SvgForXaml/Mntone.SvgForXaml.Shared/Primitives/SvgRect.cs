﻿using System;
using System.Linq;

namespace Mntone.SvgForXaml.Primitives
{
	[System.Diagnostics.DebuggerDisplay("Point = ({this.X}, {this.Y}), Size = {this.Width}x{this.Height}")]
	public struct SvgRect : IEquatable<SvgRect>
	{
		internal SvgRect(float x, float y, float width, float height)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
		}

		internal SvgRect(SvgNumber x, SvgNumber y, SvgNumber width, SvgNumber height)
		{
			this.X = x.Value;
			this.Y = y.Value;
			this.Width = width.Value;
			this.Height = height.Value;
		}

		public float X { get; }
		public float Y { get; }
		public float Width { get; }
		public float Height { get; }

		internal static SvgRect? Parse(string attributeValue)
		{
			if (string.IsNullOrWhiteSpace(attributeValue)) return null;

			var s = attributeValue.Split(new[] { ' ' }).Select(t => float.Parse(t, System.Globalization.CultureInfo.InvariantCulture)).ToArray();
			return new SvgRect(s[0], s[1], s[2], s[3]);
		}

		public bool Equals(SvgRect other) => this.X == other.X && this.Y == other.Y && this.Width == other.Width && this.Height == other.Height;
	}
}