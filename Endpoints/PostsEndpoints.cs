using PersonalWebsiteMVC.Data;

// https://codewithmukesh.com/blog/aspnet-core-webapi-crud-with-entity-framework-core-full-course/
public static class PostsEndpoints
{
    public static void MapPostsEndpoints(this IEndpointRouteBuilder routes)
    {
        var postsApi = routes.MapGroup("/api/posts");

        postsApi.MapGet("/", (ApplicationDbContext db) =>
        {
            var posts = db.Posts;
            return posts;
        });
    }
}