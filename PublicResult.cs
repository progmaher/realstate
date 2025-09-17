using System.Diagnostics.CodeAnalysis;

namespace Home
{
    public class PublicResult
    {
        [AllowNull]
        public string? token { get; set; }
        public bool status { get; set; }
        public string? message { get; set; }
    }
}
