﻿/* Copyright © 2019 Lee Kelleher.
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Collections.Generic;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Extensions;

namespace Umbraco.Community.Contentment.DataEditors
{
    public sealed class NotesDataEditor : IDataEditor
    {
        internal const string DataEditorAlias = Constants.Internals.DataEditorAliasPrefix + "Notes";
        internal const string DataEditorName = Constants.Internals.DataEditorNamePrefix + "Notes";
        internal const string DataEditorViewPath = Constants.Internals.EditorsPathRoot + "notes.html";
        internal const string DataEditorIcon = "icon-fa fa-sticky-note-o";

        private readonly ILocalizedTextService _localizedTextService;
        private readonly IShortStringHelper _shortStringHelper;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IIOHelper _ioHelper;

        public NotesDataEditor(
            ILocalizedTextService localizedTextService,
            IShortStringHelper shortStringHelper,
            IJsonSerializer jsonSerializer,
            IIOHelper ioHelper)
        {
            _localizedTextService = localizedTextService;
            _shortStringHelper = shortStringHelper;
            _jsonSerializer = jsonSerializer;
            _ioHelper = ioHelper;
        }

        public string Alias => DataEditorAlias;

        public EditorType Type => EditorType.PropertyValue;

        public string Name => DataEditorName;

        public string Icon => DataEditorIcon;

        public string Group => Constants.Conventions.PropertyGroups.Display;

        public bool IsDeprecated => false;

        public IDictionary<string, object> DefaultConfiguration => default;

        public IPropertyIndexValueFactory PropertyIndexValueFactory => new DefaultPropertyIndexValueFactory();

        public IConfigurationEditor GetConfigurationEditor() => new NotesConfigurationEditor(_ioHelper);

        public IDataValueEditor GetValueEditor()
        {
            return new ReadOnlyDataValueEditor(
                _localizedTextService,
                _shortStringHelper,
                _jsonSerializer)
            {
                ValueType = ValueTypes.Integer,
                View = DataEditorViewPath,
            };
        }

        public IDataValueEditor GetValueEditor(object configuration)
        {
            var hideLabel = false;

            if (configuration is Dictionary<string, object> config && config.ContainsKey(HideLabelConfigurationField.HideLabelAlias) == true)
            {
                hideLabel = config[HideLabelConfigurationField.HideLabelAlias].TryConvertTo<bool>().Result;
            }

            return new ReadOnlyDataValueEditor(
                _localizedTextService,
                _shortStringHelper,
                _jsonSerializer)
            {
                Configuration = configuration,
                HideLabel = hideLabel,
                ValueType = ValueTypes.Integer,
                View = DataEditorViewPath,
            };
        }
    }
}
