﻿namespace StyleCop.Analyzers
{
    using System.Collections.Immutable;
    using System.IO;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Newtonsoft.Json;
    using StyleCop.Analyzers.Settings.ObjectModel;

    /// <summary>
    /// Class that manages the settings files for StyleCopAnalyzers.
    /// </summary>
    internal static class SettingsHelper
    {
        private const string SettingsFileName = "stylecop.json";

        /// <summary>
        /// Gets the StyleCop settings.
        /// </summary>
        /// <param name="context">The context that will be used to determine the StyleCop settings.</param>
        /// <returns>A <see cref="StyleCopSettings"/> instance that represents the StyleCop settings for the given context.</returns>
        internal static StyleCopSettings GetStyleCopSettings(this SyntaxTreeAnalysisContext context)
        {
            return GetStyleCopSettings(context.Options != null ? context.Options.AdditionalFiles : ImmutableArray.Create<AdditionalText>());
        }

        private static StyleCopSettings GetStyleCopSettings(ImmutableArray<AdditionalText> additionalFiles)
        {
            try
            {
                foreach (var additionalFile in additionalFiles)
                {
                    if (Path.GetFileName(additionalFile.Path).ToLowerInvariant() == SettingsFileName)
                    {
                        var root = JsonConvert.DeserializeObject<SettingsFile>(additionalFile.GetText().ToString());
                        return root.Settings;
                    }
                }
            }
            catch (JsonException)
            {
                // The settings file is invalid -> return the default settings.
            }

            return new StyleCopSettings();
        }
    }
}
