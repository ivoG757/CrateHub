namespace Api.Utils;

public static class Constants
{
    public static class RegisterDtoConstants
    {
        public const int MaxLengthForPassword = 30;
        public const int MinLengthForPassword = 6;
        public const int MaxLengthForUsername = 20;
        public const int MinLengthForUsername = 5;
    }
    public static class UserConstants
    {
        public const int MaxLengthForPassword = 30;
        public const int MinLengthForPassword = 6;
        public const int MaxLengthForUsername = 20;
        public const int MinLengthForUsername = 5;
    }
    public static class RefreshTokenConstants
    {
        public const int RefreshTokenBytesLength = 32;
    }
    public static class FileConstants
    {
        public const long FileLengthLimit = 10L * 1024 * 1024 * 1024; // 10 GB
        public const int buffer = 8192;
        public static readonly ICollection<string> ForbiddenExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".exe",
            ".dll",
            ".com",
            ".scr",
            ".sys",
            ".drv",
            ".cpl",
            ".ocx"
        };
    }
}