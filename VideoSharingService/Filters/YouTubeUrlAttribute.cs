using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace VideoSharingService.Models
{
    public class YouTubeUrlAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if(value != null)
            {
                string url = value.ToString();
                string pattern = "(?:.+?)?(?:\\/v\\/|watch\\/|\\?v=|\\&v=|youtu\\.be\\/|\\/v=|^youtu\\.be\\/)([a-zA-Z0-9_-]{11})+";

                var regex = Regex.Match(url, pattern);

                if (regex.Success)
                {
                    return true;
                }
                this.ErrorMessage = Resources.GlobalRes.InvalidUrl;
            }
            return false;
        }
    }
}