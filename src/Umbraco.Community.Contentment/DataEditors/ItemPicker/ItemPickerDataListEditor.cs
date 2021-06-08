﻿/* Copyright © 2019 Lee Kelleher.
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Collections.Generic;
using Umbraco.Core;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Extensions;

namespace Umbraco.Community.Contentment.DataEditors
{
    public sealed class ItemPickerDataListEditor : IDataListEditor
    {
        internal const string DataEditorViewPath = Constants.Internals.EditorsPathRoot + "item-picker.html";
        internal const string DataEditorOverlayViewPath = Constants.Internals.EditorsPathRoot + "item-picker.overlay.html";

        public string Name => "Item Picker";

        public string Description => "Select items from an Umbraco style overlay.";

        public string Icon => "icon-fa fa-mouse-pointer";

        public string Group => default;

        public IEnumerable<ConfigurationField> Fields => new ConfigurationField[]
        {
            new ConfigurationField
            {
                Key = "overlaySize",
                Name = "Editor overlay size",
                Description = "Select the size of the overlay editing panel. By default this is set to 'small'. However if the editor fields require a wider panel, please select 'medium' or 'large'.",
                View = IOHelper.ResolveUrl(RadioButtonListDataListEditor.DataEditorViewPath),
                Config = new Dictionary<string, object>
                {
                    { Constants.Conventions.ConfigurationFieldAliases.Items, new[]
                        {
                            new DataListItem { Name = "Small", Value = "small" },
                            new DataListItem { Name = "Medium", Value = "medium" },
                            new DataListItem { Name = "Large", Value = "large" }
                        }
                    },
                    { Constants.Conventions.ConfigurationFieldAliases.DefaultValue, "small" }
                }
            },
            new ConfigurationField
            {
                Key = "defaultIcon",
                Name = "Default icon",
                Description = "Select an icon to be displayed as the default icon,<br><em>(for when no icon is available)</em>.",
                View = IOHelper.ResolveUrl("~/umbraco/views/propertyeditors/listview/icon.prevalues.html"),
            },
            new ConfigurationField
            {
                Key = "listType",
                Name = "List type",
                Description = "Select the style of list to be displayed in the overlay.",
                View = IOHelper.ResolveUrl(RadioButtonListDataListEditor.DataEditorViewPath),
                Config = new Dictionary<string, object>
                {
                    { Constants.Conventions.ConfigurationFieldAliases.Items, new[]
                        {
                            new DataListItem { Name = "Grid", Value = "grid", Description = "Displays as a card based layout, (3 cards per row)." },
                            new DataListItem { Name = "List", Value = "list", Description = "Displays as a single column menu, (with descriptions, if available)." }
                        }
                    },
                    { ShowDescriptionsConfigurationField.ShowDescriptions, Constants.Values.True },
                }
            },
            new EnableFilterConfigurationField
            {
                View = "views/propertyeditors/boolean/boolean.html",
                Config = new Dictionary<string, object>
                {
                    { "default", Constants.Values.True }
                },
            },
            new MaxItemsConfigurationField(),
            new AllowClearConfigurationField(),
            new ConfigurationField
            {
                Key = "allowDuplicates",
                Name = "Allow duplicates?",
                Description = "Select to allow the editor to select duplicate items.",
                View = "boolean",
            },
            new ConfigurationField
            {
                Key = "enableMultiple",
                Name = "Multiple selection?",
                Description = "Select to enable picking multiple items.",
                View = "boolean",
            },
            new DisableSortingConfigurationField(),
            new ConfigurationField
            {
                Key ="confirmRemoval",
                Name = "Confirm removals?",
                Description = "Select to enable a confirmation prompt when removing an item.",
                View = "boolean",
            }
        };

        public Dictionary<string, object> DefaultValues => new Dictionary<string, object>
        {
            { "listType", "list" },
            { "defaultIcon", Core.Constants.Icons.DefaultIcon },
            { EnableFilterConfigurationField.EnableFilter, Constants.Values.True },
            { MaxItemsConfigurationField.MaxItems, "0" },
        };

        public Dictionary<string, object> DefaultConfig => new Dictionary<string, object>
        {
            { Constants.Conventions.ConfigurationFieldAliases.OverlayView, IOHelper.ResolveUrl(DataEditorOverlayViewPath) },
            { "overlayOrderBy", string.Empty },
        };

        public bool HasMultipleValues(Dictionary<string, object> config)
        {
            return config.TryGetValueAs(MaxItemsConfigurationField.MaxItems, out int maxItems) == true && maxItems != 1;
        }

        public OverlaySize OverlaySize => OverlaySize.Small;

        public string View => DataEditorViewPath;
    }
}
