Technetium [![Status Zero][status-zero]][andivionian-status-classifier]
==========

Technetium is time- and task-management software.

Configuration
-------------
To use Technetium.Console, you'll need a Google API credentials.

1. Create a Google Cloud Project.
2. On the **Enabled APIs & services** page, choose **Enable APIs and Services**, search for **Google Tasks API** and choose **Enable**.
3. On the **Google Tasks API** page, click **Create Credentials**.
4. On the **Which API are you using?** step, choose **User data**.
5. On the **OAuth Consent Screen** step, configure it whatever you want.
6. On the **Scopes** step, choose Google Tasks API (the non-readonly variant), `https://www.googleapis.com/auth/tasks`.
7. On the **OAuth Client ID** step, choose **Desktop app**. Specify the name as `Technetium.Console`.
8. Download the resulting JSON file.
9. Remember to add the users you want to use the application on the **OAuth consent screen** page, in the **Test users** list.

Usage
-----

> [!WARNING]
>
> Note that currently Technetium.Console is just a demo application that authenticates in Google, but doesn't do anything else.

Run Technetium.Console using this shell command.

```console
$ dotnet run --project Technetium.Console -- <user-name> <client-secret-file> <configuration-file>
```

Here `username` is Google user email, e.g. `example@gmail.com`. 

Documentation
-------------

- [Contributor Guide][docs.contributing]
- [License (MIT)][docs.license]
- [Code of Conduct (adopted from the Contributor Covenant)][docs.code-of-conduct]

[andivionian-status-classifier]: https://github.com/ForNeVeR/andivionian-status-classifier#status-zero-
[docs.code-of-conduct]: CODE_OF_CONDUCT.md
[docs.contributing]: CONTRIBUTING.md
[docs.license]: LICENSE.md
[status-zero]: https://img.shields.io/badge/status-zero-lightgrey.svg
