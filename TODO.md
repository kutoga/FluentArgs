# TODO
- Finalize doc
  - Mention why `DefaultConfigs...(...)` is a good choise
- Finetune error messages
  - If a validation fails (see Help01.cs), then the error looks a bit ugly
- What should happen if a parser failes? Escpecially custom parsers
  - Currently the application just crashes
- If examples are given and a validation is available: apply the validation to all examples
  - If this fails, the examples are crap!
- Use FakeItEasy
- Add an icon (for NuGet)
- Add tags
  - Maybe even more metadata for the package?

# Ideas
- Add a "bind"-API: This bind parameter values to a config object
  - Add for all parameter definition types a ".BindingName(...)" method which defined a target property name
  - Instead of the final "Call"-call, add a "Bind(targetObj)"-call which binds all extracted parameters to an object.


