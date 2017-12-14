using System;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace PremiumDiesel.Web.HtmlHelperExtensions
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Renders a Font-Awesome view link button
        /// </summary>
        /// <param name="ajaxHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MvcHtmlString FaViewLinkButton(
            this AjaxHelper ajaxHelper,
            string actionName, string controllerName,
            int id,
            string extraClasses = "")
        {
            return RawActionLink(
                ajaxHelper,
                "<span class='fa fa-eye'></span>",
                actionName, controllerName,
                new { id = id },
                new AjaxOptions { UpdateTargetId = "content" },
                new { @class = $"btn btn-xs btn-info {extraClasses}", primaryKey = id }
                );
        }

        /// <summary>
        /// Renders a Font-Awesome edit link button
        /// </summary>
        /// <param name="ajaxHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MvcHtmlString FaEditLinkButton(
            this AjaxHelper ajaxHelper,
            string actionName,
            string controllerName,
            int id,
            string extraClasses = "")
        {
            return RawActionLink(
                ajaxHelper,
                "<span class='fa fa-pencil'></span>",
                actionName, controllerName,
                new { id = id },
                new AjaxOptions { UpdateTargetId = "content" },
                new { @class = $"btn btn-xs btn-success {extraClasses}", primaryKey = id }
                );
        }

        /// <summary>
        /// Renders a Font-Awesome delete link button
        /// </summary>
        /// <param name="ajaxHelper"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MvcHtmlString FaDeleteLinkButton(this AjaxHelper ajaxHelper, int id)
        {
            var buttonLink = $"<button primaryKey='{id}' class='btn btn-xs btn-danger js-delete'>" +
             "<span class='fa fa-trash'></span>" +
             "</button>";
            return MvcHtmlString.Create(buttonLink);
        }

        private static MvcHtmlString RawActionLink(
            this AjaxHelper ajaxHelper,
            string linkText,
            string actionName,
            string controllerName,
            object routeValues,
            AjaxOptions ajaxOptions,
            object htmlAttributes
            )
        {
            var repID = Guid.NewGuid().ToString();
            var lnk = ajaxHelper.ActionLink(repID, actionName, controllerName, routeValues, ajaxOptions, htmlAttributes);
            return MvcHtmlString.Create(lnk.ToString().Replace(repID, linkText));
        }
    }
}