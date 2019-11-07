// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Fraction.cs" company="Westerdals ACT">
//
// The MIT License (MIT)
//
// Copyright (c) 2014 Sindri Jóelsson
//               2015 Tomas Sandnes (small changes only)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// </copyright>
// <summary>
//   Defines the Fraction type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Fractions
{
	public class Fraction : IComparable<Fraction>
	{
		#region Properties

		public int Numerator { get; private set; }

		public int Denominator { get; private set; }

		// Note to students: Don't really need any slash or name behind #endregion, but one can add them for clarity.
		#endregion /Properties


		#region Constructors

		// By adding default values for both numerator and denominator, we also cover a parameterless constructor.
		public Fraction(int numerator = 0, int denominator = 1)
		{
			if (denominator != 0) {
				// Denominator should never be negative, only numerator.
				if (denominator < 0) {
					Numerator = -numerator;
					Denominator = -denominator;
				} else {
					Numerator = numerator;
					Denominator = denominator;
				}

				Reduce();

			} else {
				throw new ArgumentOutOfRangeException(nameof(denominator), "value = " + denominator);
			}
		}

		#endregion /Constructors


		#region Instance Methods

		/// <summary>
		/// Reduces this fraction
		/// </summary>
		/// <returns>Nothing (void).</returns>
		private void Reduce()
		{
            int greatestCommonDivisor = (int)MathUtil.GreatestCommonDivisor((uint)Math.Abs(Numerator), (uint)Math.Abs(Denominator));

            Numerator /= greatestCommonDivisor;
            Denominator /= greatestCommonDivisor;

            // Different Reduce solutions, some by earlier students.
            /*
            // Thomas Øhrn Hartmann, 2016:
            var a = Numerator;
            var b = Denominator;
            var gcd = a % b;
            while (gcd > 0) {
                a = b;
                b = gcd;
                gcd = a % b;
            }
            Numerator = Numerator / b;
            Denominator = Denominator / b;

            // Daniel Branil Stoa, 2016:
            for (int i = 2; i < Denominator; i++) {
                if (Numerator % i == 0 && Denominator % i == 0) {
                    Numerator = Numerator / i;
                    Denominator = Denominator / i;
                    i = 1; //so i = 2 when the loop starts again
                }
            }
            */
        }


        /// <summary>
        /// Compares this fraction to another.
        /// </summary>
        /// <param name="other">The fraction to compare with.</param>
        /// <returns>Returns 0 if equal, 1 if greater and -1 if less.</returns>
        public int CompareTo(Fraction other)
		{
			if (this == other) {
				return 0;
			}
			if (this > other) {
				return 1;
			}
			return -1;
		}


		public override string ToString()
		{
			return Numerator + "/" + Denominator;
		}


		public float ToFloat()
		{
			return Numerator / (float)Denominator;
		}


		public double ToDouble()
		{
			return Numerator / (double)Denominator;
		}


		public bool Equals(Fraction other)
		{
			return Numerator * other.Denominator == other.Numerator * Denominator;
		}


		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) {
				return false;
			}

			if (ReferenceEquals(this, obj)) {
				return true;
			}

			return obj.GetType() == GetType() && Equals((Fraction)obj);
		}


		public override int GetHashCode()
		{
			return new {
				// ReSharper disable once NonReadonlyMemberInGetHashCode
				Numerator,
				// ReSharper disable once NonReadonlyMemberInGetHashCode
				Denominator
			}.GetHashCode();
		}

		#endregion /Instance Methods


		#region Arithmetic Operations

		/// <summary>
		/// Reduces a fraction.
		/// </summary>
		/// <param name="f">The fraction to reduce.</param>
		/// <returns>A reduced copy of the fraction.</returns>
		public static Fraction Reduce(Fraction f)
		{
			int greatestCommonDivisor = (int)MathUtil.GreatestCommonDivisor((uint)Math.Abs(f.Numerator), (uint)Math.Abs(f.Denominator));

			return new Fraction(f.Numerator / greatestCommonDivisor, f.Denominator / greatestCommonDivisor);
		}


		public static Fraction Add(Fraction lhs, Fraction rhs)
		{
			int newNumeratorResult = (lhs.Numerator * rhs.Denominator) + (rhs.Numerator * lhs.Denominator);

			int newDenominatorResult = lhs.Denominator * rhs.Denominator;

			return new Fraction(newNumeratorResult, newDenominatorResult);
		}


		public static Fraction Negate(Fraction f)
		{
			return new Fraction(-f.Numerator, f.Denominator);
		}


		public static Fraction Substract(Fraction lhs, Fraction rhs)
		{
			return Add(lhs, -rhs);
		}


		public static Fraction Multiply(Fraction lhs, Fraction rhs)
		{
			return new Fraction(lhs.Numerator * rhs.Numerator, lhs.Denominator * rhs.Denominator);
		}


		public static Fraction Multiply(Fraction lhs, int numberRhs)
		{
			return new Fraction(lhs.Numerator * numberRhs, lhs.Denominator);
		}


		public static Fraction Divide(Fraction lhs, Fraction rhs)
		{
			return Multiply(lhs, Reciprocal(rhs));
		}


		public static Fraction Reciprocal(Fraction f)
		{
			return new Fraction(f.Denominator, f.Numerator);
		}


		public static Fraction Power(Fraction f, int power)
		{
			if (power == 1) {
				return f;
			}

			int numerator = (int)Math.Pow(f.Numerator, Math.Abs(power));
			int denominator = (int)Math.Pow(f.Denominator, Math.Abs(power));

			// (a/b)^-1 == 1/(a/b) == (b/a)
			// If power less than zero return reciprocal.
			return power > 0 ? new Fraction(numerator, denominator) : new Fraction(denominator, numerator);
		}


		public static bool Equals(Fraction lhs, Fraction rhs)
		{
			if (lhs.Numerator == 0 && rhs.Numerator == 0) {
				return true;
			}

			// Denominator is never zero.
			return lhs.Numerator * rhs.Denominator == rhs.Numerator * lhs.Denominator;
		}


		public static bool GreaterThan(Fraction rhs, Fraction lhs)
		{
			// If denominators are the same or if either numerator is zero or if their sign are not the same, comparing is easy by numerator.
			if ((rhs.Denominator == lhs.Denominator) ||
				(rhs.Numerator == 0 || lhs.Numerator == 0) ||
				(Math.Sign(rhs.Numerator) != Math.Sign(lhs.Numerator))) {
				return rhs.Numerator > lhs.Numerator;
			}
			// Else we just have to extend each numerator by opposite denominator and check which result is larger
			return (rhs.Numerator * lhs.Denominator) > (lhs.Numerator * rhs.Denominator);
		}


		public static bool LessThan(Fraction rhs, Fraction lhs)
		{
			return !GreaterThanOrEqual(rhs, lhs);
		}


		public static bool GreaterThanOrEqual(Fraction rhs, Fraction lhs)
		{
			return Equals(lhs, rhs) || GreaterThan(rhs, lhs);
		}


		public static bool LessThanOrEqual(Fraction rhs, Fraction lhs)
		{
			return LessThan(rhs, lhs) || Equals(lhs, rhs);
		}

		#endregion /Arithmetic Operations


		#region Operator Overloads

		public static bool operator ==(Fraction lhs, Fraction rhs)
		{
			return Equals(lhs, rhs);
		}


		public static bool operator !=(Fraction lhs, Fraction rhs)
		{
			return !(lhs == rhs);
		}


		public static bool operator >(Fraction lhs, Fraction rhs)
		{
			return GreaterThan(lhs, rhs);
		}


		public static bool operator <(Fraction lhs, Fraction rhs)
		{
			return LessThan(lhs, rhs);
		}


		public static Fraction operator +(Fraction lhs, Fraction rhs)
		{
			return Add(lhs, rhs);
		}


		public static Fraction operator -(Fraction lhs, Fraction rhs)
		{
			return Substract(lhs, rhs);
		}


		public static Fraction operator *(Fraction lhs, Fraction rhs)
		{
			return Multiply(lhs, rhs);
		}


		public static Fraction operator *(Fraction lhs, int numberRhs)
		{
			return Multiply(lhs, numberRhs);
		}


		public static Fraction operator /(Fraction lhs, Fraction rhs)
		{
			return Divide(lhs, rhs);
		}


		public static Fraction operator ^(Fraction f, int power)
		{
			return Power(f, power);
		}


		public static Fraction operator ++(Fraction f)
		{
			return new Fraction(f.Numerator + f.Denominator, f.Denominator);
		}


		public static Fraction operator --(Fraction f)
		{
			return new Fraction(f.Numerator - f.Denominator, f.Denominator);
		}


		public static Fraction operator -(Fraction f)
		{
			return Negate(f);
		}


		public static implicit operator double (Fraction f)
		{
			return f.ToDouble();
		}


		public static implicit operator float (Fraction f)
		{
			return f.ToFloat();
		}

        public static explicit operator Fraction (double d) {

            int denominator = 1;
            while (d - Math.Floor(d) != 0) {
                d *= 10;
                denominator *= 10;
                if (denominator >= 100000) break; // limit the denominator at the cost of accuracy
            }

            Fraction fr = new Fraction((int)d, denominator);
            fr.Reduce();
            return fr;
        }

        public static explicit operator Fraction (float f) {

            Fraction fr = (Fraction)((double)f);
            return fr;
        }

        #endregion /Operator Overloads
    }
}
