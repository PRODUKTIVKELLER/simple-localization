# Simple Localization
**Simple Localization** is a light-weight tool to add localization to your unity projects. It is completely free to use.

## One Datasheet for all languages

**Simple Localization** allows you to do localization in a single document, that is very straightforward to use.

![Screenshot showing installation with package manager](./Images~/add-package-from-git-url.png)

It contains one column for the localization-keys and one column for every language in the project.

### Installation

First install the [Git Dependency Resolver For Unity]("https://github.com/mob-sakai/GitDependencyResolverForUnity") by adding the following lines to your `manifest.json`:


```json
{
  "dependencies": {
    "com.coffee.git-dependency-resolver": "https://github.com/mob-sakai/GitDependencyResolverForUnity.git"
  }
}
```

*Other ways to install the [Git Dependency Resolver For Unity]("https://github.com/mob-sakai/GitDependencyResolverForUnity") can be found on their GitHub page.*

Then install **Simple Localization** by adding the following lines to your `manifest.json`:

```json
{
  "dependencies": {
    "com.produktivkeller.simple-audio-solution": "https://github.com/PRODUKTIVKELLER/simple-localization.git"
  }
}
```

Alternatively you can also import a package from GIT Url with the **package manager**:

![Screenshot showing installation with package manager](./Images~/add-package-from-git-url.png)