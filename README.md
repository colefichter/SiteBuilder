Site Builder tool
=================

This tool is a very lightweight site building tool for simple HTML brochure sites.

Usage
-----
Place the contents of the Release build folder into the root of your HTML website.

Create a folder called `_PARTIALS` in the root of your HTML website.
Create partial files in this folder using the `.part` file extension and place code snippets in them.

For example in `footer.part`:

```html
<footer>
    Copyright 2024
</footer
```

In your HTML pages, use a partials tag like `{{{ partial: footer }}}` to reference the partial you want:

```html
<html>
<head>...</head>
<body>
    ...
    {{{ partial: footer }}}
</body>
```

Finally, run the tool to copy & compile files. It will output the entire built site to `\DIST\`:

`> .\SiteBuilder.exe`

Platform
--------

This tool requires a DotNet 8.0 installation. It's compiled for Win11, and may or may not work elsewhere.