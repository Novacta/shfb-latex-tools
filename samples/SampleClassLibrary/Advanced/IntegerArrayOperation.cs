using System;

namespace SampleClassLibrary.Advanced
{
    /// <summary>
    /// Provides a method to operate on arrays of integers.
    /// </summary>
    public static class IntegerArrayOperation
    {
        /// <summary>
        /// Applies the specified function to the given array of operands.
        /// </summary>
        /// <param name="func">The function to evaluate at each operand.</param>
        /// <param name="operands">The array of operands.</param>
        /// <returns>The results of the operations.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="func"/> is <b>null</b>.<br/>
        /// -or-<br/>
        /// <paramref name="operands"/> is <b>null</b>.
        /// </exception>
        /// <example>
        /// <para>
        /// In the following example, the applied function, say 
        /// <latex mode='inline'>f:\mathbb{N}\rightarrow \mathbb{N},</latex> is defined as
        /// <latex mode='display'>
        /// \forall n \in \mathbb{N}: n \mapsto f\left(n\right)=n^2.
        /// </latex>
        /// Integers in a given array are thus squared 
        /// executing the <see cref="Operate(Func{int, int}, int[])"/> method.
        /// In addition, input validation is also checked.
        /// </para>
        /// <para>
        /// <code language="cs">
        /// using System;
        /// using SampleClassLibrary.Advanced;
        /// 
        /// namespace SampleClassLibrary.CodeExamples.Advanced
        /// {
        ///     public class IntegerArrayOperationExample
        ///     {
        ///         public void Main()
        ///         {
        ///             // Define an operator that squares its operand
        ///             Func<![CDATA[<]]>int, int> square = (int operand) => operand * operand;
        /// 
        ///             // Define an array of operands
        ///             int[] operands = new int[3] { 2, 4, 8 };
        /// 
        ///             // Operate on it
        ///             int[] results = IntegerArrayOperation.Operate(square, operands);
        ///
        ///             // Show results
        ///             for (int i = 0; i <![CDATA[<]]> results.Length; i++)
        ///             {
        ///                 Console.WriteLine(
        ///                     "The result of squaring {0} is {1}.",
        ///                     operands[i],
        ///                     results[i]);
        ///             }
        ///
        ///             // Check that an operator cannot be null
        ///             try
        ///             {
        ///                 IntegerArrayOperation.Operate(null, new int[1]);
        ///             }
        ///             catch (Exception e)
        ///             {
        ///                 Console.WriteLine();
        ///                 Console.WriteLine("Cannot apply a null function:");
        ///                 Console.WriteLine(e.Message);
        ///             }
        /// 
        ///             // Check that an array of operands cannot be null
        ///             try
        ///             {
        ///                 IntegerArrayOperation.Operate(square, null);
        ///             }
        ///             catch (Exception e)
        ///             {
        ///                 Console.WriteLine();
        ///                 Console.WriteLine("Cannot apply a function to a null array:");
        ///                 Console.WriteLine(e.Message);
        ///             }
        ///         }
        ///     }
        /// }
        ///
        /// // Executing method Main() produces the following output:
        /// // 
        /// // The result of squaring 2 is 4.
        /// // The result of squaring 4 is 16.
        /// // The result of squaring 8 is 64.
        /// // 
        /// // Cannot apply a null function:
        /// // Value cannot be null.
        /// // Parameter name: func
        /// // 
        /// // Cannot apply a function to a null array:
        /// // Value cannot be null.
        /// // Parameter name: operands
        /// </code>
        /// </para>
        /// </example>
        public static int[] Operate(Func<int, int> func, int[] operands)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            if (operands == null)
            {
                throw new ArgumentNullException(nameof(operands));
            }

            int[] result = new int[operands.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = IntegerOperation.Operate(func, operands[i]);
            }
            return result;
        }
    }
}
