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
## Bind-API
- Add a "bind"-API: This bind parameter values to a config object
  - Add for all parameter definition types a ".BindingName(...)" method which defined a target property name
  - Instead of the final "Call"-call, add a "Bind(targetObj)"-call which binds all extracted parameters to an object.

## Predefined Validators
Validators:
- File (for string and FileInfo):
    - Validation.File.Exists(p)
    - Validation.File.NotExists(p)
    - BTW: Add FileInfo as s predefined parseable data type
- Number (for all numerical data types):
    - Validation.Number.GreaterThan(x)
    - Validation.Number.SmallerThan(x)
    - Validation.Number.Between(x, y)
- String:
    - Validation.Text.ShortThan(x)
    - Validation.Text.LongerThan(x)
    - Validation.Length.Bewteen(x, y)
    - Validation.Text.Matches(regex_str) (oder regex Objekt)

=> These are all IPredefinedValidation:
    - It implements IValidation
    - And with WithErrorMessage(x => "") oder WithErrorMessage("")
        - This allows to override the predefined error message

## Metadata-API
Add a "WithMetaData(key, value)" method to all configurable parameter types. This just adds an object with a
key to the parameter. Finally add a new "Call"-method that is called "CallUntypedWithMeta(IReadonlyCollection<(object parameter, IReadonlyDictionary<string, object>))".

This allows to add metadata to all parameters. But why? E.g. we can solve the binding-API thing with this:
- Create an extension method for configurable Parameters that is called "BindingName<TType, TPropType>(Expression<Func<TType, TPropType>> propCall)"
    - It just calls "WithMetaData"
- Create an Extension method "Bind<T>(objofTypeT)" which calls "CallUntypedWithMeta" and just uses all parameters to add them to the target object

We just solved the binding API problem with a more general solution.

before something like this is implemented, one might ask if there are other use-cases with meta-data. Because implementing an over-generalized solution
is usually stupid. I have to think about this.





