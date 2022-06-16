﻿/* Copyright © 2022 Lee Kelleher.
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/. */

using System.Collections.Generic;
#if NET472
using Umbraco.Core;
using Umbraco.Core.Deploy;
using Umbraco.Core.Models;
using Umbraco.Core.PropertyEditors;
using UmbConstants = Umbraco.Core.Constants;
#else
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Deploy;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Extensions;
using UmbConstants = Umbraco.Cms.Core.Constants;
#endif

namespace Umbraco.Community.Contentment.DataEditors
{
    internal sealed class NotesConfigurationConnector : IDataTypeConfigurationConnector
    {
#if NET472 == false
        private readonly IConfigurationEditorJsonSerializer _configurationEditorJsonSerializer;
#endif
        private readonly ILocalLinkParser _localLinkParser;
        private readonly IImageSourceParser _imageSourceParser;
        private readonly IMacroParser _macroParser;

        public IEnumerable<string> PropertyEditorAliases => new[] { NotesDataEditor.DataEditorName };

        public NotesConfigurationConnector(
#if NET472 == false
            IConfigurationEditorJsonSerializer configurationEditorJsonSerializer,
#endif
            ILocalLinkParser localLinkParser,
            IImageSourceParser imageSourceParser,
            IMacroParser macroParser)
        {
#if NET472 == false
            _configurationEditorJsonSerializer = configurationEditorJsonSerializer;
#endif
            _localLinkParser = localLinkParser;
            _imageSourceParser = imageSourceParser;
            _macroParser = macroParser;
        }

        public object FromArtifact(IDataType dataType, string configuration)
        {
            var dataTypeConfigurationEditor = dataType.Editor.GetConfigurationEditor();
#if NET472
            var db = dataTypeConfigurationEditor.FromDatabase(configuration);
#else
            var db = dataTypeConfigurationEditor.FromDatabase(configuration, _configurationEditorJsonSerializer);
#endif

            if (db is Dictionary<string, object> config &&
                config.TryGetValueAs(NotesConfigurationField.Notes, out string notes) == true &&
                string.IsNullOrWhiteSpace(notes) == false)
            {
                notes = _localLinkParser.FromArtifact(notes);
                notes = _imageSourceParser.FromArtifact(notes);
                notes = _macroParser.FromArtifact(notes);

                config[NotesConfigurationField.Notes] = notes;

                return config;
            }

            return db;
        }

        public string ToArtifact(IDataType dataType, ICollection<ArtifactDependency> dependencies)
        {
            if (dataType.Configuration is Dictionary<string, object> config &&
                config.TryGetValueAs(NotesConfigurationField.Notes, out string notes) == true &&
                string.IsNullOrWhiteSpace(notes) == false)
            {
                var udis = new List<Udi>();

                notes = _localLinkParser.ToArtifact(notes, udis);
                notes = _imageSourceParser.ToArtifact(notes, udis);
                notes = _macroParser.ToArtifact(notes, udis);

                foreach (var udi in udis)
                {
                    var mode = udi.EntityType == UmbConstants.UdiEntityType.Macro
                        ? ArtifactDependencyMode.Match
                        : ArtifactDependencyMode.Exist;

                    dependencies.Add(new ArtifactDependency(udi, false, mode));
                }

                config[NotesConfigurationField.Notes] = notes;
            }

#if NET472
            return ConfigurationEditor.ToDatabase(dataType.Configuration);
#else
            return ConfigurationEditor.ToDatabase(dataType.Configuration, _configurationEditorJsonSerializer);
#endif
        }
    }
}
