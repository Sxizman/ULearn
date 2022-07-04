namespace Pluralize
{
	public static class PluralizeTask
	{
		public static string PluralizeRubles(int count)
		{
			if (count % 100 / 10 != 1)
            {
				int lastDigit = count % 10;
				if (lastDigit == 1)
					return "рубль";
				if (lastDigit >= 2 && lastDigit <= 4)
					return "рубля";
            }

			return "рублей";
		}
	}
}