﻿using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FarmaShop.Web.Extensions
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Html;
    public static class HtmlHelperExtensions
    {
        public const string HttpMethodOverrideFormName = "X-HTTP-Method-Override";
    
        public static IHtmlContent HttpMethodOverride(this IHtmlHelper helper, 
            System.Net.Http.HttpMethod method, 
            string name = HttpMethodOverrideFormName)
        {
            return helper.Raw($"<input type=\"hidden\" name=\"{name}\" value=\"{method}\" />");
        }
    }
}