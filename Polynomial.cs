using System;

namespace Polynomial
{
    public class Polynomial<T> : IComparable<Polynomial<T>>, ICloneable where T : IComparable
    {
        private T[] a;

        public T[] A
        {
            get { return a; }
            set { a = value; }
        }

        public Polynomial(int polynomialDegree)
        {
            a = new T[polynomialDegree];
        }

        public override string ToString()
        {
            string polynomial = "";

            for (int i = 0; i < a.Length; i++)
            {
                if (i == 0)
                {
                    polynomial = a[0] + "*x";
                }
                else
                {
                    int degree = i + 1;
                    polynomial += " + " + a[i] + "*x^" + degree;
                }
            }

            return "y[x] = " + polynomial;
        }

        public static Polynomial<T> operator +(Polynomial<T> w, Polynomial<T> p)
        {
            var s = new Polynomial<T>(HigherPolynomialDegree(w, p));

            for (int i = 0; i < w.a.Length; i++)
                s.a[i] = w.a[i];

            for (int i = 0; i < p.a.Length; i++)
                s.a[i] += (dynamic)p.a[i];

            return s;
        }

        public static Polynomial<T> operator -(Polynomial<T> w, Polynomial<T> p)
        {
            var s = new Polynomial<T>(HigherPolynomialDegree(w, p));

            for (int i = 0; i < w.a.Length; i++)
                s.a[i] = w.a[i];

            for (int i = 0; i < p.a.Length; i++)
                s.a[i] -= (dynamic)p.a[i];

            return s;
        }

        public static Polynomial<T> operator *(Polynomial<T> w, Polynomial<T> p)
        {
            var s = new Polynomial<T>(w.a.Length + p.a.Length);

            for (int i = 0; i < w.a.Length; i++)
            {
                for (int j = 0; j < p.a.Length; j++)
                {
                    s.a[i + j + 1] += (dynamic)w.a[i] * (dynamic)p.a[j];
                }
            }

            return s;
        }

        public static string operator +(Polynomial<T> w, T constantTerm)
        {
            return w.ToString() + " + " + constantTerm;
        }

        public static string operator -(Polynomial<T> w, T constantTerm)
        {
            return w.ToString() + " - " + constantTerm;
        }

        public static Polynomial<T> operator *(Polynomial<T> w, T x)
        {
            var s = new Polynomial<T>(w.a.Length);

            for (int i = 0; i < w.a.Length; i++)
            {
                s.a[i] = (dynamic)w.a[i] * x;
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
            if (p == null)
                return -1;
            else if (a.Length != p.a.Length)
                return -1;
            else
            {
                for (int i = 0; i < a.Length; i++)
                {
                    if (!a[i].Equals(p.a[i]))
                        return -1;
                }
                return 1;
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public T PolynomialValueOfX(T x)
        {
            T y = default(T);

            for (int i = 0; i < a.Length; i++)
            {
                y += a[i] * Math.Pow(Convert.ToDouble((dynamic)x), i + 1);                
            }

            return y;
        }

        public T PolynomialValueOfManyX(params T[] x)
        {
            T y = default(T);

            foreach (var item in x)
                y += PolynomialValueOfX((dynamic)item);

            return y;
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
                x += (dynamic)step;
                y += PolynomialValueOfX((dynamic)x);
            }

            return y;
        }

        private static int HigherPolynomialDegree(Polynomial<T> w, Polynomial<T> p)
        {
            if (w.a.Length >= p.a.Length)
                return w.a.Length;
            else
                return p.a.Length;
        }

    }
}
