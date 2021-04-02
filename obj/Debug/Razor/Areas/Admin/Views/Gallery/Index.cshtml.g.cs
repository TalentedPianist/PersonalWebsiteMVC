#pragma checksum "E:\Source\repos\PersonalWebsiteMVC\Areas\Admin\Views\Gallery\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ad4484f6113a211afdac527d0c71f4d30d63a5b8"
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
#line 1 "E:\Source\repos\PersonalWebsiteMVC\Areas\Admin\Views\_ViewImports.cshtml"
using PersonalWebsiteMVC.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "E:\Source\repos\PersonalWebsiteMVC\Areas\Admin\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "E:\Source\repos\PersonalWebsiteMVC\Areas\Admin\Views\_ViewImports.cshtml"
using Microsoft.EntityFrameworkCore;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "E:\Source\repos\PersonalWebsiteMVC\Areas\Admin\Views\Gallery\Index.cshtml"
using PersonalWebsiteMVC.Helpers;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "E:\Source\repos\PersonalWebsiteMVC\Areas\Admin\Views\Gallery\Index.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "E:\Source\repos\PersonalWebsiteMVC\Areas\Admin\Views\Gallery\Index.cshtml"
using PersonalWebsiteMVC.Data;

#line default
#line hidden
#nullable disable
#nullable restore
#line 14 "E:\Source\repos\PersonalWebsiteMVC\Areas\Admin\Views\Gallery\Index.cshtml"
using System.Linq;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ad4484f6113a211afdac527d0c71f4d30d63a5b8", @"/Areas/Admin/Views/Gallery/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"baf0051efc1e48433fe6cc17dd71c3e61b8e19d6", @"/Areas/Admin/Views/_ViewImports.cshtml")]
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
#line 4 "E:\Source\repos\PersonalWebsiteMVC\Areas\Admin\Views\Gallery\Index.cshtml"
  
    Layout = "~/Areas/Admin/Shared/_Layout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 16 "E:\Source\repos\PersonalWebsiteMVC\Areas\Admin\Views\Gallery\Index.cshtml"
  
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
            WriteLiteral("            <div class=\"Gallery\">\r\n                <h2>");
#nullable restore
#line 38 "E:\Source\repos\PersonalWebsiteMVC\Areas\Admin\Views\Gallery\Index.cshtml"
               Write(rootFolder[i].Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h2>\r\n                <a");
            BeginWriteAttribute("id", " id=\"", 1536, "\"", 1551, 2);
            WriteAttributeValue("", 1541, "button1-", 1541, 8, true);
#nullable restore
#line 39 "E:\Source\repos\PersonalWebsiteMVC\Areas\Admin\Views\Gallery\Index.cshtml"
WriteAttributeValue("", 1549, i, 1549, 2, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" data-toggle=\"modal\" data-target=\"#exampleModal-");
#nullable restore
#line 39 "E:\Source\repos\PersonalWebsiteMVC\Areas\Admin\Views\Gallery\Index.cshtml"
                                                                             Write(i);

#line default
#line hidden
#nullable disable
            WriteLiteral("\"><img");
            BeginWriteAttribute("src", " src=\"", 1608, "\"", 1620, 1);
#nullable restore
#line 39 "E:\Source\repos\PersonalWebsiteMVC\Areas\Admin\Views\Gallery\Index.cshtml"
WriteAttributeValue("", 1614, cover, 1614, 6, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"image\"></a>\r\n                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ad4484f6113a211afdac527d0c71f4d30d63a5b87565", async() => {
                WriteLiteral("View");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "id", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 1665, "button2-", 1665, 8, true);
#nullable restore
#line 40 "E:\Source\repos\PersonalWebsiteMVC\Areas\Admin\Views\Gallery\Index.cshtml"
AddHtmlAttributeValue("", 1673, i, 1673, 2, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#nullable restore
#line 40 "E:\Source\repos\PersonalWebsiteMVC\Areas\Admin\Views\Gallery\Index.cshtml"
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
            WriteLiteral("\r\n\r\n            </div>\r\n");
            WriteLiteral("            <!-- Modal -->\r\n            <div class=\"modal fade\"");
            BeginWriteAttribute("id", " id=\"", 1844, "\"", 1864, 2);
            WriteAttributeValue("", 1849, "exampleModal-", 1849, 13, true);
#nullable restore
#line 45 "E:\Source\repos\PersonalWebsiteMVC\Areas\Admin\Views\Gallery\Index.cshtml"
WriteAttributeValue("", 1862, i, 1862, 2, false);

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
#line 49 "E:\Source\repos\PersonalWebsiteMVC\Areas\Admin\Views\Gallery\Index.cshtml"
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
            BeginWriteAttribute("src", " src=\"", 2554, "\"", 2567, 1);
#nullable restore
#line 55 "E:\Source\repos\PersonalWebsiteMVC\Areas\Admin\Views\Gallery\Index.cshtml"
WriteAttributeValue("", 2560, cover1, 2560, 7, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                        </div>\r\n                        <div class=\"modal-footer justify-content-between\">\r\n                            ");
#nullable restore
#line 58 "E:\Source\repos\PersonalWebsiteMVC\Areas\Admin\Views\Gallery\Index.cshtml"
                       Write(await Component.InvokeAsync("Gallery", new { galleryId = rootFolder[i].Id, galleryName = rootFolder[i].Name }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n");
            WriteLiteral("            <script>\r\n\t\t\t $(document).ready(function () {\r\n\t\t\t\t$(\'#button1-");
#nullable restore
#line 66 "E:\Source\repos\PersonalWebsiteMVC\Areas\Admin\Views\Gallery\Index.cshtml"
                       Write(i);

#line default
#line hidden
#nullable disable
            WriteLiteral("\').on(\"click\", function () {\r\n\t\t\t\t    //$(\'#exampleModal-");
#nullable restore
#line 67 "E:\Source\repos\PersonalWebsiteMVC\Areas\Admin\Views\Gallery\Index.cshtml"
                                  Write(i);

#line default
#line hidden
#nullable disable
            WriteLiteral("\').modal(\'show\');\r\n\t\t\t\t});\r\n                 \r\n             \r\n\t\t\t });\r\n            </script>\r\n");
#nullable restore
#line 73 "E:\Source\repos\PersonalWebsiteMVC\Areas\Admin\Views\Gallery\Index.cshtml"
        }

    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
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
