# Land of Forums

A forum app built with ASP.NET Core 2.0. Uses ASP.NET Core Identity for user membership and Razor pages to show the information from the database.
```c#
public Forum GetById(int id)
        {
            var forum = _context.Forums.Where(f => f.Id == id)
                .Include(f => f.Posts)
                    .ThenInclude(p => p.User)
                .Include(f => f.Posts)
                    .ThenInclude(p => p.Replies)
                        .ThenInclude(r => r.User)
                .FirstOrDefault();
            return forum;
        }
```
