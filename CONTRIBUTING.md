# Contributing

## Setting up your development environment

### Code

1. Clone this repository.
2. Open the solution with **Visual Studio 2017** or **Rider**.
3. Add the Google `ClientId` and `ClientSecret` to your appsettings.json.

### Database

1. Download and install [PostgreSQL 10.5](https://www.postgresql.org/download/) on your machine for local development.
2. Use **pgAdmin 4** to create a new database for this project.
3. Add a connection string for this database to your appsettings.json.
    - An example of a connection string might be `Host=localhost;Username=postgres;Password=;Database=;`.
4. Add this same connection string to the appsettings.json of the ToBeRenamed.Database project.
5. Run the ToBeRenamed.Database project. This will apply all migrations to setup an initial database.

You can now build and run the web application.

## Contributing to the project

1. Create a branch off of `master` called something descriptive like `AddUserLibraries`, preferably in PascalCase.
2. Commit your changes to this branch. Use descriptive commit messages. Or at the very least, your messages should start with an uppercase, present-tense verb. [Here are some tips.](https://chris.beams.io/posts/git-commit/)
3. When your feature or bug fix is ready for peer review, submit a pull request on GitHub to merge your changes into master. Do not squash your commits.

I recommend **pgAdmin 4** for writing SQL and interacting with your local database.