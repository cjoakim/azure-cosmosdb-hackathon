# azure-cosmosdb-hackathon

## Git and GitHub Pull-Request Instructions

```
# Start from the latest version of the master branch:
$ git checkout master
$ git reset --hard
$ git pull

# Create a "Feature Branch" to do your work on:
$ git branch user-feature-name  (example: cj-challenge1-java or bh-challenge4-dotnet)
$ git checkout user-feature-name
$ git branch
$ git push -u origin user-feature-name

... make edits to your feature branch, commit and push them ...
... create a pull-request, merge it ...
... then delete the local branch with: git branch -d user-feature-name

Or, execute **$ ./newbranch.sh user-feature-name** to do the above.

# Other useful git commands:
$ git branch
$ git branch -a
$ git branch -r
$ git gui &
$ git status
$ git reset --hard
$ git gc

# Squash Merging; alternative to Pull-Requests:
... create and do commits on the xxx branch ...
$ git checkout master
$ git merge --squash feature_branch
$ git commit -m "Squash-merged the xxx branch"
$ git push
```
