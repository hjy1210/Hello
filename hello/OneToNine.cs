using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hello
{
    public class OneToNine
    {
        List<int> candidates = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        List<int> candidates2;
        bool Multiply(int A,int B,int C, out int E, out int F)
        {
            int res = C * (10 * A + B);
            E = res / 10;
            F = res % 10;
            if (res >= 100) return false;
            List<int> tmp = new List<int> { 0, A, B, C };
            if (tmp.Contains(E) || tmp.Contains(F)) return false;
            return true;
        }
        bool Add(int D,int E, int F, int G, out int H, out int I)
        {
            int x = D * 10 + E;
            int y = F * 10 + G;
            int xplusy = x + y;
            H = xplusy / 10;
            I = xplusy % 10;
            if (xplusy >= 100) return false;
            List<int> tmp = new List<int> { D, E, F, G };
            if (H!=I && candidates2.Contains(H) && candidates2.Contains(I) && !tmp.Contains(H)&&!tmp.Contains(I))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// A,B,C,D,E,F,G,H,I are distinct digits between 1 and 9, satisfy:
        ///   AB
        /// x  C 
        /// ----
        ///   DE
        /// + FG
        /// ----
        ///   HI
        /// 
        /// Note: The solution is UNIQUE.
        /// </summary>
        public void Solve()
        {
            foreach (var A in candidates)
            {
                foreach (var B in candidates)
                {
                    foreach (var C in candidates)
                    {
                        if (A != B && B != C && A != C)
                        {
                            if (Multiply(A, B, C, out int D, out int E))
                            {
                                candidates2 = (from d in candidates where d != A && d != B && d != C && d != D && d != E select d).ToList();
                                foreach (var F in candidates2)
                                {
                                    foreach (var G in candidates2)
                                    {
                                        if (F == G) continue;
                                        if (Add(D, E, F, G, out int H, out int I))
                                        {
                                            Console.WriteLine($"  {A}{B}\nx  {C}\n----\n  {D}{E}\n+ {F}{G}\n----\n  {H}{I}");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
