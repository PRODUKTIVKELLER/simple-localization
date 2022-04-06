# Simple Localization
**Simple Localization** is a light-weight tool to add localization to your unity projects and manage it in an excel sheet. It is completely free to use.

## One Datasheet for all languages

**Simple Localization** allows you to do localization in a single document, that is very straightforward to use.

![Screenshot showing structure of localization file.](./Images~/excel-example.png)

It contains one column for the localization-keys and one column for every language in the project.

## Getting Started

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

Alternatively you can also import the package from the GIT Url with the **package manager**:

![Screenshot showing installation with package manager](./Images~/add-package-from-git-url.png)


### Set-Up

1. Copy and paste the example prefab `Packages/com.produktivkeller.simple-audio-solution/Prefabs/LocalizationService` to your `Assets` folder. Otherwise, your changes to the prefab will be overriden by updates of **Simple Localization**.

2. Make sure you have an excel document that contains your localization in the right format (the name of the sheet has to be "Localization" and it must contain the columns shown in the image above). If you want an easy template to work with, you can import it from the "Samples" section of this package and move it to the folder `Assets/StreamingAssets`.

3. Drag and drop the prefab from step one to your scene.

4. That's it!