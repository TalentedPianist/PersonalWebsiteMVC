#pragma checksum "/Volumes/Data/Websites/PersonalWebsiteMVC/Areas/Admin/Views/Gallery/Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "45d47e7ff42ed51061805998d02a3214de12d596"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Admin_Views_Gallery_Index), @"mvc.1.0.view", @"/Areas/Admin/Views/Gallery/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "/Volumes/Data/Websites/PersonalWebsiteMVC/Areas/Admin/Views/_ViewImports.cshtml"
using PersonalWebsiteMVC.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "/Volumes/Data/Websites/PersonalWebsiteMVC/Areas/Admin/Views/_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "/Volumes/Data/Websites/PersonalWebsiteMVC/Areas/Admin/Views/_ViewImports.cshtml"
using Microsoft.EntityFrameworkCore;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "/Volumes/Data/Websites/PersonalWebsiteMVC/Areas/Admin/Views/Gallery/Index.cshtml"
using PersonalWebsiteMVC.Helpers;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "/Volumes/Data/Websites/PersonalWebsiteMVC/Areas/Admin/Views/Gallery/Index.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "/Volumes/Data/Websites/PersonalWebsiteMVC/Areas/Admin/Views/Gallery/Index.cshtml"
using PersonalWebsiteMVC.Data;

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "/Volumes/Data/Websites/PersonalWebsiteMVC/Areas/Admin/Views/Gallery/Index.cshtml"
using System.Linq;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"45d47e7ff42ed51061805998d02a3214de12d596", @"/Areas/Admin/Views/Gallery/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c08ccd86c98340427f57f8e5a5e1ae6ac572d505", @"/Areas/Admin/Views/_ViewImports.cshtml")]
    public class Areas_Admin_Views_Gallery_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<PersonalWebsiteMVC.Models.Gallery>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Album", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-primary"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 4 "/Volumes/Data/Websites/PersonalWebsiteMVC/Areas/Admin/Views/Gallery/Index.cshtml"
  
    Layout = "~/Areas/Admin/Shared/_Layout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\n");
