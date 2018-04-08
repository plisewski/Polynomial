using System;
using System.Linq;

namespace Polynomial
{
    public class Polynomial<T> : IComparable<Polynomial<T>>, ICloneable where T : IComparable
    {
        private T[] _a;

        public T[] A
        {
            get => _a;
            set => _a = value;
        }

        public Polynomial(int polynomialDegree) => _a = new T[polynomialDegree];


        public override string ToString()
        {
            string polynomial = "";

            for (int i = 0; i < _a.Length; i++)
            {
                if (i == 0)
                {
                    polynomial = _a[0] + "*x";
                }
                else
                {
                    int degree = i + 1;
                    polynomial += " + " + _a[i] + "*x^" + degree;
                }
            }

            return $"y[x] = " + polynomial;
        }

        public static Polynomial<T> operator +(Polynomial<T> w, Polynomial<T> p)
        {
            var s = new Polynomial<T>(HigherPolynomialDegree(w, p));

            for (int i = 0; i < w._a.Length; i++)
                s._a[i] = w._a[i];

            for (int i = 0; i < p._a.Length; i++)
            {
                dynamic dynamic = p._a[i];
                s._a[i] += dynamic;
            }

            return s;
        }

        public static Polynomial<T> operator -(Polynomial<T> w, Polynomial<T> p)
        {
            var s = new Polynomial<T>(HigherPolynomialDegree(w, p));

            for (int i = 0; i < w._a.Length; i++)
                s._a[i] = w._a[i];

            for (int i = 0; i < p._a.Length; i++)
            {
                dynamic dynamic = p._a[i];
                s._a[i] -= dynamic;
            }

            return s;
        }

        public static Polynomial<T> operator *(Polynomial<T> w, Polynomial<T> p)
        {
            var s = new Polynomial<T>(w._a.Length + p._a.Length);

            for (int i = 0; i < w._a.Length; i++)
            {
                for (int j = 0; j < p._a.Length; j++)
                {
                    s._a[i + j + 1] += (dynamic)w._a[i] * (dynamic)p._a[j];
                }
            }

            return s;
        }

        public static string operator +(Polynomial<T> w, T constantTerm) => w + " + " + constantTerm;

        public static string operator -(Polynomial<T> w, T constantTerm) => w + " - " + constantTerm;

        public static Polynomial<T> operator *(Polynomial<T> w, T x)
        {
            var s = new Polynomial<T>(w._a.Length);

            for (int i = 0; i < w._a.Length; i++)
            {
                s._a[i] = (dynamic)w._a[i] * x;
            }

            return s;
        }

        /// <summary>
        /// Returns 1 if polynomials are equall or -1 if they are diffrent.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public int CompareTo(Polynomial<T> p)
        {
            if (_a.Length != p?._a.Length)
                return -1;
            for (int i = 0; i < _a.Length; i++)
            {
                if (!_a[i].Equals(p._a[i]))
                    return -1;
            }
            return 1;
        }

        public object Clone() => MemberwiseClone();

        public T PolynomialValueOfX(T x)
        {
            T y = default(T);

            for (int i = 0; i < _a.Length; i++)
            {
                y += _a[i] * Math.Pow(Convert.ToDouble((dynamic)x), i + 1);
            }

            return y;
        }

        public T PolynomialValueOfManyX(params T[] x)
        {
            T y = default(T);

            return x.Aggregate(y, (current, item) => (T)(current + PolynomialValueOfManyX((dynamic)item)));
        }

        public T CountIntervals(T beginning, T end, T step)
        {
            if ((dynamic)end < (dynamic)beginning)
                throw new ArgumentException("End of the interval cannot be before the beginning");

            T numberOfPoints = ((dynamic)end - (dynamic)beginning) / (dynamic)step;
            T x = beginning;
            T y = PolynomialValueOfX(x);

            for (int i = 1; i <= (dynamic)numberOfPoints; i++)
            {
                dynamic step1 = step;
                x += step1;
                y += PolynomialValueOfX((dynamic)x);
            }

            return y;
        }

        private static int HigherPolynomialDegree(Polynomial<T> w, Polynomial<T> p) => w._a.Length >= p._a.Length ? w._a.Length : p._a.Length;
    }
}