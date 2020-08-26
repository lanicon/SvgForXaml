﻿using Mntone.SvgForXaml.Interfaces;
using Mntone.SvgForXaml.Internal;
using Mntone.SvgForXaml.Primitives;
using Windows.Data.Xml.Dom;

namespace Mntone.SvgForXaml.Gradients
{
	[System.Diagnostics.DebuggerDisplay("RadialGradient: Center = ({this.CenterX}, {this.CenterY}), Radius = {this.Radius}, Focus = ({this.FocusX}, {this.FocusY})")]
	public sealed class SvgRadialGradientElement : SvgGradientElement
	{
		internal SvgRadialGradientElement(INode parent, XmlElement element)
			: base(parent, element)
		{
			this.CenterX = element.ParseCoordinate("cx", 0.5F);
			this.CenterY = element.ParseCoordinate("cy", 0.5F);
			this.Radius = element.ParseLength("r", 0.5F);
			this.FocusX = element.ParseCoordinate("fx", this.CenterX);
			this.FocusY = element.ParseCoordinate("fy", this.CenterY);
		}

		public override string TagName => "radialGradient";
		public SvgLength CenterX { get; }
		public SvgLength CenterY { get; }
		public SvgLength Radius { get; }
		public SvgLength FocusX { get; }
		public SvgLength FocusY { get; }

		#region ISvgLocatable
		public override SvgRect GetBBox()
		{
			var radius = this.Radius.ValueAsPixel;
			var diameter = 2.0F * radius;
			return new SvgRect(this.CenterX.ValueAsPixel - radius, this.CenterY.ValueAsPixel - radius, diameter, diameter);
		}
		#endregion
	}
}