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

[dotnet]: https://dot.net/
[node-js]: https://nodejs.org/
