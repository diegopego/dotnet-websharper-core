module Compiler.Tests

open NUnit.Framework
open FsUnit
open System.IO
open FSharp.Compiler.CodeAnalysis
open System.Runtime.Caching
open WebSharper.Compiler
open WebSharper.Compiler.FSharp.Compile

[<SetUp>]
let Setup () =
    ()

[<Test>]
let ``Compilation test`` () =
    let mkProjectCommandLineArgs (dllName, fileNames) = 
        [| 

            let references =
                [ 
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Antiforgery.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Authentication.Abstractions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Authentication.Cookies.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Authentication.Core.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Authentication.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Authentication.OAuth.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Authorization.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Authorization.Policy.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Components.Authorization.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Components.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Components.Forms.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Components.Server.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Components.Web.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Connections.Abstractions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.CookiePolicy.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Cors.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Cryptography.Internal.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Cryptography.KeyDerivation.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.DataProtection.Abstractions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.DataProtection.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.DataProtection.Extensions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Diagnostics.Abstractions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Diagnostics.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Diagnostics.HealthChecks.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.HostFiltering.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Hosting.Abstractions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Hosting.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Hosting.Server.Abstractions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Html.Abstractions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Http.Abstractions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Http.Connections.Common.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Http.Connections.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Http.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Http.Extensions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Http.Features.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.HttpOverrides.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.HttpsPolicy.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Identity.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Localization.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Localization.Routing.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Metadata.dll"
                    //@"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Mvc.Abstractions.dll"
                    //@"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Mvc.ApiExplorer.dll"
                    //@"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Mvc.Core.dll"
                    //@"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Mvc.Cors.dll"
                    //@"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Mvc.DataAnnotations.dll"
                    //@"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Mvc.dll"
                    //@"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Mvc.Formatters.Json.dll"
                    //@"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Mvc.Formatters.Xml.dll"
                    //@"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Mvc.Localization.dll"
                    //@"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Mvc.Razor.dll"
                    //@"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Mvc.RazorPages.dll"
                    //@"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Mvc.TagHelpers.dll"
                    //@"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Mvc.ViewFeatures.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Razor.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Razor.Runtime.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.ResponseCaching.Abstractions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.ResponseCaching.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.ResponseCompression.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Rewrite.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Routing.Abstractions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Routing.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Server.HttpSys.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Server.IIS.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Server.IISIntegration.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Server.Kestrel.Core.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Server.Kestrel.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.Session.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.SignalR.Common.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.SignalR.Core.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.SignalR.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.SignalR.Protocols.Json.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.StaticFiles.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.WebSockets.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.AspNetCore.WebUtilities.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\Microsoft.CSharp.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Caching.Abstractions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Caching.Memory.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Configuration.Abstractions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Configuration.Binder.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Configuration.CommandLine.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Configuration.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Configuration.EnvironmentVariables.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Configuration.FileExtensions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Configuration.Ini.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Configuration.Json.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Configuration.KeyPerFile.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Configuration.UserSecrets.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Configuration.Xml.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.DependencyInjection.Abstractions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.DependencyInjection.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Diagnostics.HealthChecks.Abstractions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Diagnostics.HealthChecks.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.FileProviders.Abstractions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.FileProviders.Composite.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.FileProviders.Embedded.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.FileProviders.Physical.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.FileSystemGlobbing.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Hosting.Abstractions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Hosting.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Http.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Identity.Core.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Identity.Stores.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Localization.Abstractions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Localization.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Logging.Abstractions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Logging.Configuration.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Logging.Console.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Logging.Debug.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Logging.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Logging.EventLog.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Logging.EventSource.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Logging.TraceSource.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.ObjectPool.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Options.ConfigurationExtensions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Options.DataAnnotations.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Options.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.Primitives.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Extensions.WebEncoders.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.JSInterop.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Net.Http.Headers.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\Microsoft.VisualBasic.Core.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\Microsoft.VisualBasic.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Win32.Primitives.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\Microsoft.Win32.Registry.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\mscorlib.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\netstandard.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.AppContext.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Buffers.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Collections.Concurrent.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Collections.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Collections.Immutable.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Collections.NonGeneric.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Collections.Specialized.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.ComponentModel.Annotations.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.ComponentModel.DataAnnotations.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.ComponentModel.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.ComponentModel.EventBasedAsync.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.ComponentModel.Primitives.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.ComponentModel.TypeConverter.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Configuration.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Console.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Core.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Data.Common.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Data.DataSetExtensions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Data.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Diagnostics.Contracts.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Diagnostics.Debug.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Diagnostics.DiagnosticSource.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\System.Diagnostics.EventLog.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Diagnostics.FileVersionInfo.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Diagnostics.Process.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Diagnostics.StackTrace.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Diagnostics.TextWriterTraceListener.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Diagnostics.Tools.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Diagnostics.TraceSource.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Diagnostics.Tracing.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Drawing.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Drawing.Primitives.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Dynamic.Runtime.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Formats.Asn1.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Globalization.Calendars.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Globalization.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Globalization.Extensions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.IO.Compression.Brotli.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.IO.Compression.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.IO.Compression.FileSystem.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.IO.Compression.ZipFile.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.IO.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.IO.FileSystem.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.IO.FileSystem.DriveInfo.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.IO.FileSystem.Primitives.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.IO.FileSystem.Watcher.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.IO.IsolatedStorage.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.IO.MemoryMappedFiles.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\System.IO.Pipelines.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.IO.Pipes.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.IO.UnmanagedMemoryStream.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Linq.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Linq.Expressions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Linq.Parallel.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Linq.Queryable.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Memory.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Net.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Net.Http.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Net.Http.Json.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Net.HttpListener.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Net.Mail.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Net.NameResolution.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Net.NetworkInformation.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Net.Ping.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Net.Primitives.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Net.Requests.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Net.Security.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Net.ServicePoint.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Net.Sockets.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Net.WebClient.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Net.WebHeaderCollection.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Net.WebProxy.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Net.WebSockets.Client.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Net.WebSockets.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Numerics.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Numerics.Vectors.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.ObjectModel.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Reflection.DispatchProxy.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Reflection.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Reflection.Emit.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Reflection.Emit.ILGeneration.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Reflection.Emit.Lightweight.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Reflection.Extensions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Reflection.Metadata.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Reflection.Primitives.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Reflection.TypeExtensions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Resources.Reader.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Resources.ResourceManager.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Resources.Writer.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Runtime.CompilerServices.Unsafe.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Runtime.CompilerServices.VisualC.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Runtime.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Runtime.Extensions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Runtime.Handles.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Runtime.InteropServices.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Runtime.InteropServices.RuntimeInformation.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Runtime.Intrinsics.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Runtime.Loader.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Runtime.Numerics.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Runtime.Serialization.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Runtime.Serialization.Formatters.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Runtime.Serialization.Json.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Runtime.Serialization.Primitives.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Runtime.Serialization.Xml.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\System.Security.AccessControl.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Security.Claims.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Security.Cryptography.Algorithms.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\System.Security.Cryptography.Cng.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Security.Cryptography.Csp.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Security.Cryptography.Encoding.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Security.Cryptography.Primitives.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Security.Cryptography.X509Certificates.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\System.Security.Cryptography.Xml.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Security.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\System.Security.Permissions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Security.Principal.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\System.Security.Principal.Windows.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Security.SecureString.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.ServiceModel.Web.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.ServiceProcess.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Text.Encoding.CodePages.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Text.Encoding.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Text.Encoding.Extensions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Text.Encodings.Web.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Text.Json.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Text.RegularExpressions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Threading.Channels.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Threading.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Threading.Overlapped.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Threading.Tasks.Dataflow.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Threading.Tasks.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Threading.Tasks.Extensions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Threading.Tasks.Parallel.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Threading.Thread.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Threading.ThreadPool.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Threading.Timer.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Transactions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Transactions.Local.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.ValueTuple.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Web.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Web.HttpUtility.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Windows.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.AspNetCore.App.Ref\5.0.0\ref\net5.0\System.Windows.Extensions.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Xml.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Xml.Linq.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Xml.ReaderWriter.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Xml.Serialization.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Xml.XDocument.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Xml.XmlDocument.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Xml.XmlSerializer.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Xml.XPath.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\System.Xml.XPath.XDocument.dll"
                    @"c:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\5.0.0\ref\net5.0\WindowsBase.dll"
                ]
            for r in references do
                  yield "-r:" + r
            for x in fileNames do 
                yield x

            let nugets =
                [ 
                    @"fsharp.core\5.0.0\lib\netstandard2.0\FSharp.Core.dll"
                    @"htmlagilitypack\1.11.0\lib\netstandard2.0\HtmlAgilityPack.dll"
                    @"system.xml.xpath.xmldocument\4.3.0\ref\netstandard1.3\System.Xml.XPath.XmlDocument.dll"
                ]

            yield "-g"
            yield "--debug:portable"
            yield "--noframework"
            yield "--define:TRACE"
            yield "--define:DEBUG"
            yield "--define:NET"
            yield "--define:NET5_0"
            yield "--define:NETCOREAPP"
            yield "--define:NET5_0_OR_GREATER"
            yield "--define:NETCOREAPP1_0_OR_GREATER"
            yield "--define:NETCOREAPP1_1_OR_GREATER"
            yield "--define:NETCOREAPP2_0_OR_GREATER"
            yield "--define:NETCOREAPP2_1_OR_GREATER"
            yield "--define:NETCOREAPP2_2_OR_GREATER"
            yield "--define:NETCOREAPP3_0_OR_GREATER"
            yield "--define:NETCOREAPP3_1_OR_GREATER"
            yield "--optimize-"
            yield "--tailcalls-"
            yield "--target:exe"
            yield "--warn:3"
            yield "--warnaserror:3239"
            yield "--fullpaths"
            yield "--flaterrors"
            yield "--highentropyva+"
            yield "--targetprofile:netcore"
            yield "--nocopyfsharpcore"
            yield "--deterministic+"
            yield "--simpleresolution"

            for n in nugets do
                yield @"-r:" + System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile) + @"\.nuget\packages\" + n


            let projRefs =
                [
                    @"..\..\build\Debug\netstandard2.0\WebSharper.AspNetCore.dll"
                    @"..\..\build\Debug\netstandard2.0\WebSharper.Collections.dll"
                    @"..\..\build\Debug\netstandard2.0\WebSharper.Control.dll"
                    @"..\..\build\Debug\netstandard2.0\WebSharper.Core.dll"
                    @"..\..\build\Debug\netstandard2.0\WebSharper.Core.JavaScript.dll"
                    @"..\..\build\Debug\netstandard2.0\WebSharper.InterfaceGenerator.dll"
                    @"..\..\build\Debug\netstandard2.0\WebSharper.JavaScript.dll"
                    @"..\..\build\Debug\netstandard2.0\WebSharper.JQuery.dll"
                    @"..\..\build\Debug\netstandard2.0\WebSharper.Main.dll"
                    @"..\..\build\Debug\netstandard2.0\WebSharper.MathJS.dll"
                    @"..\..\build\Debug\netstandard2.0\WebSharper.MathJS.Extensions.dll"
                    @"..\..\build\Debug\netstandard2.0\WebSharper.Sitelets.dll"
                    @"..\..\build\Debug\Tests\netstandard2.0\WebSharper.Sitelets.Tests.dll"
                    @"..\..\build\Debug\netstandard2.0\WebSharper.Testing.dll"
                    @"..\..\build\Debug\netstandard2.0\WebSharper.Web.dll"
                ]
            for projRef in projRefs do
                yield @"-r:" + Path.Combine(__SOURCE_DIRECTORY__, projRef)

            yield "--out:" + dllName
            yield "--project:" + Path.Combine(__SOURCE_DIRECTORY__, @"..\GeneratedProject\Preview.fsproj")
            yield "--wsconfig:" + Path.Combine(__SOURCE_DIRECTORY__, @"..\GeneratedProject\wsconfig.json")
        |]

    let fileNames =
        [
            Path.Combine(__SOURCE_DIRECTORY__, @"..\GeneratedProject\obj\Debug\net5.0\Preview.AssemblyInfo.fs")
            Path.Combine(__SOURCE_DIRECTORY__, @"..\GeneratedProject\example1.abc.fs")
            Path.Combine(__SOURCE_DIRECTORY__, @"..\GeneratedProject\Site.fs")
            Path.Combine(__SOURCE_DIRECTORY__, @"..\GeneratedProject\Startup.fs")
        ]
    let args = mkProjectCommandLineArgs (@"bin\example1.abc.dll", fileNames)

    System.Environment.CurrentDirectory <- Path.Combine(__SOURCE_DIRECTORY__, @"..\GeneratedProject\")
    let memCache = MemoryCache.Default


    let checker = FSharpChecker.Create(keepAssemblyContents = true)
    let checkerFactory() = checker

    let tryGetMetadata (r: WebSharper.Compiler.FrontEnd.Assembly) =
        match r.LoadPath with
        // memCache.[<non-existent key>] won't error. It's returning null, if the key is not present.
        | Some x when memCache.[x] = null -> 
            match WebSharper.Compiler.FrontEnd.TryReadFromAssembly WebSharper.Compiler.FrontEnd.ReadOptions.FullMetadata r with
            | None ->
                None
            | result ->
                let policy = CacheItemPolicy()
                let monitor = new HostFileChangeMonitor([| x |])
                policy.ChangeMonitors.Add monitor
                memCache.Set(x, result, policy)
                memCache.[x] :?> Result<WebSharper.Core.Metadata.Info, string> option
        | Some x ->
            memCache.[x] :?> Result<WebSharper.Core.Metadata.Info, string> option
        | None ->
            // in-memory assembly may have no path. nLogger. 
            // nLogger.Trace "Reading assembly: %s" x makes this compilation fail. No Tracing here.
            WebSharper.Compiler.FrontEnd.TryReadFromAssembly WebSharper.Compiler.FrontEnd.ReadOptions.FullMetadata r


    let logger = {
        new LoggerBase() with 
            member _.Out s =
                printfn "%s" s
            member _.Error s =
                eprintfn "%s" s
            }

    let parsedOptions = ParseOptions args logger
    match parsedOptions with
    | HelpOrCommand _ ->
        failwith "it shouldn't be help or command"
    | ParsedOptions (wsConfig, warnSettings) ->
        let compile() = 
            StandAloneCompile wsConfig warnSettings logger checkerFactory tryGetMetadata
        compile()
        |> should equal 0

        let stopWatch = System.Diagnostics.Stopwatch.StartNew()
        compile()
        |> should equal 0
        stopWatch.Stop()

        stopWatch.Elapsed |> should lessThan (System.TimeSpan.FromSeconds(4.0))
