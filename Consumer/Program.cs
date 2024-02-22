namespace AfasDemo
{
    internal partial class Program
    {
        public static partial string GetFileName();

        static void Main(string[] args)
        {
            Console.WriteLine(SQL.MySelect);
            Console.WriteLine(GetFileName());

        }
    }
}