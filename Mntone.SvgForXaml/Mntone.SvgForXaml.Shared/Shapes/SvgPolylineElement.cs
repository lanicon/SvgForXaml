﻿using Mntone.SvgForXaml.Interfaces;
using Mntone.SvgForXaml.Internal;
using Mntone.SvgForXaml.Primitives;
using Windows.Data.Xml.Dom;

namespace Mntone.SvgForXaml.Shapes
{
	public sealed class SvgPolylineElement : SvgElement, ISvgStylable, ISvgTransformable
	{
		internal SvgPolylineElement(INode parent, XmlElement element)
			: base(parent, element)
		{
			this._stylableHelper = new SvgStylableHelper(this, element);
			this._transformableHelper = new SvgTransformableHelper(element);

			this.Points = new SvgPointCollection(element.GetAttribute("points"));
		}

		protected override void DeepCopy(SvgElement element)
		{
			var casted = (SvgPolylineElement)element;
			casted._stylableHelper = this._stylableHelper.DeepCopy(casted);
			casted._transformableHelper = this._transformableHelper.DeepCopy();
		}

		public override string TagName => "polyline";
		public SvgPointCollection Points { get; }

		#region ISvgStylable
		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		private SvgStylableHelper _stylableHelper;
		public string ClassName => this._stylableHelper.ClassName;
		public CssStyleDeclaration Style => this._stylableHelper.Style;
		public ICssValue GetPresentationAttribute(string name) => this._stylableHelper.GetPresentationAttribute(name);
		#endregion

		#region ISvgLocatable
		public SvgRect GetBBox()
		{
			float left = float.MaxValue, top = float.MinValue, right = float.MaxValue, bottom = float.MinValue;
			foreach (var point in this.Points)
			{
				if (point.X < left) left = point.X;
				if (point.X > right) right = point.X;
				if (point.Y < top) top = point.Y;
				if (point.Y > bottom) bottom = point.Y;
			}
			return new SvgRect(left, top, right - left, bottom - top);
		}
		#endregion

		#region ISvgTransformable
		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		private SvgTransformableHelper _transformableHelper;
		public SvgTransformCollection Transform => this._transformableHelper.Transform;
		#endregion
	}
}