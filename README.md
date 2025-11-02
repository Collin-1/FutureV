# FutureV

A .NET 9 Razor Pages eCommerce experience showcasing speculative futuristic vehicles. FutureV highlights cinematic landing visuals, immersive vehicle stories, and a lightweight admin deck secured by a passphrase for live CRUD operations.

## Highlights

- **Catalog storytelling**: Landing page tiles and detailed vehicle pages mix specs with narrative copy and multi-angle imagery.
- **Admin deck**: Passphrase-protected CRUD workflows for vehicles and their image galleries with client-side helpers for dynamic image inputs.
- **PostgreSQL persistence**: Entity Framework Core with seeding for three concept cars and cascade image management.
- **Responsive styling**: Futuristic gradient palette, card polish, and carousel previews powered by Bootstrap 5 and custom CSS.

## Tech Stack

- .NET 9 Razor Pages
- Entity Framework Core 9 + PostgreSQL (Npgsql)
- Bootstrap 5 + custom theming

## Getting Started

1. **Install prerequisites**

   - [.NET SDK 9.0](https://dotnet.microsoft.com/download)
   - PostgreSQL 16+ locally, or a managed instance such as [Neon](https://neon.tech)

2. **Configure the connection string**

   Update `appsettings.Development.json` (local) or `appsettings.json` (deployment) with credentials for your PostgreSQL instance. The defaults target `postgres:postgres` on `localhost` using a database named `futurev_dev`.

3. **Restore & build**

   ```powershell
   dotnet restore
   dotnet build
   ```

4. **Apply migrations & seed data**

   ```powershell
   dotnet ef database update
   ```

   The first launch runs additional seeding logic in `SeedData.InitializeAsync`.

5. **Run the site**
   ```powershell
   dotnet run
   ```
   Navigate to `https://localhost:5001` (or the port reported in the console).

## Deploying to Render

- **Prerequisites**: A free Render account and access to the Render Dashboard.
- **Blueprint deploy**: Connect this repository and pick _Deploy from render.yaml_. The blueprint provisions a free web service and managed PostgreSQL instance.
- **First deploy flow**:
  1.  Render runs the `Dockerfile`, publishing the app to a container listening on port 8080.
  2.  The managed database connection string is injected via `ConnectionStrings__DefaultConnection` / `DATABASE_URL`.
  3.  After the container starts, Render performs a health check on `/` and your site goes live at the Render-provided URL.
- **Database migrations**: In Render’s _Shell_ tab (or via `render-cli`), run `dotnet ef database update` to apply the latest migrations.
- **Configuration knobs**: Override settings with additional environment variables under _Environment_ → _Add Environment Variable_. Common additions include `ADMIN_PASSPHRASE` and `Logging__LogLevel__Default`.

## Admin Access

- Visit `/admin/login` and enter the passphrase `admin` ("thin" per the design brief).
- Once authenticated you can add, edit, preview, and delete vehicles plus manage multi-angle imagery.
- Use the **Sign out** button in the admin header to revoke the secure cookie.

## Project Structure

- `Data/` – EF Core `ApplicationDbContext` and database seed routine.
- `Models/` – Domain entities for cars and associated images.
- `Pages/` – Razor Pages for the storefront, catalog, contact form, and admin deck.
- `ViewModels/` – Input models backing the admin forms.
- `wwwroot/` – Static assets including the custom `site.css` theme.

## Development Notes

- The project uses collection expressions (`[]`) and other C# 12 features enabled by .NET 9.
- Dynamic image fields in the admin UI reindex automatically to keep model binding stable.
- Carousel components gracefully fall back when images are missing, keeping the UX polished even with sparse data.

## Next Steps

- Wire up a real contact pipeline (email or CRM integration).
- Add filtering and comparison tools to the public catalog.
- Expand admin authentication to ASP.NET Core Identity when ready for multi-user scenarios.
