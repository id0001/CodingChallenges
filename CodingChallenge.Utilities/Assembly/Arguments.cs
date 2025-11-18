using CodingChallenge.Utilities.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace CodingChallenge.Utilities.Assembly
{
    public abstract record Arguments(object[] Values)
    {
        public int Length => Values.Length;

        public object this[int index] => Values[index];

        public T GetValue<T>(int index, Func<string, T> lookup)
            where T : IParsable<T>
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, Values.Length);

            if (Values[index] is T value)
                return value;

            return lookup((string)Values[index]);
        }

        public static Arguments Parse<T1>(params string[] args)
            where T1 : IParsable<T1>
        {
            ArgumentOutOfRangeException.ThrowIfNotEqual(args.Length, 1);

            var genericTypes = MethodBase.GetCurrentMethod()!.GetGenericArguments();
            var values = new object[args.Length];

            DetermineTypeAndValue<T1>(0, args, genericTypes, values);

            var openGenericType = typeof(Arguments<>);
            var closedGenericType = openGenericType.MakeGenericType(genericTypes);

            return (Arguments)Activator.CreateInstance(closedGenericType, values)!;
        }

        public static Arguments Parse<T1, T2>(params string[] args)
            where T1 : IParsable<T1>
            where T2 : IParsable<T2>
        {
            ArgumentOutOfRangeException.ThrowIfNotEqual(args.Length, 2);

            var genericTypes = MethodBase.GetCurrentMethod()!.GetGenericArguments();
            var values = new object[args.Length];

            DetermineTypeAndValue<T1>(0, args, genericTypes, values);
            DetermineTypeAndValue<T2>(1, args, genericTypes, values);

            var openGenericType = typeof(Arguments<,>);
            var closedGenericType = openGenericType.MakeGenericType(genericTypes);

            return (Arguments)Activator.CreateInstance(closedGenericType, values)!;
        }

        public static Arguments Parse<T1, T2, T3>(params string[] args)
            where T1 : IParsable<T1>
            where T2 : IParsable<T2>
            where T3 : IParsable<T3>
        {
            ArgumentOutOfRangeException.ThrowIfNotEqual(args.Length, 3);

            var genericTypes = MethodBase.GetCurrentMethod()!.GetGenericArguments();
            var values = new object[args.Length];

            DetermineTypeAndValue<T1>(0, args, genericTypes, values);
            DetermineTypeAndValue<T2>(1, args, genericTypes, values);
            DetermineTypeAndValue<T3>(2, args, genericTypes, values);

            var openGenericType = typeof(Arguments<,,>);
            var closedGenericType = openGenericType.MakeGenericType(genericTypes);

            return (Arguments)Activator.CreateInstance(closedGenericType, values)!;
        }

        public static Arguments Parse<T1, T2, T3, T4>(params string[] args)
            where T1 : IParsable<T1>
            where T2 : IParsable<T2>
            where T3 : IParsable<T3>
            where T4 : IParsable<T4>
        {
            ArgumentOutOfRangeException.ThrowIfNotEqual(args.Length, 4);

            var genericTypes = MethodBase.GetCurrentMethod()!.GetGenericArguments();
            var values = new object[args.Length];

            DetermineTypeAndValue<T1>(0, args, genericTypes, values);
            DetermineTypeAndValue<T2>(1, args, genericTypes, values);
            DetermineTypeAndValue<T3>(2, args, genericTypes, values);
            DetermineTypeAndValue<T4>(3, args, genericTypes, values);

            var openGenericType = typeof(Arguments<,,,>);
            var closedGenericType = openGenericType.MakeGenericType(genericTypes);

            return (Arguments)Activator.CreateInstance(closedGenericType, values)!;
        }



        private static void DetermineTypeAndValue<T>(int i, string[] args, Type[] types, object[] values) where T : IParsable<T>
        {
            if (args[i].Is<T>(out var tval))
            {
                types[i] = typeof(T);
                values[i] = tval;
            }
            else
            {
                types[i] = typeof(string);
                values[i] = args[i];
            }
        }
    }

    public sealed record Arguments<T1> : Arguments
    {
        [SetsRequiredMembers]
        public Arguments(params object[] values) : base(values)
        {
            A = (T1)values[0];
        }

        public required T1 A { get; init; }
    }

    public sealed record Arguments<T1, T2> : Arguments
    {
        [SetsRequiredMembers]
        public Arguments(params object[] values) : base(values)
        {
            A = (T1)values[0];
            B = (T2)values[1];
        }

        public required T1 A { get; init; }
        public required T2 B { get; init; }
    }

    public sealed record Arguments<T1, T2, T3> : Arguments
    {
        [SetsRequiredMembers]
        public Arguments(params object[] values) : base(values)
        {
            A = (T1)values[0];
            B = (T2)values[1];
            C = (T3)values[2];
        }

        public required T1 A { get; init; }
        public required T2 B { get; init; }
        public required T3 C { get; init; }
    }

    public sealed record Arguments<T1, T2, T3, T4> : Arguments
    {
        [SetsRequiredMembers]
        public Arguments(params object[] values) : base(values)
        {
            A = (T1)values[0];
            B = (T2)values[1];
            C = (T3)values[2];
            D = (T4)values[3];
        }

        public required T1 A { get; init; }
        public required T2 B { get; init; }
        public required T3 C { get; init; }
        public required T4 D { get; init; }
    }
}
