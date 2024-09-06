using NuGet.Configuration;

namespace MengGrocery.Helpers
{
    public interface ICookieHelper
    {
        void DropCookie(string key, string value, int? expireTime);
        string GetCookie(string key);
    }
    public class CookieHelper : ICookieHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CookieHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void DropCookie(string key, string value, int? expireTime)
        {
            if(_httpContextAccessor.HttpContext != null)
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, new CookieOptions
                {
                    Expires = expireTime.HasValue ? DateTime.Now.AddMinutes(expireTime.Value) : DateTime.Now.AddMonths(1)
                });
            }
        }

        public string GetCookie(string key)
        {
            if(_httpContextAccessor.HttpContext != null)
            {
                if(_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(key, out string value))
                {
                    return value;
                }
            }

            return null;
        }
    }
}