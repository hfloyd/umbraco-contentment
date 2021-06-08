﻿/* Copyright © 2019 Lee Kelleher.
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Infrastructure.WebAssets;
using Umbraco.Community.Contentment;
using Umbraco.Community.Contentment.Composing;
using Umbraco.Community.Contentment.DataEditors;
using Umbraco.Community.Contentment.Telemetry;

namespace Umbraco.Extensions
{
    public static partial class UmbracoBuilderExtensions
    {
        public static IUmbracoBuilder AddContentment(this IUmbracoBuilder builder, Action<ContentmentSettings> configure = default)
        {
            // TODO: [LK:2021-05-10] Is there a way to combine these? e.g. `configure` will fallback on values from appSettings?
            _ = configure is not null
                ? builder.Services.Configure(configure)
                : builder.Services.Configure<ContentmentSettings>(builder.Config.GetSection(Constants.Internals.ConfigurationSection));

            builder
                .WithCollectionBuilder<ContentmentListItemCollectionBuilder>()
                    .Add(() => builder.TypeLoader.GetTypes<IContentmentListItem>())
            ;

            builder.Services.AddUnique<ConfigurationEditorUtility>();

            builder.Components().Append<ContentmentComponent>();

            builder.AddNotificationHandler<ServerVariablesParsing, ContentmentServerVariablesParsing>();
            builder.AddNotificationHandler<DataTypeSavedNotification, ContentmentTelemetryHandler>();

            return builder;
        }

        public static IUmbracoBuilder WithContentmentListItems(this IUmbracoBuilder builder, Action<ContentmentListItemCollectionBuilder> configure)
        {
            var items = builder.WithCollectionBuilder<ContentmentListItemCollectionBuilder>();

            if (configure is not null)
            {
                configure(items);
            }

            return builder;
        }

        public static IUmbracoBuilder UnlockContentment(this IUmbracoBuilder builder)
        {
            builder
                .WithCollectionBuilder<ContentmentListItemCollectionBuilder>()
                    // Data List - Data Sources
                    .Add<CountriesDataListSource>()
                    .Add<CurrenciesDataListSource>()
                    .Add<ExamineDataListSource>()
                    .Add<TimeZoneDataListSource>()
                    .Add<uCssClassNameDataListSource>()
                    .Add<UmbracoContentPropertiesDataListSource>()
                    .Add<UmbracoContentXPathDataListSource>()
                    .Add<UmbracoDictionaryDataListSource>()
                    .Add<UmbracoEntityDataListSource>()
                    .Add<UmbracoImageCropDataListSource>()
                    .Add<UmbracoMembersDataListSource>()
                    .Add<UmbracoMemberGroupDataListSource>()
                    .Add<UserDefinedDataListSource>()
            ;

            return builder;
        }
    }
}
