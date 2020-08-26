﻿using Mntone.SvgForXaml.Interfaces;
using Mntone.SvgForXaml.Internal;
using Mntone.SvgForXaml.Primitives;
using Mntone.SvgForXaml.Shapes;
using System;
using Windows.Data.Xml.Dom;

namespace Mntone.SvgForXaml
{
	public sealed class SvgUseElement : SvgElement, ISvgStylable, ISvgTransformable
	{
		private Lazy<SvgElement> _instanceRoot;

		internal SvgUseElement(INode parent, XmlElement element)
			: base(parent, element)
		{
			this._stylableHelper = new SvgStylableHelper(this, element);
			this._transformableHelper = new SvgTransformableHelper(element);

			var nan = new SvgLength(SvgLength.SvgLengthType.Unknown, float.NaN);
			this.X = element.ParseCoordinate("x", nan);
			this.Y = element.ParseCoordinate("y", nan);
			this.Width = element.ParseLength("width", nan);
			this.Height = element.ParseLength("height", nan);
			this.Href = element.GetAttributeNS("http://www.w3.org/1999/xlink", "href").Substring(1);
			this._instanceRoot = new Lazy<SvgElement>(this.CreateInstanceRoot, true);

			if (this.X.UnitType != SvgLength.SvgLengthType.Unknown && this.Y.UnitType != SvgLength.SvgLengthType.Unknown)
			{
				this.Transform.Add(SvgTransform.CreateTranslate(
					this.X.UnitType != SvgLength.SvgLengthType.Unknown ? this.X.ValueAsPixel : 0.0F,
					this.Y.UnitType != SvgLength.SvgLengthType.Unknown ? this.Y.ValueAsPixel : 0.0F));
			}
		}

		private SvgElement CreateInstanceRoot()
		{
			var refNode = this.OwnerDocument.GetElementById(this.Href);
			if (refNode == null)
				throw new NullReferenceException($"No element with id = {this.Href}");

			var child = (SvgElement)refNode.CloneNode(true);
			child.ParentNode = this;
			if (child.GetType() == typeof(SvgSymbolElement))
			{
				throw new NotImplementedException();
			}
			else if (child.GetType() == typeof(SvgRectElement))
			{
				var casted = (SvgRectElement)child;
				if (this.Width.UnitType != SvgLength.SvgLengthType.Unknown) casted.Width = this.Width;
				if (this.Height.UnitType != SvgLength.SvgLengthType.Unknown) casted.Height = this.Height;
			}
			return child;
		}

		protected override void DeepCopy(SvgElement element)
		{
			var casted = (SvgUseElement)element;
			casted._instanceRoot = new Lazy<SvgElement>(this.CreateInstanceRoot, true);
			casted._stylableHelper = this._stylableHelper.DeepCopy(casted);
			casted._transformableHelper = this._transformableHelper.DeepCopy();
		}

		public override string TagName => "use";
		public SvgLength X { get; }
		public SvgLength Y { get; }
		public SvgLength Width { get; }
		public SvgLength Height { get; }
		public string Href { get; }
		public SvgElement InstanceRoot => this._instanceRoot.Value;

		#region ISvgStylable
		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		private SvgStylableHelper _stylableHelper;
		public string ClassName => this._stylableHelper.ClassName;
		public CssStyleDeclaration Style => this._stylableHelper.Style;
		public ICssValue GetPresentationAttribute(string name) => this._stylableHelper.GetPresentationAttribute(name);
		#endregion

		#region ISvgLocatable
		public SvgRect GetBBox() => SvgLocatableHelper.GetUnionBBox(this);
		#endregion

		#region ISvgTransformable
		[System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
		private SvgTransformableHelper _transformableHelper;
		public SvgTransformCollection Transform => this._transformableHelper.Transform;
		#endregion
	}
}