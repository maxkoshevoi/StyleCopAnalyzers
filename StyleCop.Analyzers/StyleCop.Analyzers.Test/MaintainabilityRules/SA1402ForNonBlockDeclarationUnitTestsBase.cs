﻿// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.Test.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.MaintainabilityRules;
    using StyleCop.Analyzers.Test.Verifiers;

    public abstract class SA1402ForNonBlockDeclarationUnitTestsBase
    {
        public abstract string Keyword { get; }

        protected SA1402SettingsConfiguration SettingsConfiguration { get; set; } = SA1402SettingsConfiguration.ConfigureAsTopLevelType;

        protected static DiagnosticResult Diagnostic()
            => StyleCopDiagnosticVerifier<SA1402FileMayOnlyContainASingleType>.Diagnostic();

        protected static Task VerifyCSharpDiagnosticAsync(string source, string testSettings, DiagnosticResult expected, CancellationToken cancellationToken)
            => VerifyCSharpDiagnosticAsync(source, testSettings, new[] { expected }, cancellationToken);

        protected static Task VerifyCSharpDiagnosticAsync(string source, string testSettings, DiagnosticResult[] expected, CancellationToken cancellationToken)
        {
            var test = new StyleCopCodeFixVerifier<SA1402FileMayOnlyContainASingleType, SA1402CodeFixProvider>.CSharpTest
            {
                TestCode = source,
                Settings = testSettings,
            };

            test.ExpectedDiagnostics.AddRange(expected);
            return test.RunAsync(cancellationToken);
        }

        protected static Task VerifyCSharpFixAsync(string source, string testSettings, DiagnosticResult expected, (string fileName, string content)[] fixedSources, CancellationToken cancellationToken)
            => VerifyCSharpFixAsync(source, testSettings, new[] { expected }, fixedSources, cancellationToken);

        protected static Task VerifyCSharpFixAsync(string source, string testSettings, DiagnosticResult[] expected, (string fileName, string content)[] fixedSources, CancellationToken cancellationToken)
        {
            var test = new StyleCopCodeFixVerifier<SA1402FileMayOnlyContainASingleType, SA1402CodeFixProvider>.CSharpTest
            {
                TestCode = source,
                Settings = testSettings,
            };

            foreach (var fixedSource in fixedSources)
            {
                test.FixedSources.Add(fixedSource);
            }

            test.ExpectedDiagnostics.AddRange(expected);
            return test.RunAsync(cancellationToken);
        }

        protected virtual string GetSettings()
        {
            return this.SettingsConfiguration.GetSettings(this.Keyword);
        }
    }
}
