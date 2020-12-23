namespace FarmaShop.Web.Util
{
    public class Util
    {
        public static int GetCategoryIdFromUrl(string url)
        {
            var path = url.Split("/");
            return int.Parse(path[path.Length - 1]);
        }
    }
}