#nullable restore
#line 16 "/Volumes/Data/Websites/PersonalWebsiteMVC/Areas/Admin/Views/Gallery/Index.cshtml"
  
    string id = "01PTFRO2O4BZ2ACOZGKND2H3VV6RF5DVEG";
    var rootFolder = _Graph.GraphClient().Result.Drives["douglas@douglasmcgregor.co.uk"].Items[id].Children.Request().Expand("thumbnails").GetAsync().Result;
    for (int i = 0; i < rootFolder.Count(); i++)
    {
        var albumId = rootFolder[i].Id;
        var album = _Graph.GraphClient().Result.Drives["douglas@douglasmcgregor.co.uk"].Items[albumId].Children.Request().Expand("thumbnails").GetAsync().Result;
        var data = new Dictionary<string, string>
{
                    {"q", albumId },
                    {"title", rootFolder[i].Name }
               };
        foreach (var item in album[i].Thumbnails)
        {
            var random = new Random();
            var index = random.Next(album.Count());
            var cover = album[index].Thumbnails.Select(t => t.Medium.Url).FirstOrDefault();
            var cover1 = album[index].Thumbnails.Select(t => t.Large.Url).FirstOrDefault();




#line default
#line hidden
#nullable disable
            WriteLiteral("            <div class=\"Gallery\">\n                <h2>");
#nullable restore
#line 38 "/Volumes/Data/Websites/PersonalWebsiteMVC/Areas/Admin/Views/Gallery/Index.cshtml"
               Write(rootFolder[i].Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h2>\n                <a");
            BeginWriteAttribute("id", " id=\"", 1498, "\"", 1513, 2);
            WriteAttributeValue("", 1503, "button1-", 1503, 8, true);
#nullable restore
#line 39 "/Volumes/Data/Websites/PersonalWebsiteMVC/Areas/Admin/Views/Gallery/Index.cshtml"
WriteAttributeValue("", 1511, i, 1511, 2, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" data-toggle=\"modal\" data-target=\"#exampleModal-");
#nullable restore
#line 39 "/Volumes/Data/Websites/PersonalWebsiteMVC/Areas/Admin/Views/Gallery/Index.cshtml"
                                                                             Write(i);

#line default
#line hidden
#nullable disable
            WriteLiteral("\"><img");
            BeginWriteAttribute("src", " src=\"", 1570, "\"", 1582, 1);
#nullable restore
#line 39 "/Volumes/Data/Websites/PersonalWebsiteMVC/Areas/Admin/Views/Gallery/Index.cshtml"
WriteAttributeValue("", 1576, cover, 1576, 6, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"image\"></a>\n                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "45d47e7ff42ed51061805998d02a3214de12d5967489", async() => {
                WriteLiteral("View");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "id", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 1626, "button2-", 1626, 8, true);
#nullable restore
#line 40 "/Volumes/Data/Websites/PersonalWebsiteMVC/Areas/Admin/Views/Gallery/Index.cshtml"
AddHtmlAttributeValue("", 1634, i, 1634, 2, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#nullable restore
#line 40 "/Volumes/Data/Websites/PersonalWebsiteMVC/Areas/Admin/Views/Gallery/Index.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues = data;

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-all-route-data", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\n\n            </div>\n");
            WriteLiteral("            <!-- Modal -->\n            <div class=\"modal fade\"");
            BeginWriteAttribute("id", " id=\"", 1800, "\"", 1820, 2);
            WriteAttributeValue("", 1805, "exampleModal-", 1805, 13, true);
#nullable restore
#line 45 "/Volumes/Data/Websites/PersonalWebsiteMVC/Areas/Admin/Views/Gallery/Index.cshtml"
WriteAttributeValue("", 1818, i, 1818, 2, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" tabindex=""-1"" role=""dialog"" aria-labelledby=""exampleModalLabel"" aria-hidden=""true"">
                <div class=""modal-dialog modal-xl"" role=""document"">
                    <div class=""modal-content"">
                        <div class=""modal-header"">
                            <h2 class=""modal-title"" id=""exampleModalLabel"">");
#nullable restore
#line 49 "/Volumes/Data/Websites/PersonalWebsiteMVC/Areas/Admin/Views/Gallery/Index.cshtml"
                                                                      Write(rootFolder[i].Name);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</h2>
                            <button type=""button"" class=""close"" data-dismiss=""modal"" aria-label=""Close"">
                                <span aria-hidden=""true"">&times;</span>
                            </button>
                        </div>
                        <div class=""modal-body"">
                            <img");
            BeginWriteAttribute("src", " src=\"", 2500, "\"", 2513, 1);
#nullable restore
#line 55 "/Volumes/Data/Websites/PersonalWebsiteMVC/Areas/Admin/Views/Gallery/Index.cshtml"
WriteAttributeValue("", 2506, cover1, 2506, 7, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\n                        </div>\n                        <div class=\"modal-footer justify-content-between\">\n                            ");
#nullable restore
#line 58 "/Volumes/Data/Websites/PersonalWebsiteMVC/Areas/Admin/Views/Gallery/Index.cshtml"
                       Write(await Component.InvokeAsync("Gallery", new { galleryId = rootFolder[i].Id, galleryName = rootFolder[i].Name }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\n                        </div>\n                    </div>\n                </div>\n            </div>\n");
            WriteLiteral("            <script>\n\t\t\t $(document).ready(function () {\n\t\t\t\t$(\'#button1-");
#nullable restore
#line 66 "/Volumes/Data/Websites/PersonalWebsiteMVC/Areas/Admin/Views/Gallery/Index.cshtml"
                       Write(i);

#line default
#line hidden
#nullable disable
            WriteLiteral("\').on(\"click\", function () {\n\t\t\t\t    //$(\'#exampleModal-");
#nullable restore
#line 67 "/Volumes/Data/Websites/PersonalWebsiteMVC/Areas/Admin/Views/Gallery/Index.cshtml"
                                  Write(i);

#line default
#line hidden
#nullable disable
            WriteLiteral("\').modal(\'show\');\n\t\t\t\t});\n                 \n             \n\t\t\t });\n            </script>\n");
#nullable restore
#line 73 "/Volumes/Data/Websites/PersonalWebsiteMVC/Areas/Admin/Views/Gallery/Index.cshtml"
        }

    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public ApplicationDbContext db { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IHttpContextAccessor http { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IGraphSDKHelper _Graph { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<PersonalWebsiteMVC.Models.Gallery> Html { get; private set; }
    }
}
#pragma warning restore 1591