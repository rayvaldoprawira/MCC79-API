namespace API.Utilities;

public class GenerateOTP
{
    public static int GenerateRandomOTP()
    {
        Random random = new Random();
        HashSet<int> uniqueDigits = new HashSet<int>();
        while (uniqueDigits.Count < 6)
        {
            int digit = random.Next(0, 9);
            uniqueDigits.Add(digit);
        }

        int generatedOtp = uniqueDigits.Aggregate(0, (acc, digit) => acc * 10 + digit);

        return generatedOtp;
    }
}