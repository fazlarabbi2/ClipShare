using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClipShare.Utility
{
    public static class SD
    {
        public const string AdminRole = "Admin";
        public const string UserRole = "User";
        public const string ModeratorRole = "Moderator";
        public static readonly List<string> Roles = new List<string> { AdminRole, UserRole, ModeratorRole };
        public const int MB = 1000000;

        public static string IsActive(this IHtmlHelper html, string controller, string action, string cssClass = "active")
        {
            var routeData = html.ViewContext.RouteData;
            var routeAction = routeData.Values["action"]?.ToString();
            var routeController = routeData.Values["controller"]?.ToString();

            var returnActive = controller == routeController && action == routeAction;

            return returnActive ? cssClass : string.Empty;
        }

        //public static string GetRandomName()
        //{
        //    var randomNumber = new byte[10];

        //    using var rng = RandomNumberGenerator.Create();
        //    rng.GetBytes(randomNumber);

        //    return Convert.ToBase64String(randomNumber);
        //}

        public static int GetRandomNumber(int minValue, int maxValue, int seed)
        {
            Random random = new(seed);

            return random.Next(minValue, maxValue);
        }

        public static DateTime GetRandomDate(DateTime minDate, DateTime maxDate, int seed)
        {
            Random random = new(seed);

            int range = (maxDate - minDate).Days;
            return minDate.AddDays(random.Next(range + 1));
        }
    }
}
