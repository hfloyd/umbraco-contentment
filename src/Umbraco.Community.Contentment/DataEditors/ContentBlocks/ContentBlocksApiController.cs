﻿/* Copyright © 2019 Lee Kelleher.
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/. */

// TODO: [LK:2021-05-03] v9 Commenting out, as I'm (currently) unsure how to do the `RazorViewEngine` bits.
// Feel that I need more understanding of .NET Core RazorPages.

//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json.Linq;
//using Umbraco.Cms.Core.Models.PublishedContent;
//using Umbraco.Cms.Core.Services;
//using Umbraco.Cms.Web.BackOffice.Controllers;
//using Umbraco.Cms.Web.Common.Attributes;
//using Umbraco.Community.Contentment.Web.PublishedCache;

//namespace Umbraco.Community.Contentment.DataEditors
//{
//    [PluginController(Constants.Internals.PluginControllerName), IsBackOffice]
//    public sealed class ContentBlocksApiController : UmbracoAuthorizedJsonController
//    {
//        private readonly ILogger<ContentBlocksApiController> _logger;
//        private readonly IPublishedModelFactory _publishedModelFactory;
//        private readonly IContentTypeService _contentTypeService;

//        public ContentBlocksApiController(
//            ILogger<ContentBlocksApiController> logger,
//            IPublishedModelFactory publishedModelFactory,
//            IContentTypeService contentTypeService)
//        {
//            _logger = logger;
//            _publishedModelFactory = publishedModelFactory;
//            _contentTypeService = contentTypeService;
//        }

//        [HttpPost]
//        public HttpResponseMessage GetPreviewMarkup([FromBody] JObject item, int elementIndex, Guid elementKey, int contentId)
//        {
//            var preview = true;

//            var content = UmbracoContext.Content.GetById(true, contentId);
//            if (content == null)
//            {
//                _logger.LogDebug($"Unable to retrieve content for ID '{contentId}', it is most likely a new unsaved page.");
//            }

//            var element = default(IPublishedElement);
//            var block = item.ToObject<ContentBlock>();
//            if (block != null && block.ElementType.Equals(Guid.Empty) == false)
//            {
//                if (ContentTypeCacheHelper.TryGetAlias(block.ElementType, out var alias, _contentTypeService) == true)
//                {
//                    var contentType = UmbracoContext.PublishedSnapshot.Content.GetContentType(alias);
//                    if (contentType != null && contentType.IsElement == true)
//                    {
//                        var properties = new List<IPublishedProperty>();

//                        foreach (var thing in block.Value)
//                        {
//                            var propType = contentType.GetPropertyType(thing.Key);
//                            if (propType != null)
//                            {
//                                properties.Add(new DetachedPublishedProperty(propType, null, thing.Value, preview));
//                            }
//                        }

//                        element = _publishedModelFactory.CreateModel(new DetachedPublishedElement(block.Key, contentType, properties));
//                    }
//                }
//            }

//            var viewData = new System.Web.Mvc.ViewDataDictionary(element)
//            {
//                { nameof(content), content },
//                { nameof(element), element },
//                { nameof(elementIndex), elementIndex },
//            };

//            if (ContentTypeCacheHelper.TryGetIcon(content.ContentType.Alias, out var contentIcon, _contentTypeService) == true)
//            {
//                viewData.Add(nameof(contentIcon), contentIcon);
//            }

//            if (ContentTypeCacheHelper.TryGetIcon(element.ContentType.Alias, out var elementIcon, _contentTypeService) == true)
//            {
//                viewData.Add(nameof(elementIcon), elementIcon);
//            }

//            var markup = default(string);

//            try
//            {
//                markup = ContentBlocksViewHelper.RenderPartial(element.ContentType.Alias, viewData);
//            }
//            catch (InvalidCastException icex)
//            {
//                // NOTE: This type of exception happens on a new (unsaved) page, when the context becomes the parent page,
//                // and the preview view is strongly typed to the current page's model type.
//                markup = "<p class=\"text-center mt4\">Unable to render the preview until the page has been saved.</p>";

//                _logger.LogError(icex, "Error rendering preview view.");
//            }
//            catch (Exception ex)
//            {
//                markup = $"<pre class=\"error\"><code>{ex}</code></pre>";

//                _logger.LogError(ex, "Error rendering preview view.");
//            }

//            return Request.CreateResponse(HttpStatusCode.OK, new { elementKey, markup });
//        }
//    }
//}
