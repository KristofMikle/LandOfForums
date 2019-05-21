# Land of Forums

A forum app built with ASP.NET Core 2.0. Uses ASP.NET Core Identity for user membership and Razor pages to show the information from the database.

# Code

First I get the needed data from the Database.

In the Post Service:
```c#
public Post GetById(int id)
    {
        return _context.Posts.Where(post => post.Id == id)
            .Include(post => post.User)
            .Include(post => post.Replies)
                .ThenInclude(reply => reply.User)
            .Include(post => post.Forum)
            .First();
    }
```
Then I build a model that I can pass to the View, so I have access to all the information, that I need there.

In the Post Controlller:
```c#
public IActionResult Index(int id)
    {
        var post = _postService.GetById(id);

        var replies = BuildPostReplies(post.Replies);

        var model = new PostIndexModel
        {
            Id = post.Id,
            Title = post.Title,
            AuthorId = post.User.Id,
            AuthorName = post.User.UserName,
            AuthorImageURL = post.User.ProfileImageURL,
            AuthorRating = post.User.Rating,
            Created = post.Created,
            Content = post.Content,
            Replies = replies,
            ForumID = post.Forum.Id,
            ForumName = post.Forum.Title,
            IsAuthorAdmin= IsAuthorAdmin(post.User)
        };
        return View(model);
    }
```
Using the @model I can acces the information I passed from the controller.

In the Post/Index.cshtml:
```html
@model LandOfForums.Models.Post.PostIndexModel
...
<div class="row postHeader">
    <div class="postHeading">
        <span class="postIndexTitle">
            @Model.Title
        </span>
        <span id="headingBtn">
            <a asp-controller="Forum" asp-action="Topic" asp-route-id="@Model.ForumID" class="btn btn-back">
                    Back to @Model.ForumName
            </a>
        </span>
    </div>
</div>
...
```
