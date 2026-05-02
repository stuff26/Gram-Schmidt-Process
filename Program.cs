public class GramSchmidtProcess
{
    public static void Main()
    {
        List<List<double>> testBasis = [
            [1, 1, 0, 0],
            [0, 1, 0, 1],
            [0, 0, 1, 1]
        ];
        int vectorSize = testBasis[0].Count;

        List<List<double>> orthonormBasis = [];

        // Make first vector of orthonorm basis the normalized first vector
        orthonormBasis.Add(NormalizeVector(testBasis[0]));

        // Compute other vectors
        for (int num = 1; num < testBasis.Count; num++)
        {
            List<double> projection = MakeZeroVector(vectorSize);
            for (int projNum = 1; projNum <= num; projNum++)
            {
                double prodFactor = InnerProduct(testBasis[projNum], orthonormBasis[projNum-1]);
                var projToAdd = ScalerMult(prodFactor, orthonormBasis[projNum-1]);
                projection = VectorAdd(projection, projToAdd);
            }

            List<double> orthoComplement = VectorAdd(testBasis[num], projection, subtract:true);
            var orthoVector = NormalizeVector(orthoComplement);
            orthonormBasis.Add(orthoVector);
        }

        Console.WriteLine(PrintSet(orthonormBasis));
    }

    /// <summary>
    /// Computes the inner product of 2 vectors, done by summing the products of each corresponding entry
    /// </summary>
    /// <param name="vector1">First vector</param>
    /// <param name="vector2">Second vector</param>
    /// <returns>Resulting inner product</returns>
    public static double InnerProduct(List<double> vector1, List<double> vector2)
    {
        double result = 0;
        for (int i = 0; i < vector1.Count; i++)
        {
            var num1 = vector1[i];
            var num2 = vector2[i];
            result += num1 * num2;
        }
        return result;
    }

    /// <summary>
    /// Computes the norm of a vector by taking the square root of an inner product of itself
    /// </summary>
    /// <param name="vector">Vector to take the norm of</param>
    /// <returns>The norm or length of a vector</returns>
    public static double Norm(List<double> vector)
    {
        return Math.Sqrt(InnerProduct(vector, vector));
    }

    /// <summary>
    /// Turns a given vector into a unit vector of the same direction
    /// </summary>
    /// <param name="vector">Initial vector to normalize</param>
    /// <returns>A unit vector in the direction of the initial given vector</returns>
    public static List<double> NormalizeVector(List<double> vector)
    {
        double normFactor = 1 / Norm(vector);
        return ScalerMult(normFactor, vector);
    }

    /// <summary>
    /// Performs a scaler multiplication on a vector
    /// </summary>
    /// <param name="scaler">Scaler to multiply by</param>
    /// <param name="vector">Initial vector to multiply with</param>
    /// <returns>A new vector with each entry multiplies by the given scaler</returns>
    public static List<double> ScalerMult(double scaler, List<double> vector)
    {
        List<double> newVector = [];
        foreach (var num in vector)
        {
            newVector.Add(num * scaler);
        }
        return newVector;
    }

    /// <summary>
    /// Performs vector addition on two given vectors
    /// </summary>
    /// <param name="vector1">First vector to add</param>
    /// <param name="vector2">First vector to add with, or will subtract from vector1 is subtract is true</param>
    /// <param name="subtract">Changes to vector subtraction, taking vector1 and subtracting vector2 from it</param>
    /// <returns>Resulting vector from adding two vectors</returns>
    public static List<double> VectorAdd(List<double> vector1, List<double> vector2, bool subtract = false)
    {
        List<double> newVector = [];
        for (int i = 0; i < vector1.Count; i++)
        {
            if (subtract)
                newVector.Add(vector1[i] - vector2[i]);
            else
                newVector.Add(vector1[i] + vector2[i]);
        }
        return newVector;
    }

    /// <summary>
    /// Makes a vector a certain size containging only 0s
    /// </summary>
    /// <param name="size">Size of vector to make</param>
    /// <returns>A zero vector</returns>
    public static List<double> MakeZeroVector(int size)
    {
        List<double> result = [];
        for (int i = 0; i < size; i++)
        {
            result.Add(0);
        }
        return result;
    }

    /// <summary>
    /// Prints out a set of vectors
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="vectorSet">Vectors to print out</param>
    /// <returns>String containing the entries of the vector set</returns>
    public static string PrintSet<T>(List<List<T>> vectorSet)
    {
        string result = "";
        foreach (List<T> vector in vectorSet)
        {
            result += "[";
            foreach (T num in vector)
            {
                result += $"{num}, ";
            }
            result = result[..^2] + "]\n";
        }
        return result;
    }
}