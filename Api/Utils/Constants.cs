namespace Api.Utils;

public static class Constants
{
    public static class Validation
    {
        public const int UsernameMinLength = 3;
        public const int UsernameMaxLength = 20;

        public const int PasswordMinLength = 8;
        public const int PasswordMaxLength = 64;
    }

    public static class RefreshToken
    {
        public const int BytesLength = 32;
    }

    public static class Files
    {
        public const long MaxSize = 10L * 1024 * 1024 * 1024;
        public const int BufferSize = 8192;

        public static readonly HashSet<string> ForbiddenExtensions =
        [
            ".exe",
            ".dll",
            ".com",
            ".scr",
            ".sys",
            ".drv",
            ".cpl",
            ".ocx"
        ];
    }
}