NOTE: Readme in production is a different file and still important.

In production, uniqueStyles has been minified and put in a style tag directly in index.html
In production, all the whitespace in LessonGame has been removed and it has been placed into index.html.

Please make sure to make edits to the original uncompressed files and then compress them to another file named as the ones in production.

ALWAYS KEEP A COPY OF THE UNCOMPRESSED/NON-MINIFIED FILES.

Reasoning:

The reason the files are minified and some files have been combined in the production version is for 2 reasons. The first reason is file sizes,
the less bandwidth we use, the better. In reality the quizes being created are not an intergral part of a lesson and should not be the reason
the page takes forever to load. The second reason is http requests. A webpage has to request each file, which takes time and there is a limit
on how many can be downloaded at the same time. By reducing the number of files, it takes less time to download the content. The reason all the
css and js files have not been compressed into one is due to the fact that the files are not always garenteed to be used. A game does not need to
use both game modes and therefore there is no reason to download both modes if there is no need, I made the decision to rather have a few extra
http requests if they use both game modes vs extra bandwidth if they do not use both.
