Technetium Contributor Guide
============================

Development Prerequisites
-------------------------
To develop Technetium, you'll need [.NET 7 SDK][dotnet] (or a later version) and [Node.js][node-js] 18 (or a later version).

Build the Project
-----------------
Execute the following shell command:

```console
$ dotnet build
```

### Continuously Build the Frontend
If required, run the following shell command to automatically rebuild the frontend code on changes:
```console
$ npm run watch
```

Run Tests
---------
To run backend tests, execute the following shell command:
```console
$ dotnet test
```

To run frontend tests once, execute the following shell command in the `Technetium.Frontend` directory:
```console
$ npm test
```

To run the frontend tests in watch mode, execute the following shell command in the `Technetium.Frontend` directory:
```console
$ npm run test-watch
```

Create a Database Migration
---------------------------
When modifying the database structure, you'll need to create a new migration. To do that, run the following shell commands:

```console
$ dotnet tool restore
$ cd Technetium.Data
$ dotnet ef migrations add <migration-name>
```

[dotnet]: https://dot.net/
[node-js]: https://nodejs.org/
