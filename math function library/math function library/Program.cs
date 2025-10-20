using Microsoft.VisualBasic;
using System.Globalization;

namespace math_function_library
{
    
    internal class Program
    {
        static void Main(string[] args)
        {
            LinearFunction trollFunkce = new LinearFunction("f", 3, 5);
            Console.WriteLine(trollFunkce.getDomain());
        }
    }

    abstract class MathFunction
    {
        String name;
        List<Double> constants = new List<Double>();
        public Interval Domain { get; private set; }
        public Interval Range { get; private set; }

        abstract public Double getValue(Double x);
        abstract public string getDescription();
        abstract public string getDomain();
        abstract public string getRange();

        public void setDomain(Double lowerLimit, Double upperLimit, bool lowerClosed=false, bool upperClosed=false)
        {
            this.Domain = new Interval(lowerLimit, upperLimit, lowerClosed, upperClosed);
        }
    }

    class LinearFunction : MathFunction
    {
        String name;
        private List<Double> constants = new List<Double>();
        public override Double getValue(Double x)
        {
            return constants[0] * x + constants[1];
        }
        public override string getDescription()
        {
            string output = "{name}(x) = ";
            if (constants[0] != 0)
            {
                output += $"{constants[0]}x ";
            }
            if (constants[1] > 0)
            {
                output += $"+{constants[1]}";
            }
            if (constants[1] == 0 & constants[0] == 0)
            {
                output += "0";
            }
            if (constants[1] < 0)
            {
                output += constants[1];
            }
            return output;
        }
        public override string getDomain()
        {
            return Domain.ToString();
        }
        public override string getRange()
        {
            return Range.ToString();
        }
        public LinearFunction(string name, int a, int b)
        {
            this.name = name;
            constants.Add(a);
            constants.Add(b);

            this.setDomain(Double.NegativeInfinity, Double.PositiveInfinity);
        }
    }
    struct Interval
    {
        public Double lowerLimit;
        public Double upperLimit;

        public bool lowerClosed;
        public bool upperClosed;

        public override string ToString()
        {
            String output = "";
            if (lowerClosed)
            {
                output += '[';
            }
            else
            {
                output += '(';
            }
            output += $"{lowerLimit.ToString()}, " + (upperLimit+10.0).ToString();
            if (upperClosed)
            {
                output += ']';
            }
            else
            {
                output += ')';
            }
            return output;
        }
        public Interval(Double lowerLimit, Double upperLimit, bool lowerClosed, bool upperClosed)
        {
            this.lowerClosed = lowerClosed;
            this.upperClosed = upperClosed;
            this.lowerLimit = lowerLimit;
            this.upperLimit = upperLimit;
            if(lowerLimit == Double.NegativeInfinity)
            {
                this.lowerClosed = false;
            }
            if(upperLimit == Double.PositiveInfinity)
            {
                this.upperClosed = false;
            }
        }
    }
}
