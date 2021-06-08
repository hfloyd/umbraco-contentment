﻿/* Copyright © 2019 Lee Kelleher.
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.Authorization;
using Umbraco.Community.Contentment.DataEditors;
using Umbraco.Extensions;

namespace Umbraco.Community.Contentment.Web.Controllers
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [PluginController(Constants.Internals.PluginControllerName), IsBackOffice]
    [Authorize(Policy = AuthorizationPolicies.BackOfficeAccess)]
    public sealed class EnumDataSourceApiController : UmbracoAuthorizedJsonController
    {
        internal const string GetAssembliesUrl = "backoffice/Contentment/EnumDataSourceApi/GetAssemblies";
        internal const string GetEnumsUrl = "backoffice/Contentment/EnumDataSourceApi/GetEnums?assembly={0}";

        private readonly IShortStringHelper _shortStringHelper;

        public EnumDataSourceApiController(IShortStringHelper shortStringHelper)
        {
            _shortStringHelper = shortStringHelper;
        }

        [HttpGet]
        public IEnumerable<DataListItem> GetAssemblies()
        {
            const string App_Code = "App_Code";

            var options = new SortedDictionary<string, DataListItem>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            if (assemblies?.Length > 0)
            {
                foreach (var assembly in assemblies)
                {
                    if (options.ContainsKey(assembly.FullName) == true || assembly.IsDynamic == true)
                        continue;

                    var hasEnums = false;
                    if (assembly.ExportedTypes != null)
                    {
                        foreach (var exportedType in assembly.ExportedTypes)
                        {
                            if (exportedType.IsEnum == true)
                            {
                                hasEnums = true;
                                break;
                            }
                        }
                    }

                    if (hasEnums == false)
                        continue;

                    if (assembly.FullName.StartsWith(App_Code) == true && options.ContainsKey(App_Code) == false)
                    {
                        options.Add(App_Code, new DataListItem { Name = App_Code, Value = App_Code });
                    }
                    else
                    {
                        var assemblyName = assembly.GetName();
                        options.Add(assemblyName.FullName, new DataListItem { Name = assemblyName.Name, Value = assemblyName.FullName });
                    }
                }
            }

            return options.Values;
        }

        [HttpGet]
        public IEnumerable<DataListItem> GetEnums(string assembly)
        {
            var options = new SortedDictionary<string, DataListItem>();

            var types = Assembly.Load(assembly).GetTypes();
            foreach (var type in types)
            {
                if (type.IsEnum == false)
                    continue;

                options.Add(type.FullName, new DataListItem { Name = type.Name.SplitPascalCasing(_shortStringHelper), Value = type.FullName });
            }

            return options.Values;
        }
    }
}
