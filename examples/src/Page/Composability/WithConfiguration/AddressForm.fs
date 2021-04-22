module Page.Composability.WithConfiguration.AddressForm

open Warded.Simple

type AddressForm<'Values> =
    Form.Form<'Values, Address.T>

type Values =
    {
        Country : string
        City : string
        PostalCode : string
    }

let blank =
    {
        Country = ""
        City = ""
        PostalCode = ""
    }

type Config<'A> =
    {
        Get : 'A -> Values
        Update : Values -> 'A -> 'A
    }

let form
    ({ Get = get; Update = update} : Config<'A>)
    : AddressForm<'A> =

    let updateField fn value values =
        update (fn value (get values)) values

    let countryField =
        Form.textField
            {
                // Use a custom EmailAddress parser and map the result back into a string
                // as the email field is represented using a string in the Form
                Parser =
                    Address.Country.tryParse
                Value =
                    get >> fun values -> values.Country
                Update =
                    updateField (fun newValue values -> { values with Country = newValue })
                Error =
                    fun _ -> None
                Attributes =
                    {
                        Label = "Country"
                        Placeholder = ""
                    }
            }

    let cityField =
        Form.textField
            {
                // Use a custom EmailAddress parser and map the result back into a string
                // as the email field is represented using a string in the Form
                Parser =
                    Address.City.tryParse
                Value =
                    get >> fun values -> values.City
                Update =
                    updateField (fun newValue values -> { values with City = newValue })
                Error =
                    fun _ -> None
                Attributes =
                    {
                        Label = "City"
                        Placeholder = ""
                    }
            }

    let postalCodeField =
        Form.textField
            {
                Parser =
                    Address.PostalCode.tryParse
                Value =
                    get >> fun values -> values.PostalCode
                Update =
                    updateField (fun newValue values -> { values with PostalCode = newValue })
                Error =
                    fun _ -> None
                Attributes =
                    {
                        Label = "PostalCode"
                        Placeholder = ""
                    }
            }

    Form.succeed Address.create
        |> Form.append countryField
        |> Form.append cityField
        |> Form.append postalCodeField