using Microsoft.EntityFrameworkCore;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;

public class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

        if (context == null || context.Posts == null)
        {
            throw new NullReferenceException(
                "Posts table is null");
        }

        if (context.Posts.Any())
        {
            return;
        }

//        context.Posts.AddRange(
//            new Posts
//            {
//                PostTitle = "IIS Express",
//                PostContent = "IIS Express, a lightweight (4.5–6.6 MB) version of IIS, is available as a standalone freeware server and may be installed on Windows XP with Service Pack 3 and subsequent versions of Microsoft Windows. IIS 7.5 Express supports only the HTTP and HTTPS protocols. It is portable, stores its configuration on a per-user basis, does not require administrative privileges and attempts to avoid conflicting with existing web servers on the same machine.[36] IIS Express can be downloaded separately[37] or as a part of WebMatrix[38] or Visual Studio 2012 and later.[39] (In Visual Studio 2010 and earlier, web developers developing ASP.NET apps used ASP.NET Development Server, codenamed \"Cassini\".)[40] By default, IIS Express only serves local traffic.[41][39]",
//                PostDate = DateTime.Now,
//                PostAuthor = "Douglas McGregor"
//            },
//            new Posts
//            {
//                PostTitle = "IIS Extensions",
//                PostContent = @"IIS releases new feature modules between major version release to add new functionality. The following extensions are available for IIS 7.5:

//FTP Publishing Service: Lets Web content creators publish content securely to IIS 7 Web servers with SSL-based authentication and data transfer.[42]
//Administration Pack: Adds administration UI support for management features in IIS 7, including ASP.NET authorization, custom errors, FastCGI configuration, and request filtering.[43]
//Application Request Routing: Provides a proxy-based routing module that forwards HTTP requests to content servers based on HTTP headers, server variables, and load balance algorithms.[44]
//Database Manager: Allows easy management of local and remote databases from within IIS Manager.[45]
//Media Services: Integrates a media delivery platform with IIS to manage and administer the delivery of rich media and other Web content.[46]
//URL Rewrite Module: Provides a rule-based rewriting mechanism for changing request URLs before they are processed by the Web server.[47]
//WebDAV: Lets Web authors publish content securely to IIS 7 Web servers, and lets Web administrators and hosters manage WebDAV settings using IIS 7 management and configuration tools.[48]
//Web Deployment Tool: Synchronizes IIS 6.0 and IIS 7 servers, migrates an IIS 6.0 server to IIS 7, and deploys Web applications to an IIS 7 server.[49]",
//                PostDate = DateTime.Now,
//                PostAuthor = "Douglas McGregor"
//            },
//            new Posts
//            {
//                PostTitle = "IIS Security", 
//                PostContent = @"IIS 4 and IIS 5 were affected by the CA-2001-13 security vulnerability which led to the infamous Code Red attack;[53][54] however, both versions 6.0 and 7.0 have no reported issues with this specific vulnerability.[55] In IIS 6.0 Microsoft opted to change the behaviour of pre-installed ISAPI handlers,[56] many of which were culprits in the vulnerabilities of 4.0 and 5.0, thus reducing the attack surface of IIS.[54] In addition, IIS 6.0 added a feature called ""Web Service Extensions"" that prevents IIS from launching any program without explicit permission by an administrator.

//By default IIS 5.1 and earlier run websites in a single process running the context of the System account,[57] a Windows account with administrative rights. Under 6.0 all request handling processes run in the context of the Network Service account, which has significantly fewer privileges, so should there be a vulnerability in a feature or custom code it won't necessarily compromise the entire system given the sandboxed environment these worker processes run in.[58] IIS 6.0 also contained a new kernel HTTP stack (http.sys) with a stricter HTTP request parser and response cache for both static and dynamic content.[59]

//According to Secunia, as of June 2011, IIS 7 had a total of six resolved vulnerabilities while[55] IIS 6 had a total of eleven vulnerabilities, out of which one was still unpatched. The unpatched security advisory has a severity rating of 2 out of 5.[55]

//In June 2007, a Google study of 80 million domains concluded that while the IIS market share was 23% at the time, IIS servers hosted 49% of the world's malware, the same as Apache servers whose market share was 66%. The study also observed the geographical location of these dirty servers and suggested that the cause of this could be the use of unlicensed copies of Windows that could not obtain security updates from Microsoft.[60] In a blog post on 28 April 2009, Microsoft noted that it supplies security updates to everyone without genuine verification.[61][62]

//The 2013 mass surveillance disclosures made it more widely known that IIS is particularly bad in supporting perfect forward secrecy (PFS), especially when used in conjunction with Internet Explorer. Possessing one of the long term asymmetric secret keys used to establish a HTTPS session should not make it easier to derive the short term session key to then decrypt the conversation, even at a later time. Diffie–Hellman key exchange (DHE) and elliptic curve Diffie–Hellman key exchange (ECDHE) are in 2013 the only ones known to have that property. Only 30% of Firefox, Opera, and Chromium Browser sessions use it, and nearly 0% of Apple's Safari and Microsoft Internet Explorer sessions.[63]",
//                PostDate = DateTime.Now,
//                PostAuthor = "Douglas McGregor"
//            },
//new Posts
//{
//    PostTitle = "IIS Usage",
//    PostContent = "According to Netcraft, in February 2014, IIS had a \"market share of all sites\" of 32.80%, making it the second most popular web server in the world, behind Apache HTTP Server at 38.22%. Netcraft showed a rising trend in market share for IIS, since 2012.[50] On 14 February 2014, however, the W3Techs shows different results. According to W3Techs, IIS is the third most used web server behind Apache HTTP Server (1st place) and Nginx. Furthermore, it shows a consistently falling trend for IIS use since February 2013.[51]\n\nNetcraft data in February 2017 indicates IIS had a \"market share of the top million busiest sites\" of 10.19%, making it the third most popular web server in the world, behind Apache at 41.41% and nginx at 28.34%.[52]",
//    PostDate = DateTime.Now,
//    PostAuthor = "Douglas McGregor"
//}
//        );

        context.SaveChanges();
    }
}