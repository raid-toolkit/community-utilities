using System;

namespace Raid.Toolkit.Community
{
	public static partial class ModelExtensions
	{
		public static double AsDouble(this Plarium.Common.Numerics.Fixed num)
		{
			return (double)Math.Round(num.m_rawValue / (double)uint.MaxValue, 2);
		}
	}
}
