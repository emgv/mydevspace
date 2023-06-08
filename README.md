# mydevspace

Exploring automation ideas to improve the dev workflow by using the APIs from Azure DevOps and GitHub, for now some of the ideas to explore are:
- Attach personal comments and metadata to WorkItems / Issues
- Automatic alerts:
  - When a comment has been made in a PR
  - When a PR is approved and it's waiting for the author to complete
  - When a PR is created run scripts to performs validations like:
    - Check for duplicates in the localization resource files
    - Execute code analysis checks
    - These validations could be done in the CI/CD, but sometimes we do not have the permissions or we just want to test something
