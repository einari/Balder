using System;
using System.ComponentModel;
using System.Globalization;
using Balder.Content;
using Balder.Execution;
using Balder.Imaging;
using Ninject;

namespace Balder.Silverlight.TypeConverters
{
	public class UriToImageTypeConverter : TypeConverter
	{
		private readonly IContentManager _contentManager;

		public UriToImageTypeConverter()
			: this(Runtime.Instance.Kernel.Get<IContentManager>())
		{
		}

		public UriToImageTypeConverter(IContentManager contentManager)
		{
			_contentManager = contentManager;
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType.Equals(typeof(string));
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if( value is string )
			{
				var uri = value as string;
				var images = _contentManager.Load<Image>(uri);
				return images;
			}
			return null;
		}
	}
}