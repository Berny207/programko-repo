using Microsoft.VisualBasic;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Xml;


namespace math_function_library
{
    
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            List<MathFunction> functions = new List<MathFunction>();
            functions.Add(new ConstantFunction("a", [4]));
            functions.Add(new LinearFunction("b", [3, -5]));
            functions.Add(new QuadraticFunction("c", [-4, 2, 8]));
            functions.Add(new LinearAbsoluteValueFunction("d", [-6, -2, 5, 2, 0]));
            functions.Add(new RationalFunction("e", [6, 2, -8, 3]));
            double x = double.Parse(Console.ReadLine());
            foreach(MathFunction function in functions)
            {
                Console.WriteLine(function.getDescription());
                foreach(Interval interval in function.getDomain())
                {
                    Console.Write(interval.ToString() + " ");
                }
                Console.WriteLine();
                Console.WriteLine(function.getValue(x));
                if(function is IDerivable)
                {
                    Console.WriteLine(function.derivate().getDescription());
                }
                if(function is IInvertable)
                {
                    Console.WriteLine(function.invert().getDescription());
                }
            }
        }
    }

    abstract class MathFunction
    {

        public string name { get; set; }
        static public int constantsAmount { get; protected set; }
		protected ArgumentException differentArgumentAmount = new ArgumentException($"This function requires {constantsAmount} arguments.");
		/// <summary>
		/// in base polynomial functions i-th constant corresponds to the amount of x^i
		/// </summary>
		public List<double> constants = new List<double>();
        protected List<Interval> range = new List<Interval>();
		protected List<Interval> domain = new List<Interval>();
        abstract public double getValue(double x);
        abstract public string getDescription();
        abstract public void calculateDomain();
        abstract public void calculateRange();
        public ReadOnlyCollection<Interval> getDomain()
        {
            return domain.AsReadOnly();
        }
		public ReadOnlyCollection<Interval> getRange()
		{
			return range.AsReadOnly();
		}
        protected void checkConstantAmount(double[] constants, int constantAmount)
        {
			if (constants.Length != constantsAmount)
			{
				throw differentArgumentAmount;
			}
		}

        public MathFunction derivate()
        {
            var derivable = this as IDerivable;
            if (derivable != null)
            {
                return derivable.derivate();
            }
            throw new Exception("Function cannot be derivated");
        }
        public MathFunction invert()
        {
			var invertable = this as IInvertable;
			if (invertable != null)
			{
				return invertable.invert();
			}
			throw new Exception("Function cannot be inverted");
		}
	}
    class ConstantFunction : MathFunction, IDerivable
    {
		override public double getValue(double x)
        {
            return constants[0];
        }
        override public void calculateDomain()
        {
            domain.Add(new Interval(double.NegativeInfinity, double.PositiveInfinity));
        }
		public override void calculateRange()
		{
			range.Add(new Interval(constants[0], constants[0], true, true));
		}
		public override string getDescription()
		{
            return $"{this.name}(x) = {constants[0]}";
		}
        public MathFunction derivate()
        {
            return new ConstantFunction($"{this.name}`", [0]);
        }
        public ConstantFunction(string name, double[] constants)
        {
            constantsAmount = 1;
            this.checkConstantAmount(constants, constantsAmount);
			this.name = name;
			this.constants = constants.Reverse().ToList();
			this.calculateDomain();
            this.calculateRange();
        }
    }
    class LinearFunction : MathFunction, IDerivable, IInvertable
    {
        public override double getValue(double x)
        {
            return constants[1] * x + constants[0];
        }
        public override void calculateDomain()
        {
            domain.Add(new Interval(double.NegativeInfinity, double.PositiveInfinity));
        }
        public override void calculateRange()
        {
            range.Add(new Interval(double.NegativeInfinity, double.PositiveInfinity));
        }
        public override string getDescription()
        {
            return $"{this.name}(x) = {constants[1]}x {constants[0].ToString("+ 0;- 0")}";
        }
		public MathFunction derivate()
        {
			return new ConstantFunction($"{this.name}`", [constants[1]]);
		}
        public MathFunction invert()
        {
            return new LinearFunction($"I{this.name}", [1 / constants[1], constants[0]]);
        }
        public LinearFunction(string name, double[] constants)
        {
			constantsAmount = 2;
			this.checkConstantAmount(constants, constantsAmount);
			this.name = name;
			this.constants = constants.Reverse().ToList();
			if (constants[0] == 0)
            {
                throw new ArgumentException($"First argument cannot be 0. Create a constant function instead.");
            }
            this.calculateDomain();
            this.calculateRange();
        }
    }
    class QuadraticFunction : MathFunction, IDerivable
    {
		public override double getValue(double x)
		{
			return constants[2]*Math.Pow(x, 2) + constants[1]*x + constants[0];
		}
		public override string getDescription()
		{
		return $"{name}(x) = {constants[2]}x^2 {constants[1].ToString("+ 0;- 0")}x {constants[0].ToString("+ 0;- 0")}, parabolic";
		}
		public override void calculateDomain()
		{
			domain.Add(new Interval(double.NegativeInfinity, double.PositiveInfinity));
		}
        public MathFunction derivate()
        {
            return new LinearFunction($"{this.name}`", [constants[2]*2, constants[1]]);
        }
		public override void calculateRange()
		{
			if (constants[2] < 0)
            {
                range.Add(new Interval(double.NegativeInfinity, this.getValue(-constants[1] / 2 / constants[2]), false, true));
            }
            else
            {
				range.Add(new Interval(this.getValue(-constants[1] / 2 / constants[2]), double.PositiveInfinity, true, false));
			}
		}
		public QuadraticFunction(string name, double[] constants)
		{
            constantsAmount = 3;
			this.checkConstantAmount(constants, constantsAmount);
			this.name = name;
			this.constants = constants.Reverse().ToList();
			if (constants[0] == 0)
			{
				throw new ArgumentException($"First argument cannot be 0. Create a linear function instead.");
			}
			this.calculateDomain();
            this.calculateRange();
		}
	}
    class LinearAbsoluteValueFunction : MathFunction
    {
		public override double getValue(double x)
		{
            return constants[4]*Math.Abs(constants[3] * x + constants[2]) + constants[1] * x + constants[0];
		}
		public override string getDescription()
		{
            return $"{name}(x) = {constants[4]}|{constants[3]}x {constants[2].ToString("+ 0;- 0")}| {constants[1].ToString("+ 0;- 0")}x {constants[0].ToString("+ 0;- 0")}";
		}
        public override void calculateDomain()
        {
			domain.Add(new Interval(double.NegativeInfinity, double.PositiveInfinity));
        }
		public override void calculateRange()
		{
            if (Math.Abs(constants[4] * constants[3]) < Math.Abs(constants[1]))
            {
                range.Add(new Interval(double.NegativeInfinity, double.PositiveInfinity));
                return;
            }
            if (constants[4] > 0)
            {
                 range.Add(new Interval(this.getValue(-constants[2] / constants[3]), double.PositiveInfinity, true));
            }
            else
            {
                range.Add(new Interval(double.NegativeInfinity, this.getValue(-constants[2] / constants[3]), false, true));
            }
		}
        public LinearAbsoluteValueFunction(string name, double[] constants)
        {
            constantsAmount = 5;
			this.checkConstantAmount(constants, constantsAmount);
			this.name = name;
			this.constants = constants.Reverse().ToList();
			if (constants[4] == 0)
            {
                throw new ArgumentException($"First argument cannot be 0. Create a linear function instead.");
			}
			if (constants[3] == 0)
			{
				throw new ArgumentException($"Second argument cannot be 0. Create a linear function instead.");
			}
			this.calculateDomain();
			this.calculateRange();
		}
    }
    //derivace by měla x^2 ve jmenovateli a to my nepodporujem
    class RationalFunction : MathFunction
    {
		public override double getValue(double x)
		{
			return constants[1] * x + constants[0] / (constants[3] * x + constants[2]);
		}
		public override void calculateDomain()
		{
            domain.Add(new Interval(double.NegativeInfinity, -constants[2]/constants[3]));
            domain.Add(new Interval(-constants[2] / constants[3], double.PositiveInfinity));
		}
		public override void calculateRange()
		{
            if (constants[3] / constants[2] == constants[1] / constants[0])
            {
                range.Add(new Interval(constants[3] / constants[1], constants[3] / constants[1], true, true));
                return;
            }
            range.Add(new Interval(double.NegativeInfinity, this.getValue(-constants[2] / constants[3]), false, true));
			range.Add(new Interval(this.getValue(-constants[2] / constants[3]), double.PositiveInfinity, true, false));
		}
		public override string getDescription()
		{
            string output = $"{this.name}(x) = {constants[1]}x {constants[0].ToString("+ 0;- 0")} / {constants[3]}x {constants[2].ToString("+ 0;- 0")}, ";
            if(constants[3] / constants[2] == constants[1] / constants[0])
            {
                output += "constant";
            }
            else
            {
                output += "hyperbolic";
            }
                return output;
		}
		public RationalFunction(string name, double[] constants)
        {
            constantsAmount = 4;
			this.checkConstantAmount(constants, constantsAmount);
			this.name = name;
			this.constants = constants.Reverse().ToList();
			if (constants[3] == 0)
            {
                throw new ArgumentException("First argument cannot be 0. Use a linear function instead");
            }
            this.calculateDomain();
            this.calculateRange();
		}
    }
    struct Interval
    {
        public double lowerLimit;
        public double upperLimit;

        public bool lowerClosed;
        public bool upperClosed;

        public override string ToString()
        {
            string output = "";
            if (lowerClosed)
            {
                output += '[';
            }
            else
            {
                output += '(';
            }
            output += $"{lowerLimit.ToString()}, {upperLimit.ToString()}";
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
		private double absZero(double x)
		{
			if (x == -0)
			{
				return 0;
			}
			return x;
		}
		public Interval(double lowerLimit, double upperLimit, bool lowerClosed=false, bool upperClosed=false)
        {
            this.lowerClosed = lowerClosed;
            this.upperClosed = upperClosed;
            this.lowerLimit = absZero(lowerLimit);
            this.upperLimit = absZero(upperLimit);
            if(double.IsInfinity(lowerLimit))
            {
                this.lowerClosed = false;
            }
            if(double.IsInfinity(upperLimit))
            {
                this.upperClosed = false;
            }
        }
    }
    interface IDerivable
    {
        MathFunction derivate();
    }
    interface IInvertable
    {
        MathFunction invert();
    }
}
