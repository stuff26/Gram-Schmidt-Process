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

    public static List<double> NormalizeVector(List<double> vector)
    {
        double normFactor = 1 / Norm(vector);
        return ScalerMult(normFactor, vector);
    }

    public static List<double> ScalerMult(double scaler, List<double> vector)
    {
        List<double> newVector = [];
        foreach (var num in vector)
        {
            newVector.Add(num * scaler);
        }
        return newVector;
    }

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

    public static List<double> MakeZeroVector(int size)
    {
        List<double> result = [];
        for (int i = 0; i < size; i++)
        {
            result.Add(0);
        }
        return result;
    }

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