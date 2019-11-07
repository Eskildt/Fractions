using System;

namespace Fractions
{
	internal static class Program
	{
		static void Main()
		{
			// Some test code. Could have added MORE tests here.
			// NOTE: At a later lecture (number 9ish), we'll look at a SEXIER 
			//       way of testing the fraction class! (Unit testing!)

			Fraction empty = new Fraction();
			Fraction oneThird = new Fraction(1, 3);
			Fraction twoThird = new Fraction(2, 3);
			Fraction twoFourth = new Fraction(2, 4);

			Console.WriteLine("\n\nEmpty fraction is initialized to: {0}", empty);

			Console.WriteLine("\n\n2/4 is automatically reduced to: {0}", twoFourth);

			Fraction addedTogether = oneThird + twoFourth;
			Console.WriteLine("\n\n{0} + {1} = {2}", oneThird, twoFourth, addedTogether);

			Fraction multipliedTogether = twoThird * twoFourth;
			Console.WriteLine("\n\n{0} * {1} = {2}", twoThird, twoFourth, multipliedTogether);

			// Hack to not end main without a keypress in debug mode.
			Console.Write("\n\nPress any key to continue...");
			Console.ReadKey(true);
		}
	}
}
