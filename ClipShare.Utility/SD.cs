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

        public static string IsActivePage(this IHtmlHelper html, string page)
        {
            var currentPage = html.ViewContext.HttpContext.Request.Query["page"].ToString();
            var isPageMatch = currentPage == page;

            // Default Home Page
            if (string.IsNullOrEmpty(currentPage) && page == "Home") return "active";

            return isPageMatch ? "active" : string.Empty;
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

        public static string GetContentType(string fileExtension)
        {
            return fileExtension switch
            {
                "video/mp4" => ".mp4",
                "video/quicktime" => ".mov",
                "video/x-msvideo" => ".avi",
                "video/x-ms-wmv" => ".wmv",
                "video/x-flv" => ".flv",
                "video/x-matroska" => ".mkv",
                "video/webm" => ".webm",
                "video/ogg" => ".ogv",
                "video/3gpp" => ".3gp",
                "video/3gpp2" => ".3g2",
                _ => ".mp4"
            };
        }


        public static string GetFileExtension(string contentType)
        {
            return contentType switch
            {
                "video/mp4" => ".mp4",
                "video/quicktime" => ".mov",
                "video/x-msvideo" => ".avi",
                "video/x-ms-wmv" => ".wmv",
                "video/x-flv" => ".flv",
                "video/x-matroska" => ".mkv",
                "video/webm" => ".webm",
                "video/ogg" => ".ogv",
                "video/3gpp" => ".3gp",
                "video/3gpp2" => ".3g2",
                _ => ".mp4"
            };
        }
    }
}
