﻿using Mntone.SvgForXaml.Interfaces;
using Mntone.SvgForXaml.Internal;
using Mntone.SvgForXaml.Primitives;
using Windows.Data.Xml.Dom;

namespace Mntone.SvgForXaml
{
	public sealed class SvgDefinitionsElement : SvgElement, ISvgStylable, ISvgTransformable
	{
		internal SvgDefinitionsElement(INode parent, XmlElement element)
			: base(parent, element)
		{
			this._stylableHelper = new SvgStylableHelper(this, element);
			this._transformableHelper = new SvgTransformableHelper(element);
		}

		protected override void DeepCopy(SvgElement element)
		{
			var casted = (SvgDefinitionsElement)element;
			casted._stylableHelper = this._stylableHelper.DeepCopy(casted);
			casted._transformableHelper = this._transformableHelper.DeepCopy();
		}

		public override string TagName => "defs";

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