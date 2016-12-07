namespace AkkaPrez.App.Calc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ControlDigitAlgorithm
    {
        Func<long, IEnumerable<int>> GetDigitsOf { get; }
        IEnumerable<int> MultiplyingFactors { get; }
        int Modulo { get; }

        public ControlDigitAlgorithm(Func<long, IEnumerable<int>> getDigitsOf,
            IEnumerable<int> multiplyingFactors, int modulo)
        {
            GetDigitsOf = getDigitsOf;
            MultiplyingFactors = multiplyingFactors;
            Modulo = modulo;
        }

        public int GetControlDigit(long number) =>
                GetDigitsOf(number)
                    .Zip(this.MultiplyingFactors, (a, b) => a * b)
                    .Sum()
                % Modulo;
    }

    static class ControlDigitAlgorithms
    {

        public static ControlDigitAlgorithm ForMSFest =>
            new ControlDigitAlgorithm(x => x.DigitsFromLowest(), MultiplyingFactors, 9);

        private static IEnumerable<int> MultiplyingFactors
        {
            get
            {
                int factor = 3;
                while (true)
                {
                    yield return factor;
                    factor = 4 - factor;
                }
            }
        }
    }


}