namespace DotNetGroup.Services.Helper
{
    using System;

    public static class SystemDateTime
    {
        static SystemDateTime()
        {
            UtcNow = () => DateTime.UtcNow;
        }

        public static Func<DateTime> UtcNow { get; set; } 
    }
}
