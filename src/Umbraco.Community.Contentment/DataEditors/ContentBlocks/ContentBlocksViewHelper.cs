﻿/* Copyright © 2020 Lee Kelleher.
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/. */

// TODO: [LK:2021-05-03] v9 Commenting out, as I'm (currently) unsure how to do the `RazorViewEngine` bits.
// Feel that I need more understanding of .NET Core RazorPages.

//using System.IO;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.Routing;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Razor;

//namespace Umbraco.Community.Contentment.DataEditors
//{
//    internal static class ContentBlocksViewHelper
//    {
//        private class FakeController : Controller { }

//        private static readonly RazorViewEngine _viewEngine = new RazorViewEngine
//        {
//            PartialViewLocationFormats = new[]
//            {
//                "~/Views/Partials/Blocks/{0}.cshtml",
//                "~/Views/Partials/Blocks/Default.cshtml",
//                Constants.Internals.PackagePathRoot + "render/ContentBlockPreview.cshtml"
//            }
//        };

//        internal static string RenderPartial(string partialName, ViewDataDictionary viewData)
//        {
//            using (var sw = new StringWriter())
//            {
//                var httpContext = new HttpContextWrapper(HttpContext.Current);

//                var routeData = new RouteData { Values = { { "controller", nameof(FakeController) } } };

//                var controllerContext = new ControllerContext(new RequestContext(httpContext, routeData), new FakeController());

//                var viewResult = _viewEngine.FindPartialView(controllerContext, partialName, false);

//                if (viewResult.View == null)
//                {
//                    return null;
//                }

//                viewResult.View.Render(new ViewContext(controllerContext, viewResult.View, viewData, new TempDataDictionary(), sw), sw);

//                return sw.ToString();
//            }
//        }
//    }
//}
