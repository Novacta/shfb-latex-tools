using System;

namespace SampleClassLibrary
{
    /// <summary>
    /// Provides a method to operate on integers.
    /// </summary>
    public static class IntegerOperation
    {
        /// <summary>
        /// Applies the specified function to the given operand.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <param name="operand">The operand.</param>
        /// <returns>The result of the operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="func"/> is <b>null</b>.</exception>
        /// <example>
        /// <para>
        /// In the following example, the applied function, say 
        /// <latex mode='inline'>f:\mathbb{N}\rightarrow \mathbb{N},</latex> is defined as
        /// <latex mode='display'>
        /// \forall n \in \mathbb{N}: n \mapsto f\left(n\right)=n^2.
        /// </latex>
        /// An integer is thus squared 
        /// executing the <see cref="Operate(Func{int, int}, int)"/> method.
        /// In addition, input validation is also checked.
        /// </para>
        /// <para>
        /// <code language="cs">
        /// using System;
        /// namespace SampleClassLibrary.CodeExamples
        /// {
        ///     public class IntegerOperationExample
        ///     {
        ///         public void Main()
        ///         {
        ///             // Define an operator that squares its operand
        ///             Func<![CDATA[<]]>int, int> square = (int operand) => operand * operand;
        /// 
        ///             // Define an operand
        ///             int integer = 2;
        /// 
        ///             // Operate on it
        ///             Console.WriteLine("Squaring {0}...", integer);
        ///             int result = IntegerOperation.Operate(square, integer);
        ///             Console.WriteLine("...the result is {0}.", result);
        /// 
        ///             // Check that an operator cannot be null
        ///             try
        ///             {
        ///                 IntegerOperation.Operate(null, 0);
        ///             }
        ///             catch (Exception e)
        ///             {
        ///                 Console.WriteLine();
        ///                 Console.WriteLine("Cannot apply a null function:");
        ///                 Console.WriteLine(e.Message);
        ///             }
        ///         }
        ///     }
        /// }
        ///  
        /// // Executing method Main() produces the following output:
        /// // 
        /// // Squaring 2...
        /// // ...the result is 4.
        /// // 
        /// // Cannot apply a null function:
        /// // Value cannot be null.
        /// // Parameter name: func
        /// </code>
        /// </para>
        /// </example>
        public static int Operate(Func<int, int> func, int operand)
        {
            if (func==null)
            {
                throw new ArgumentNullException(nameof(func));
            }
            return func(operand);
        }
    }
}
